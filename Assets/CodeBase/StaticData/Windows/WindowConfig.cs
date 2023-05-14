#region

using CodeBase.UI.Services.Window;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

#endregion

namespace CodeBase.StaticData.Windows
{
    [Serializable]
    public class WindowConfig
    {
        [SerializeField] private string _nameInInspector = string.Empty;
        [field: SerializeField] public WindowId WindowId { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject Template { get; private set; }

        public void OnValidate()
        {
            _nameInInspector = WindowId.ToString();
        }
    }
}