using Plugins.Creator_Kit___RPG.Scripts.Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Editor
{
    public class StoryItemSearchDialog : ScriptableWizard
    {
        public StoryItem si;

        private StoryItem[] items;
        private List<StoryItem> results = new List<StoryItem>();
        private GUIStyle buttonStyle;
        private System.Action<StoryItem> onSelect = null;

        private string query = "";

        private void OnEnable()
        {
            buttonStyle = EditorStyles.miniButton;
            buttonStyle.alignment = TextAnchor.MiddleLeft;
            items = GameObject.FindObjectsOfType<StoryItem>();
            items.OrderBy(i => i.text);
            results.AddRange(items);
        }

        public static void Show(StoryItem target, System.Action<StoryItem> onSelect)
        {
            var w = ScriptableWizard.DisplayWizard<StoryItemSearchDialog>("Story Item Search", "Close");
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
                if (GUILayout.Button(i.text, buttonStyle))
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
            results.AddRange(from i in items orderby i.text where Match(query, i) select i);
        }

        private bool Match(string query, StoryItem i)
        {
            var id = i.ID.ToLower();
            var text = i.text.ToLower();
            var q = query.ToLower();
            if (id != string.Empty)
            {
                if (id.StartsWith(q)) return true;
                if (id.EndsWith(q)) return true;
                if (id.Contains(q)) return true;
            }
            if (text != string.Empty)
            {
                if (text.StartsWith(q)) return true;
                if (text.EndsWith(q)) return true;
                if (text.Contains(q)) return true;
            }
            return false;
        }
    }
}