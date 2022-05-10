using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRectTransformHandle : MonoBehaviour, IBeginDragHandler, IDragHandler {
    [SerializeField] private RectTransform targetTransform;

    private Vector2 lastDragPosition;

    public void OnBeginDrag(PointerEventData eventData) {
        lastDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 dragDiff = eventData.position - lastDragPosition;

        targetTransform.position += new Vector3(dragDiff.x, dragDiff.y, 0);

        lastDragPosition = eventData.position;
    }
}