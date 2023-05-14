using Plugins.Creator_Kit___RPG.Scripts.Core;
using Plugins.Creator_Kit___RPG.Scripts.UI;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.Gameplay
{
    /// <summary>
    /// Marks a gameObject as a collectable item.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
    public class InventoryItem : MonoBehaviour
    {
        public int count = 1;
        public Sprite sprite;

        private GameModel model = Schedule.GetModel<GameModel>();

        private void Reset()
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
        }

        private void OnEnable()
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            MessageBar.Show($"You collected: {name} x {count}");
            model.AddInventoryItem(this);
            UserInterfaceAudio.OnCollect();
            gameObject.SetActive(false);
        }
    }
}