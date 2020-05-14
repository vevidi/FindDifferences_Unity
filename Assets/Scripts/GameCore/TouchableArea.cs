using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameLogic
{
    public class TouchableArea : MonoBehaviour, ICanvasRaycastFilter, IPointerClickHandler
    {
        private DifferenceInfoModel model;
        private RectTransform rectTransform;
        private float radius = 50f;

        public void Init(DifferenceInfoModel model, int offsetX = 0, int offsetY = 0)
        {
            this.model = model;
            radius = model.Radius * 0.8f; // IMPORTANT! multiplier added temporary for debug
            rectTransform.sizeDelta = Vector2.one * radius * 2;
            rectTransform.localPosition = new Vector2(model.X + offsetX, model.Y + offsetY);
            rectTransform.localScale = Vector3.one;
        }

        public int GetId()
        {
            return model.Id;
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnClick()
        {
            Debug.Log("On click");
            GameManager.Instance.gameEventSystem.Publish(new DiffFoundCommand(model,this));
            gameObject.SetActive(false);
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            Vector2 screenPoint = eventCamera.WorldToScreenPoint(rectTransform.position);
            bool isClickable = Vector2.Distance(sp, screenPoint) < radius;
            return isClickable;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }
    }
}