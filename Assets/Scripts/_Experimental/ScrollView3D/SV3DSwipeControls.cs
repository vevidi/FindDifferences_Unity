using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Vevidi.Experimental
{
    public class SV3DSwipeControls : MonoBehaviour, IEndDragHandler, IDragHandler
    {
        public enum eDraggedDirection
        {
            Up,
            Down,
            Right,
            Left
        }

        public enum eDragType
        {
            Horizontal,
            Vertical,
            Both
        }

        [SerializeField]
        private eDragType dragType = eDragType.Horizontal;
        [SerializeField]
        private float distanceTresholdPercent = 0.3f;
        [SerializeField]
        private ScrollView3D scrollView3D;

        private RectTransform area;
        private float areaWidth;
        private float areaHeight;

        private void Awake()
        {
            area = GetComponent<RectTransform>();
            areaWidth = area.rect.width;
            areaHeight = area.rect.height;
        }

        private eDraggedDirection GetDragDirection(Vector3 dragVector)
        {
            float positiveX = Mathf.Abs(dragVector.x);
            float positiveY = Mathf.Abs(dragVector.y);
            eDraggedDirection draggedDir;
            if (dragType == eDragType.Both)
            {
                if (positiveX > positiveY)
                    draggedDir = (dragVector.x > 0) ? eDraggedDirection.Right : eDraggedDirection.Left;
                else
                    draggedDir = (dragVector.y > 0) ? eDraggedDirection.Up : eDraggedDirection.Down;
            }
            else if (dragType == eDragType.Horizontal)
                draggedDir = (dragVector.x > 0) ? eDraggedDirection.Right : eDraggedDirection.Left;
            else
                draggedDir = (dragVector.y > 0) ? eDraggedDirection.Up : eDraggedDirection.Down;

            Debug.Log(draggedDir);
            return draggedDir;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
 //           Debug.LogWarning((eventData.position - eventData.pressPosition).magnitude);
            if (dragType == eDragType.Both)
            {
                float dist = (eventData.position - eventData.pressPosition).magnitude;
                if (dist >= distanceTresholdPercent * new Vector2(areaWidth, areaHeight).magnitude)
                    DragDone(GetDragDirection(dragVectorDirection));
            }
            else if(dragType == eDragType.Horizontal)
            {
                float dist = (new Vector2(eventData.position.x,0) - new Vector2(eventData.pressPosition.x, 0)).magnitude;
                if (dist >= distanceTresholdPercent * areaWidth)
                    DragDone(GetDragDirection(dragVectorDirection));
            }
            else
            {
                float dist = (new Vector2(eventData.position.y, 0) - new Vector2(eventData.pressPosition.y, 0)).magnitude;
                if (dist >= distanceTresholdPercent * areaHeight)
                    DragDone(GetDragDirection(dragVectorDirection));
            }
            //DragDone(GetDragDirection(dragVectorDirection));
        }

        // HMM... Looks strange, but IEndDragHandler dont fire without IDragHandler o_O
        public void OnDrag(PointerEventData eventData)
        {

        }

        private void DragDone(eDraggedDirection direction)
        {
            switch(direction)
            {
                case eDraggedDirection.Left:
                    scrollView3D.SwipeLeft(); break;
                case eDraggedDirection.Right:
                    scrollView3D.SwipeRight(); break;
            }
        }
    }
}