#region

using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace CodeBase.UI.Elements
{
    public class PlayerInteractionButton : BaseWindow
    {
        [field: SerializeField] public Button InteractionButton { get; private set; }
    }
}