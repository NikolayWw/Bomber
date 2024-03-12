using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.Windows.HUD.Input
{
    public class InputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action<bool> OnClick;

        public void OnPointerDown(PointerEventData eventData) => 
            OnClick?.Invoke(true);

        public void OnPointerUp(PointerEventData eventData) => 
            OnClick?.Invoke(false);
    }
}