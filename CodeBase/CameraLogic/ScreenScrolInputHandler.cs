using System;
using CodeBase.Infrastructure.Services.InputService;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class ScreenScrollInputHandler : IDisposable
    {
        private readonly IInput _input;
        private readonly CameraMove _cameraMove;

        private bool _isActive;

        public ScreenScrollInputHandler(IInput input, CameraMove cameraMove)
        {
            _input = input;
            _cameraMove = cameraMove;

            _input.Drag += OnDrag;
        }

        public void Enable() => 
            _isActive = true;
        
        public void Disable() => 
            _isActive = false;
        
        private void OnDrag(Vector3 direction)
        {
            if(_isActive)
                _cameraMove.Move(direction);
        }

        public void Dispose() => 
            _input.Drag -= OnDrag;
    }
}