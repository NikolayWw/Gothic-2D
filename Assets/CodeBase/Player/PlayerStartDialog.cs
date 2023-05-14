using CodeBase.Dialogs;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerStartDialog : MonoBehaviour, IInteractionWithPlayer
    {
        [SerializeField] private BaseDialog _baseDialog;

        public void InteractPlayer() =>
            _baseDialog.StartDialog();
    }
}