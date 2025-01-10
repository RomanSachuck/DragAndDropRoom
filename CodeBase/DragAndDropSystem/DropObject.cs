using System;
using UnityEngine;

namespace CodeBase.DragAndDropSystem
{
    [RequireComponent(typeof(EventTriggerExample), 
        typeof(FollowToCursor), 
        typeof(CircleCollider2D))]
    public class DropObject : MonoBehaviour
    {
        public event Action PointerUpEvent;
        public event Action PointerDownEvent;
        
        [SerializeField] private FollowToCursor _followToCursor;
        [SerializeField] private EventTriggerExample _eventTrigger;
        [SerializeField] private CircleCollider2D _circleCollider2D;
        
        private DropController _dropController;

        private void OnDestroy()
        {
            _eventTrigger.PointerDown -= PointerDown;
            _eventTrigger.PointerUp -= PointerUp;
        }

        public void Initialize(DropController dropController)
        {
            _dropController = dropController;
            
            _eventTrigger.PointerDown += PointerDown;
            _eventTrigger.PointerUp += PointerUp;

            PointerUp();
        }

        private void PointerDown()
        {
            _followToCursor.enabled = true;
            PointerDownEvent?.Invoke();
        }
        
        private void PointerUp()
        {
            _followToCursor.enabled = false;
            PointerUpEvent?.Invoke();
            
            _dropController.Drop(_circleCollider2D);
        }
    }
}