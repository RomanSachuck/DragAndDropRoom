using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.InputService
{
    public class DesktopInput : IInput
    {
        private const int LeftMouseButton = 0;
        
        public event Action<Vector3> ClickUp;
        public event Action<Vector3> ClickDown;
        public event Action<Vector3> Drag;

        private bool _isSwiping;
        private Vector3 _startDragPosition;

        public Vector3 CursorPosition =>
            Input.mousePosition;
        
        public void Tick()
        {
            ProcessClickDown();
            ProcessClickUp();
            ProcessSwipe();
        }
        
        private void ProcessSwipe()
        {
            if (_isSwiping == false)
                return;

            Vector3 currentMousePosition = Input.mousePosition;

            if(currentMousePosition != _startDragPosition)   
                Drag?.Invoke(currentMousePosition - _startDragPosition);

            _startDragPosition = currentMousePosition;
        }

        private void ProcessClickUp()
        {
            if (Input.GetMouseButtonUp(LeftMouseButton))
            {
                _isSwiping = false;
                ClickUp?.Invoke(Input.mousePosition);
            }
        }

        private void ProcessClickDown()
        {
            if (Input.GetMouseButtonDown(LeftMouseButton))
            {
                _isSwiping = true;
                _startDragPosition = Input.mousePosition;
                ClickDown?.Invoke(_startDragPosition);
            }
        }
    }
}