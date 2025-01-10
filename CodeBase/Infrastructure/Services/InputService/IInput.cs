using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.InputService
{
    public interface IInput : ITickable
    {
        event Action<Vector3> ClickUp;
        event Action<Vector3> ClickDown;
        event Action<Vector3> Drag;
        Vector3 CursorPosition {get;}
    }
}