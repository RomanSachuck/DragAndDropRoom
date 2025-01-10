using System;
using CodeBase.CameraLogic;
using CodeBase.DragAndDropSystem;

namespace CodeBase.Infrastructure
{
    public class Game : IDisposable
    {
        private readonly DropObjectFactory _dropObjectFactory;
        private readonly ScreenScrollInputHandler _screenScrollInputHandler;
        
        private DropObject _dropObject;

        public Game(DropObjectFactory dropObjectFactory, ScreenScrollInputHandler screenScrollInputHandler)
        {
            _dropObjectFactory = dropObjectFactory;
            _screenScrollInputHandler = screenScrollInputHandler;
        }

        public void Run()
        {
            _dropObject = _dropObjectFactory.CreateDropObject().GetComponent<DropObject>();
            _screenScrollInputHandler.Enable();
            
            _dropObject.PointerDownEvent += DropObjectPointerDown;
            _dropObject.PointerUpEvent += DropObjectPointerUp;
        }

        public void Dispose()
        {
            _dropObject.PointerDownEvent -= DropObjectPointerDown;
            _dropObject.PointerUpEvent -= DropObjectPointerUp;
        }
        
        private void DropObjectPointerUp() => 
            _screenScrollInputHandler.Enable();

        private void DropObjectPointerDown() => 
            _screenScrollInputHandler.Disable();
    }
}