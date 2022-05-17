using UnityEngine;
using UnityEngine.EventSystems;

public class DragRectTransformHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [SerializeField] private RectTransform targetTransform;

    private Vector2 lastDragPosition;

    public bool IsDragging { get; private set; } = false;

    public void OnBeginDrag(PointerEventData eventData) {
        lastDragPosition = eventData.position;
        IsDragging = true;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 dragDiff = eventData.position - lastDragPosition;

        targetTransform.position += new Vector3(dragDiff.x, dragDiff.y, 0);

        lastDragPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        IsDragging = false;
    }
}