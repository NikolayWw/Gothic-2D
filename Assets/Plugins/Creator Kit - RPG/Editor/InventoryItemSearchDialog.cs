using Plugins.Creator_Kit___RPG.Scripts.Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Editor
{
    public class InventoryItemSearchDialog : ScriptableWizard
    {
        public StoryItem si;

        private InventoryItem[] items;
        private List<InventoryItem> results = new List<InventoryItem>();
        private GUIStyle buttonStyle;
        private System.Action<InventoryItem> onSelect = null;

        private string query = "";

        private void OnEnable()
        {
            buttonStyle = EditorStyles.miniButton;
            buttonStyle.alignment = TextAnchor.MiddleLeft;
            items = GameObject.FindObjectsOfType<InventoryItem>();
            items.OrderBy(i => i.name);
            results.AddRange(items);
        }

        public static void Show(StoryItem target, System.Action<InventoryItem> onSelect)
        {
            var w = ScriptableWizard.DisplayWizard<InventoryItemSearchDialog>("Inventory Item Search", "Close");
            w.si = target;
            w.onSelect = onSelect;
        }

        private void OnWizardCreate()
        {
        }

        private void Update()
        {
        }

        protected override bool DrawWizardGUI()
        {
            var lastQuery = query;
            query = GUILayout.TextField(query);
            if (lastQuery != query)
            {
                Search();
            }
            foreach (var i in results)
            {
                GUILayout.Space(5);
                if (GUILayout.Button(i.name, buttonStyle))
                {
                    if (onSelect != null)
                        onSelect(i);
                    Close();
                }
            }
            return false;
        }

        private void Search()
        {
            results.Clear();
            results.AddRange(from i in items orderby i.name where Match(query, i) select i);
        }

        private bool Match(string query, InventoryItem i)
        {
            var id = i.name.ToLower();
            var q = query.ToLower();
            if (id != string.Empty)
            {
                if (id.StartsWith(q)) return true;
                if (id.EndsWith(q)) return true;
                if (id.Contains(q)) return true;
            }
            return false;
        }
    }
}