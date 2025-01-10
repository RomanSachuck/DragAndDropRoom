using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.InputService
{
    public class MobileInput : IInput
    {
        private const int FirstTouch = 0;
        
        public event Action<Vector3> ClickUp;
        public event Action<Vector3> ClickDown;
        public event Action<Vector3> Drag;

        private Vector2 _startDragPosition;
        
        public Vector3 CursorPosition =>
            Input.GetTouch(FirstTouch).position;

        public void Tick()
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(FirstTouch);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startDragPosition = touch.position;
                        ClickDown?.Invoke(_startDragPosition);
                        break;

                    case TouchPhase.Moved:
                        Drag?.Invoke(touch.position - _startDragPosition);
                        _startDragPosition = touch.position;
                        break;

                    case TouchPhase.Ended:
                        ClickUp?.Invoke(touch.position);
                        break;
                }
            }
        }
    }
}