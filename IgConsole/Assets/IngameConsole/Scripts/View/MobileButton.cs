using UnityEngine;
using UnityEngine.EventSystems;

namespace IngameConsole {
    public class MobileButton : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private ConsoleViewer viewer;

        private DragRectTransformHandle dragHandle;

        private void Awake() {
            dragHandle = GetComponent<DragRectTransformHandle>();

            viewer.OnHide += () => gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (dragHandle.IsDragging) {
                return;
            }

            viewer.Show();
            gameObject.SetActive(false);
        }
    }
}