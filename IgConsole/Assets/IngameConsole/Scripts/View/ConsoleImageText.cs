using UnityEngine;
using UnityEngine.UI;

namespace IngameConsole {

    public class ConsoleImageText : ConsoleText {
        public const float FixedHeight = 148;
        [SerializeField] protected Image image;

        protected override void OnDataUpdated() {
            base.OnDataUpdated();

            image.sprite = consoleData.Sprite;
        }

        private void Awake() {
            imgBackground.preserveAspect = true;
            rectTransform = GetComponent<RectTransform>();
        }

        public override void UpdateRectTransform() {
            rectTransform.sizeDelta = new Vector2(0, FixedHeight);
        }
    }
}