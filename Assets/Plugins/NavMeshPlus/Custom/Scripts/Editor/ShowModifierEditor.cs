#region

using Plugins.NavMeshPlus.NavMeshComponents.Scripts;
using System.Linq;
using UnityEditor;
using UnityEngine;

#endregion

namespace Plugins.NavMeshPlus.Custom.Scripts.Editor
{
    public class ShowModifierEditor : UnityEditor.Editor
    {
        private const string Path = "Tools/NavMeshPlus/";
        private const string ModifierName = "Modifier";
        private const string SpritePath = "Modifier";

        [MenuItem(Path + "Show Modifier Markers")]
        private static void Show()
        {
            foreach (var marker in Resources.FindObjectsOfTypeAll<ModifierMarker>())
            {
                marker.gameObject.SetActive(true);
                EditorUtility.SetDirty(marker);
            }
        }

        [MenuItem(Path + "Hide Modifier Markers")]
        private static void Hide()
        {
            foreach (var marker in FindObjectsOfType<ModifierMarker>())
            {
                EditorUtility.SetDirty(marker);
                marker.gameObject.SetActive(false);
            }
        }

        [MenuItem(Path + "Add Modifier")]
        private static void AddModifier()
        {
            if (Selection.activeTransform == null)
                return;

            EditorUtility.SetDirty(Selection.activeTransform);

            if (Selection.activeTransform.TryGetComponent(out ModifierMarker _))
                return;

            if (Selection.activeTransform.GetComponentsInChildren<ModifierMarker>()
                .Any(child => child.TryGetComponent(out ModifierMarker _)))
                return;

            var markerObj = new GameObject(ModifierName);
            markerObj.transform.parent = Selection.activeTransform;
            markerObj.transform.localPosition = Vector3.zero;

            var navMeshModifier = markerObj.AddComponent<NavMeshModifier>();
            navMeshModifier.overrideArea = true;
            navMeshModifier.area = 1; //not walkable

            markerObj.AddComponent<ModifierMarker>();

            var spriteRenderer = markerObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>(SpritePath);
            spriteRenderer.sortingOrder = 999;
        }

        [MenuItem(Path + "Remove Modifier")]
        private static void RemoveModifier()
        {
            if (Selection.activeTransform == null)
                return;
            EditorUtility.SetDirty(Selection.activeTransform);

            if (Selection.activeTransform.TryGetComponent(out ModifierMarker marker))
                DestroyImmediate(marker.gameObject);

            if (Selection.activeTransform == null)
                return;

            var componentsInChildren = Selection.activeTransform.GetComponentsInChildren<ModifierMarker>();
            foreach (var childMarker in componentsInChildren)
                DestroyImmediate(childMarker.gameObject);
        }
    }
}