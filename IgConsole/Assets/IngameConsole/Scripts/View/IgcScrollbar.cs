using UnityEngine;
using UnityEngine.EventSystems;

namespace IngameConsole {
    public class IgcScrollbar : MonoBehaviour, IDragHandler, IEndDragHandler {
        [SerializeField] private IgcScrollView scrollView;

        public void OnDrag(PointerEventData eventData) {
            scrollView.UnlockSliderOnDrag(-eventData.delta.y);
        }

        public void OnEndDrag(PointerEventData eventData) {
            scrollView.OnEndDrag(eventData);
        }
    }
}