using System;
using UnityEngine.EventSystems;

namespace CodeBase.DragAndDropSystem
{
    public class EventTriggerExample : EventTrigger
    {
        public event Action PointerDown;
        public event Action PointerUp;
        
        public override void OnPointerDown( PointerEventData data ) => 
            PointerDown?.Invoke();

        public override void OnPointerUp( PointerEventData data ) => 
            PointerUp?.Invoke();
    }
}