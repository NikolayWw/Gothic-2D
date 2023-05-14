#region

using UnityEngine;

#endregion

namespace CodeBase.Logic.Move
{
    public interface ILookDirection
    {
        Vector2 LookDirection { get; }

        bool IsMoving();
    }
}