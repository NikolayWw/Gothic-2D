using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Cutscene.Editor
{
    [CustomEditor(typeof(Runtime.Cutscene))]
    public class CutsceneEditor : UnityEditor.Editor
    {
        private ReorderableList list;
        private Runtime.Cutscene cutscene;

        private void OnEnable()
        {
            cutscene = target as Runtime.Cutscene;
            list = new ReorderableList(serializedObject, serializedObject.FindProperty("cutsceneEvents"), true, true, true, true);
            list.drawElementCallback = OnDrawElement;
            list.onAddCallback += OnAdd;
            list.onRemoveCallback += OnRemove;
            list.drawHeaderCallback += OnDrawHeader;
            list.onSelectCallback += OnSelect;
            list.elementHeight = 128;
            Undo.undoRedoPerformed -= OnUndoRedo;
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnSelect(ReorderableList list)
        {
        }

        private void OnDrawHeader(Rect rect)
        {
            GUI.Label(rect, "Cutscene Events");
        }

        private void OnUndoRedo()
        {
            if (serializedObject != null)
                serializedObject.Update();
        }

        private void OnRemove(ReorderableList list)
        {
            list.serializedProperty.DeleteArrayElementAtIndex(list.index);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void OnAdd(ReorderableList list)
        {
            list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
            var n = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
            n.FindPropertyRelative("newImage").objectReferenceValue = null;
            n.FindPropertyRelative("newText").stringValue = "";
            serializedObject.ApplyModifiedProperties();
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var p = list.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, p);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("zoom"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeInTime"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeOutTime"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crossFadeTime"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pan"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClip"), new GUIContent("Background Audio"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyWhenFinished"), true);
            serializedObject.ApplyModifiedProperties();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}