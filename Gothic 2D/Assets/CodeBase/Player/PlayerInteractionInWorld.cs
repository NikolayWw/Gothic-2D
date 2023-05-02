#region

using CodeBase.UI.Elements;
using UnityEngine;

#endregion

namespace CodeBase.Player
{
    public class PlayerInteractionInWorld : MonoBehaviour
    {
        private readonly Collider2D[] _interactionColliders = new Collider2D[15];
        private PlayerInteractionButton _interactionButton;

        public void Construct(PlayerInteractionButton interactionButton)
        {
            _interactionButton = interactionButton;
            _interactionButton.InteractionButton.onClick.AddListener(Interact);
        }

        private void OnDestroy() =>
            _interactionButton.InteractionButton.onClick.RemoveListener(Interact);

        private void Interact()
        {
            var actCount = Physics2D.OverlapBoxNonAlloc(transform.position, Vector2.one, 0, _interactionColliders);
            for (var i = 0; i < actCount; i++)
                if (_interactionColliders[i].TryGetComponent(out IInteractionWithPlayer actionWithPlayer))
                {
                    actionWithPlayer.InteractPlayer();
                    break;
                }
        }
    }
}