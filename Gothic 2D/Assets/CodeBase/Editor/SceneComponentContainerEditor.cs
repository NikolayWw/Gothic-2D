using CodeBase.Inventory;
using CodeBase.Logic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(SceneComponentContainer))]
    public class SceneComponentContainerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SceneComponentContainer container = (SceneComponentContainer)target;
            if (GUILayout.Button("Collect"))
            {
                container.SetBoxes(FindObjectsOfType<InitInventoryBox>().ToList());
                EditorUtility.SetDirty(target);
            }
        }
    }
}