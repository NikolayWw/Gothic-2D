using CodeBase.Inventory;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerOpenChest : MonoBehaviour, IInteractionWithPlayer
    {
        [SerializeField] private OpenBox _box;

        public void InteractPlayer() =>
            _box.Open();
    }
}