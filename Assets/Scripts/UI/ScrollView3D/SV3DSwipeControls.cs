using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.FindDiff.UI
{
    public class SV3DSwipeControls : MonoBehaviour, IEndDragHandler, IBeginDragHandler,
                                     IDragHandler, IPointerClickHandler
    {
        public enum eDragDirection
        {
            Up,
            Down,
            Right,
            Left,
            Unknown
        }

        public enum eDragType
        {
            Horizontal,
            Vertical,
            Both
        }

#pragma warning disable 0649
        [SerializeField]
        private eDragType dragType = eDragType.Horizontal;
        [SerializeField]
        private float distanceTresholdPercent = 0.3f;
        [SerializeField]
        private float timeTresholdMs = 300f;
        [SerializeField]
        private float maxMoveDelta = 0.03f;
#pragma warning restore 0649

        private RectTransform area;
        private float areaWidth;
        private float areaHeight;
        private ScrollView3D scrollView3D;

        private void Awake()
        {
            area = GetComponent<RectTransform>();
            areaWidth = area.rect.width;
            areaHeight = area.rect.height;
            scrollView3D = GetComponentInParent<ScrollView3D>();
        }

        private eDragDirection GetDragDirection(Vector3 dragVector)
        {
            float positiveX = Mathf.Abs(dragVector.x);
            float positiveY = Mathf.Abs(dragVector.y);
            switch (dragType)
            {
                case eDragType.Both:
                {
                    if (positiveX > positiveY)
                        return (dragVector.x > 0) ? eDragDirection.Right : eDragDirection.Left;
                    else
                        return (dragVector.y > 0) ? eDragDirection.Up : eDragDirection.Down;
                }
                case eDragType.Horizontal:
                    return (dragVector.x > 0) ? eDragDirection.Right : eDragDirection.Left;
                case eDragType.Vertical:
                    return (dragVector.y > 0) ? eDragDirection.Up : eDragDirection.Down;
            }
            return eDragDirection.Unknown;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;

            switch (dragType)
            {
                case eDragType.Both:
                {
                    float dist = (eventData.position - eventData.pressPosition).magnitude;
                    bool distanceReached = dist >= distanceTresholdPercent * new Vector2(areaWidth, areaHeight).magnitude;
                    DragDone(GetDragDirection(dragVectorDirection), distanceReached);
                    break;
                }
                case eDragType.Horizontal:
                {
                    float dist = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
                    bool distanceReached = dist >= distanceTresholdPercent * areaWidth;
                    DragDone(GetDragDirection(dragVectorDirection), distanceReached);
                    break;
                }
                case eDragType.Vertical:
                {
                    float dist = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
                    bool distanceReached = dist >= distanceTresholdPercent * areaHeight;
                    DragDone(GetDragDirection(dragVectorDirection), distanceReached);
                    break;
                }
            }
            isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            startTime = DateTime.Now;
            Vector3 dragVectorDirection = eventData.delta;
            scrollView3D.DragStarted(GetDragDirection(dragVectorDirection));
        }

        private bool isDragging = false;
        private DateTime startTime;

        public void OnDrag(PointerEventData eventData)
        {
            // TODO: add code for other directions if needed, only horizontal just nor needed
            Vector3 dragVectorDirection = eventData.delta;
            float dist = dragVectorDirection.x;
            eDragDirection dragDirection = GetDragDirection(dragVectorDirection);
            switch (dragDirection)
            {
                case eDragDirection.Left:
                {
                    float moveDelta = dist / Screen.width;
                    if (moveDelta < -maxMoveDelta)
                        moveDelta = -maxMoveDelta;
                    scrollView3D.MoveLeftPercent(moveDelta * 2.2f);
                    break;
                }
                case eDragDirection.Right:
                {
                    float moveDelta = dist / Screen.width;
                    if (moveDelta > maxMoveDelta)
                        moveDelta = maxMoveDelta;
                    scrollView3D.MoveRightPercent(moveDelta * 2.2f);
                    break;
                }
            }
        }

        private void DragDone(eDragDirection direction, bool distanceReached = false)
        {
            var timeSpan = DateTime.Now.Subtract(startTime);
            var timeSpanMs = timeSpan.TotalMilliseconds;
            Utils.DebugLog(timeSpanMs + " " + timeTresholdMs);
            scrollView3D.ResetCurrentMoveValue();

            if (timeSpanMs <= timeTresholdMs && distanceReached)
            {
                switch (direction)
                {
                    case eDragDirection.Left:
                        scrollView3D.SwipeLeft(); break;
                    case eDragDirection.Right:
                        scrollView3D.SwipeRight(); break;
                }
            }
            else
            {
                Debug.Log("AR IT");
                scrollView3D.ArrangeItemsMoveFinger();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isDragging)
                scrollView3D.DoClick(eventData.pressPosition);
        }
    }
}