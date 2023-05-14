#region

using CodeBase.Inventory;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace CodeBase.Logic
{
    public class SceneComponentContainer : MonoBehaviour
    {
        [field: SerializeField] public Collider2D CameraConfinerCollider { get; private set; }
        [field: SerializeField] public List<InitInventoryBox> Boxes { get; private set; }

        public void SetBoxes(List<InitInventoryBox> boxes) =>
            Boxes = boxes;
    }
}