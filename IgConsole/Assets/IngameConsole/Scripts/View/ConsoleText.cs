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

        public TextMeshProUGUI TmpText {
            get => tmpText;
        }

        public RectTransform RectTransform {
            get { return rectTransform; }
        }

        public Color BgColor {
            get {
                return imgBackground.color;
            }
            set {
                imgBackground.color = value;
            }
        }

        public ConsoleData ConsoleData {
            get {
                return consoleData;
            }
            set {
                consoleData = value;

                OnDataUpdated();
            }
        }

        public Action<ConsoleText> OnClick { get; set; }

        public float Height {
            get { return rectTransform.sizeDelta.y; }
        }


        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void OnDataUpdated() {
            tmpText.text = consoleData.Msg;
            tmpText.color = IgConsole.LogColor(consoleData.LogType);
        }

        public virtual void UpdatePreferredHeight() {
            RectTransform parentRect= transform.parent.GetComponent<RectTransform>();

            float prfHeight = tmpText.GetPreferredValues(consoleData.Msg, parentRect.rect.width, 0).y;

            rectTransform.sizeDelta = new Vector2(0, prfHeight);
        }

        public void OnPointerClick(PointerEventData eventData) {
            OnClick?.Invoke(this);
        }

    }
}