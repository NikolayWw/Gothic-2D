#region

using UnityEngine;

#endregion

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 MoveAxis { get; }
    }
}