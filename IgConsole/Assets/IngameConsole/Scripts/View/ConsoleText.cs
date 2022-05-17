using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameConsole {

    public class ConsoleText : PoolableObject, IPointerClickHandler {

        protected ConsoleData consoleData;

        [SerializeField] protected TextMeshProUGUI tmpText;
        [SerializeField] protected Image imgBackground;

        protected RectTransform rectTransform;

        public TextMeshProUGUI TmpText => tmpText;

        public RectTransform RectTransform => rectTransform;

        public Color BgColor {
            get => imgBackground.color;
            set => imgBackground.color = value;
        }

        public ConsoleData ConsoleData {
            get => consoleData;
            set {
                consoleData = value;

                OnDataUpdated();
            }
        }

        public Action<ConsoleText> OnClick { get; set; }

        public float Height => rectTransform.sizeDelta.y;


        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void OnDataUpdated() {
            tmpText.text = consoleData.Msg;
            tmpText.color = IgConsole.LogColor(consoleData.LogType);
        }

        public virtual void UpdateRectTransform() {
            RectTransform parentRect= transform.parent.GetComponent<RectTransform>();

            var s = consoleData.Msg.Length > 0 ? consoleData.Msg : " ";
            float prfHeight = tmpText.GetPreferredValues(s, parentRect.rect.width, 0).y;

            rectTransform.sizeDelta = new Vector2(0, prfHeight);
        }

        public void OnPointerClick(PointerEventData eventData) {
            OnClick?.Invoke(this);
        }
    }
}