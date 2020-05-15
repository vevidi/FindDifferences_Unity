using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vevidi.FindDiff.GameLogic
{
    public class ClickableBackground : MonoBehaviour, IPointerDownHandler 
    {
        public Action<PointerEventData> OnBackgroundClick = delegate { };

        public void OnPointerDown(PointerEventData eventData)
        {
            OnBackgroundClick?.Invoke(eventData);
        }
    }
}