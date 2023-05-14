#region

using System;
using UnityEngine;

#endregion

namespace CodeBase.Logic
{
    public class UniqueId : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }

        public void SetId(string id)
        {
            Id = id;
        }

        public void GenerateId()
        {
            Id = $"{gameObject.scene.name}_{Guid.NewGuid().ToString()}";
        }
    }
}