using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace IngameConsole {

    [RequireComponent(typeof(ScrollRect))]
    public class IgcScrollView : MonoBehaviour {
        [SerializeField] private ConsoleDetailedViewer detailedViewer;
        [SerializeField] private GameObject pfLogTextPreset;
        [SerializeField] private GameObject pfLogImagePreset;

        private readonly List<ConsoleChunk> consoleChunks = new List<ConsoleChunk>(32);

        private ConsoleChunk cntChunk;
        private ObjectPooler poolerImageText;
        private ObjectPooler poolerLogText;

        private readonly List<float> preferredHeightCache = new List<float>();

        private bool            resetSliderOnUpdate = true;
        private ScrollRect      scrollRect;
        private TextMeshProUGUI tmpPreset;

        public void Initialize() {
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener(OnScrollSliderValueChanged);

            poolerLogText = new GameObject("TextPool").AddComponent<ObjectPooler>();
            poolerLogText.transform.SetParent(transform);
            poolerLogText.Initialize(pfLogTextPreset, OnSpawnConsoleText);

            tmpPreset = pfLogTextPreset.GetComponent<ConsoleText>().TmpText;

            poolerImageText = new GameObject("ImagePool").AddComponent<ObjectPooler>();
            poolerImageText.transform.SetParent(transform);
            poolerImageText.Initialize(pfLogImagePreset, OnSpawnConsoleText);
        }

        private void OnSpawnConsoleText(GameObject go) {
            ConsoleText consoleText = go.GetComponent<ConsoleText>();

            consoleText.OnClick = OnClickText;
        }

        private void OnClickText(ConsoleText consoleText) {
            detailedViewer.Show(
               consoleText.ConsoleData.Msg + "\n\n" +
               consoleText.ConsoleData.Detailed + "\n" +
               IgConsole.LogStartTime.AddSeconds(consoleText.ConsoleData.RealtimeSinceStartup).ToString(ConsoleDetailedViewer.TIME_FORMAT));
        }

        public void Add(ConsoleData data) {
            var targetChunk = GetTargetChunk();

            targetChunk.Add(data);

            scrollRect.content.sizeDelta = new Vector2(
                scrollRect.content.sizeDelta.x,
                -targetChunk.Bottom
            );

            VisibilityCheck(targetChunk);

            if (resetSliderOnUpdate && gameObject.activeInHierarchy) {
                scrollRect.verticalNormalizedPosition = 0;
            }
        }

        public void Rebuild() {
            if (consoleChunks.Count <= 0) {
                return;
            }

            float top = 0;
            for (var loop = 0; loop < consoleChunks.Count; loop++) {
                var toBeReplaced = consoleChunks[loop];

                consoleChunks[loop] = CreateConsoleChunk(top);
                consoleChunks[loop].UpdateArray(toBeReplaced.ConsoleDatas);
                top = consoleChunks[loop].Bottom;

                toBeReplaced.Disable();
            }

            cntChunk = consoleChunks[consoleChunks.Count - 1];

            scrollRect.content.sizeDelta = new Vector2(
                scrollRect.content.sizeDelta.x,
                -cntChunk.Bottom
            );
        }

        public void ResetData(List<ConsoleData> arrData) {
            Clear();

            if (arrData.Count() <= 0) {
                cntChunk = CreateConsoleChunk(0);
                consoleChunks.Add(cntChunk);
                VisibilityCheck(cntChunk);
                return;
            }

            int arrCount = arrData.Count();
            float top = 0;
            var dataIdx = 0;
            while (dataIdx < arrCount) {
                var arrSplit = new ConsoleData[ConsoleChunk.MaxCountPerChunk];
                var chunk = CreateConsoleChunk(top);

                Profiler.BeginSample("LoopLoopLoop");
                for (var loop = 0; loop < arrSplit.Length; loop++) {
                    if (arrCount <= dataIdx) {
                        break;
                    }

                    arrSplit[loop] = arrData[dataIdx];

                    dataIdx++;
                }
                Profiler.EndSample();

                chunk.UpdateArray(arrSplit);
                consoleChunks.Add(chunk);

                top = chunk.Bottom;
            }

            cntChunk = consoleChunks[consoleChunks.Count - 1];

            scrollRect.content.sizeDelta = new Vector2(
                scrollRect.content.sizeDelta.x,
                -cntChunk.Bottom
            );

            VisibilityCheckAll();

            Profiler.EndSample();
        }

        public void Clear() {
            consoleChunks.ForEach(chunk => chunk.Disable());
            consoleChunks.Clear();

            cntChunk = null;

            scrollRect.content.sizeDelta = new Vector2(
                scrollRect.content.sizeDelta.x,
                0
            );
        }

        private void OnScrollSliderValueChanged(Vector2 val) {
            resetSliderOnUpdate = val.y < 0.001f || scrollRect.verticalScrollbar.size > 0.999f;

            if (scrollRect.verticalScrollbar.size < 0.05f) {
                scrollRect.verticalScrollbar.size = 0.05f;
            }

            VisibilityCheckAll();
        }

        private void VisibilityCheckAll() {
            foreach (var chunk in consoleChunks) {
                VisibilityCheck(chunk);
            }
        }

        private void VisibilityCheck(ConsoleChunk chunk) {
            var visibleTop    = -scrollRect.content.anchoredPosition.y;
            var visibleBottom = -scrollRect.content.anchoredPosition.y - scrollRect.viewport.rect.height;

            if (chunk.VisibilityCheck(visibleTop, visibleBottom)) {
                chunk.Enable();
            } else {
                chunk.Disable();
            }
        }

        private ConsoleChunk GetTargetChunk() {
            ConsoleChunk chunk;
            if (cntChunk == null) {
                chunk = CreateConsoleChunk(0);
            } else if (cntChunk.IsFull) {
                chunk = CreateConsoleChunk(cntChunk.Bottom);
            } else {
                return cntChunk;
            }

            consoleChunks.Add(chunk);

            VisibilityCheck(chunk);

            cntChunk = chunk;
            return cntChunk;
        }

        private ConsoleChunk CreateConsoleChunk(float top) {
            return new ConsoleChunk(
                top,
                CreateConsoleText,
                GetPreferredHeight
            );
        }

        private float GetPreferredHeight(ConsoleData data) {
            if (data.HasSprite) {
                return ConsoleImageText.FixedHeight;
            }
            preferredHeightCache.EnsureSize(data.LogId + 1, -1);

            if (preferredHeightCache[data.LogId] < 0) {
                var s = data.Msg.Length > 0 ? data.Msg : " ";
                var prfHeight = tmpPreset.GetPreferredValues(
                    s,
                    scrollRect.content.rect.width,
                    0
                ).y;

                preferredHeightCache[data.LogId] = prfHeight;
            }

            return preferredHeightCache[data.LogId];
        }

        private ConsoleText CreateConsoleText(ConsoleData data, float posY) {
            var consoleText = data.HasSprite
                ? poolerImageText.TakeThis().GetComponent<ConsoleImageText>()
                : poolerLogText.TakeThis().GetComponent<ConsoleText>();

            consoleText.ConsoleData = data;

            consoleText.transform.SetParent(scrollRect.content.transform);
            consoleText.RectTransform.anchoredPosition = new Vector2(0, posY);
            consoleText.UpdatePreferredHeight();

            return consoleText;
        }
    }
}