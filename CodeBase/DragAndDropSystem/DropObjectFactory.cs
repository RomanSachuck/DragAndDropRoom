using CodeBase.Infrastructure.Services.InputService;
using UnityEngine;

namespace CodeBase.DragAndDropSystem
{
    public class DropObjectFactory
    {
        private readonly DropController _dropController;
        private readonly IInput _inputService;
        private readonly GameObject _objectPrefab;

        public DropObjectFactory(DropController dropController, IInput inputService, GameObject objectPrefab)
        {
            _dropController = dropController;
            _inputService = inputService;
            _objectPrefab = objectPrefab;
        }

        public GameObject CreateDropObject()
        {
            GameObject obj = Object.Instantiate(_objectPrefab, Vector3.zero, Quaternion.identity);
            obj.GetComponent<DropObject>().Initialize(_dropController);
            obj.GetComponent<FollowToCursor>().Initialize(_inputService);
            return obj;
        }
    }
}