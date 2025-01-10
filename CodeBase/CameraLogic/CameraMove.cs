using DG.Tweening;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private SpriteRenderer _spriteLimiting;
        [SerializeField] private float _movementSpeed;
        
        private int _screenWidth;
        private int _screenHeight;
        private float _leftExtremePosition;
        private float _rightExtremePosition;
        
        public void Move(Vector3 direction)
        {
            CheckScreenChanges();
            DOTween.Kill(transform);
            transform.DOMoveX(CalculateEndPosition(direction), Constants.MovementCameraDuration).SetId(transform);
        }

        private void CheckScreenChanges()
        {
            if (_screenWidth != Screen.width || _screenHeight != Screen.height)
            {
                CalculateExtremePositions();
                _screenWidth = Screen.width;
                _screenHeight = Screen.height;
            }
        }
        
        private float CalculateEndPosition(Vector3 direction)
        {
            float endPosition = -direction.normalized.x * _movementSpeed + transform.position.x;
            
            endPosition = endPosition < _leftExtremePosition ? _leftExtremePosition : endPosition;
            endPosition = endPosition > _rightExtremePosition ? _rightExtremePosition : endPosition;
            return endPosition;
        }

        private void CalculateExtremePositions()
        {
            float cameraWidth = _camera.aspect * _camera.orthographicSize * 2;
            float spriteWidth = _spriteLimiting.bounds.size.x;
            
            if(cameraWidth > spriteWidth)
                cameraWidth = spriteWidth;
            
            _rightExtremePosition = spriteWidth / 2 - cameraWidth / 2;
            _leftExtremePosition = -_rightExtremePosition;
        }
    }
}