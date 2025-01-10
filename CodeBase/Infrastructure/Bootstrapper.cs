using System;
using System.Collections.Generic;
using CodeBase.CameraLogic;
using CodeBase.DragAndDropSystem;
using CodeBase.Infrastructure.Services.InputService;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private CameraMove _cameraMove;
        [SerializeField] private GameObject _dropObjectPrefab;
        
        private IInput _inputService;
        private ScreenScrollInputHandler _screenScrollInputHandler;
        private DropObjectFactory _dropObjectFactory;
        
        
        private List<ITickable> _ticks;
        private List<IDisposable> _disposables;

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }

        private void Awake()
        {
            _ticks = new List<ITickable>();
            _disposables = new List<IDisposable>();

            BindInputService();
            BindScreenScrollInputHandler();
            BindDropObjectFactory();
        }

        private void Start()
        {
            Game game = new Game(_dropObjectFactory, _screenScrollInputHandler);
            game.Run();
            
            _disposables.Add(game);
        }

        private void Update()
        {
            foreach (var tickable in _ticks)
                tickable.Tick();
        }
        
        private void BindInputService()
        {
            if(SystemInfo.deviceType == DeviceType.Handheld)
                _inputService = new MobileInput();
            else
                _inputService = new DesktopInput();
            
            _ticks.Add(_inputService);
        }
        
        private void BindScreenScrollInputHandler()
        {
            _screenScrollInputHandler = new ScreenScrollInputHandler(_inputService, _cameraMove);
            _disposables.Add(_screenScrollInputHandler);
        }
        
        private void BindDropObjectFactory() => 
            _dropObjectFactory = new DropObjectFactory(new DropController(), _inputService, _dropObjectPrefab);
    }
}
