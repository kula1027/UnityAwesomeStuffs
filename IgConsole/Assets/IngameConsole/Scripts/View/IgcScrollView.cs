using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameConsole {

    [RequireComponent(typeof(ScrollRect))]
    public class IgcScrollView : MonoBehaviour, IDragHandler, IEndDragHandler {
        private const float SLIDER_LOCK_THRESHOLD = 0.002f;

        public ScrollRect ScrollRect { get; private set; }

        private readonly List<ConsoleChunk> consoleChunks = new List<ConsoleChunk>(32);

        [SerializeField] private ConsoleDetailedViewer detailedViewer;
        [SerializeField] private GameObject pfLogTextPreset;
        [SerializeField] private GameObject pfLogImagePreset;

        private ConsoleChunk cntChunk;
        private ObjectPooler poolerImageText;
        private ObjectPooler poolerLogText;

        private bool            lockSliderToZero = true;
        private TextMeshProUGUI tmpPreset;

        public void Initialize() {
            ScrollRect = GetComponent<ScrollRect>();

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

        public void Add(ConsoleData data) {
            var targetChunk = GetTargetChunk();

            targetChunk.Add(data);

            ScrollRect.content.sizeDelta = new Vector2(
                ScrollRect.content.sizeDelta.x,
                -targetChunk.Bottom
            );

            VisibilityCheck(targetChunk);

            if (lockSliderToZero && gameObject.activeInHierarchy) {
                ScrollRect.verticalNormalizedPosition = 0;
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

            ScrollRect.content.sizeDelta = new Vector2(
                ScrollRect.content.sizeDelta.x,
                -cntChunk.Bottom
            );

            VisibilityCheckAll();
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

                for (var loop = 0; loop < arrSplit.Length; loop++) {
                    if (arrCount <= dataIdx) {
                        break;
                    }

                    arrSplit[loop] = arrData[dataIdx];

                    dataIdx++;
                }

                chunk.UpdateArray(arrSplit);
                consoleChunks.Add(chunk);

                top = chunk.Bottom;
            }

            cntChunk = consoleChunks[consoleChunks.Count - 1];

            ScrollRect.content.sizeDelta = new Vector2(
                ScrollRect.content.sizeDelta.x,
                -cntChunk.Bottom
            );

            VisibilityCheckAll();
        }

        public void Clear() {
            consoleChunks.ForEach(chunk => chunk.Disable());
            consoleChunks.Clear();

            cntChunk = null;

            ScrollRect.content.sizeDelta = new Vector2(
                ScrollRect.content.sizeDelta.x,
                0
            );
        }

        private void VisibilityCheckAll() {
            foreach (var chunk in consoleChunks) {
                VisibilityCheck(chunk);
            }
        }

        private void VisibilityCheck(ConsoleChunk chunk) {
            var visibleTop    = -ScrollRect.content.anchoredPosition.y;
            var visibleBottom = -ScrollRect.content.anchoredPosition.y - ScrollRect.viewport.rect.height;

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

            var s = data.Msg.Length > 0 ? data.Msg : " ";

            return tmpPreset.GetPreferredValues(
                    s,
                    ScrollRect.content.rect.width,
                    0
                ).y;
        }

        private ConsoleText CreateConsoleText(ConsoleData data, float posY) {
            var consoleText = data.HasSprite
                ? poolerImageText.TakeThis().GetComponent<ConsoleImageText>()
                : poolerLogText.TakeThis().GetComponent<ConsoleText>();

            consoleText.ConsoleData = data;

            consoleText.transform.SetParent(ScrollRect.content.transform);
            consoleText.transform.localScale = Vector3.one;
            consoleText.RectTransform.anchoredPosition = new Vector2(0, posY);
            consoleText.UpdateRectTransform();

            return consoleText;
        }

        #region UI Interactions

        private bool isDragging = false;

        public void OnScrollSliderValueChanged(Vector2 val) {
            lockSliderToZero = val.y < SLIDER_LOCK_THRESHOLD;

            if (ScrollRect.verticalScrollbar.size < 0.05f) {
                ScrollRect.verticalScrollbar.size = 0.05f;
            }

            VisibilityCheckAll();
        }

        private void OnClickText(ConsoleText consoleText) {
            if (isDragging == false) {
                this.detailedViewer.Show(consoleText.ConsoleData);
            }
        }

        public void UnlockSliderOnScroll(BaseEventData evt) {
            if (lockSliderToZero && Input.mouseScrollDelta.y > 0) {
                lockSliderToZero = false;
                ScrollRect.verticalNormalizedPosition = SLIDER_LOCK_THRESHOLD + 0.0005f;
            }
        }

        public void OnDrag(PointerEventData eventData) {
            isDragging = true;
            UnlockSliderOnDrag(eventData.delta.y);
        }

        public void UnlockSliderOnDrag(float yDelta) {
            if (lockSliderToZero && yDelta < 0) {
                lockSliderToZero = false;
                ScrollRect.verticalNormalizedPosition = SLIDER_LOCK_THRESHOLD + 0.0005f;
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            isDragging = false;
        }

        #endregion
    }
}