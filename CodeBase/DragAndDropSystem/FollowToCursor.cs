using CodeBase.Infrastructure.Services.InputService;
using UnityEngine;

namespace CodeBase.DragAndDropSystem
{
    public class FollowToCursor : MonoBehaviour
    {
        private Camera _camera;
        private IInput _inputService;

        private void Awake() => 
            _camera = Camera.main;

        public void Initialize(IInput inputService) => 
            _inputService = inputService;

        private void Update()
        {
            Vector3 cursorPosition = _camera.ScreenToWorldPoint(_inputService.CursorPosition);
            cursorPosition.z = 0;
            transform.position = cursorPosition;
        }
    }
}