using Plugins.Creator_Kit___RPG.Scripts.Core;
using Plugins.Creator_Kit___RPG.Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.UI
{
    public class InventoryController : MonoBehaviour
    {
        public Transform elementPrototype;
        public float stepSize = 1;

        private Vector2 firstItem;
        private GameModel model = Schedule.GetModel<GameModel>();
        private SpriteUIElement sizer;

        private void Start()
        {
            firstItem = elementPrototype.localPosition;
            elementPrototype.gameObject.SetActive(false);
            sizer = GetComponent<SpriteUIElement>();
            Refresh();
        }

        // Update is called once per frame
        public void Refresh()
        {
            var cursor = firstItem;
            for (var i = 1; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
            var displayCount = 0;
            foreach (var i in model.InventoryItems)
            {
                var count = model.GetInventoryCount(i);
                if (count <= 0) continue;
                displayCount++;
                var e = Instantiate(elementPrototype);
                e.transform.parent = transform;
                e.transform.localPosition = cursor;
                e.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = model.GetInventorySprite(i);
                e.transform.GetChild(1).GetComponent<TextMeshPro>().text = $"x {count}";
                e.gameObject.SetActive(true);
                cursor.y -= stepSize;
            }

            if (displayCount > 0)
                sizer.Show();
            else
                sizer.Hide();
        }
    }
}