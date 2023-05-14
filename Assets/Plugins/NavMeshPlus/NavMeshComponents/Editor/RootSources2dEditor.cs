﻿using Plugins.NavMeshPlus.NavMeshComponents.Scripts;
using UnityEditor;

namespace Plugins.NavMeshPlus.NavMeshComponents.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RootSources2d))]
    internal class RootSources2dEditor : UnityEditor.Editor
    {
        private SerializedProperty _rootSources;

        private void OnEnable()
        {
            _rootSources = serializedObject.FindProperty("_rootSources");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var surf = target as RootSources2d;
            EditorGUILayout.HelpBox("Add GameObjects to create NavMesh form it and it's ancestors", MessageType.Info);

            if (surf.NavMeshSurfaceOwner.collectObjects != CollectObjects.Children)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Root Sources are only suitable for 'CollectObjects - Children'", MessageType.Info);
                EditorGUILayout.Space();
            }
            EditorGUILayout.PropertyField(_rootSources);

            serializedObject.ApplyModifiedProperties();
        }
    }
}