using Plugins.NavMeshPlus.NavMeshComponents.Scripts;
using UnityEditor;
using UnityEngine;

namespace Plugins.NavMeshPlus.NavMeshComponents.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CollectSources2d))]
    internal class CollectSources2dEditor : UnityEditor.Editor
    {
        private SerializedProperty m_OverrideByGrid;
        private SerializedProperty m_UseMeshPrefab;
        private SerializedProperty m_CompressBounds;
        private SerializedProperty m_OverrideVector;

        private void OnEnable()
        {
            m_OverrideByGrid = serializedObject.FindProperty("m_OverrideByGrid");
            m_UseMeshPrefab = serializedObject.FindProperty("m_UseMeshPrefab");
            m_CompressBounds = serializedObject.FindProperty("m_CompressBounds");
            m_OverrideVector = serializedObject.FindProperty("m_OverrideVector");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var surf = target as CollectSources2d;

            EditorGUILayout.PropertyField(m_OverrideByGrid);
            using (new EditorGUI.DisabledScope(!m_OverrideByGrid.boolValue))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_UseMeshPrefab);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.PropertyField(m_CompressBounds);
            EditorGUILayout.PropertyField(m_OverrideVector);

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();

            using (new EditorGUI.DisabledScope(Application.isPlaying))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth);
                if (GUILayout.Button("Rotate Surface to XY"))
                {
                    foreach (CollectSources2d item in targets)
                    {
                        item.transform.rotation = Quaternion.Euler(-89.98f, 0f, 0f);
                    }
                }
                GUILayout.EndHorizontal();
                foreach (CollectSources2d navSurface in targets)
                {
                    if (!Mathf.Approximately(navSurface.transform.eulerAngles.x, 270.0198f) && !Mathf.Approximately(navSurface.transform.eulerAngles.x, 270f))
                    {
                        EditorGUILayout.HelpBox("NavMeshSurface is not rotated respectively to (x-90;y0;z0). Apply rotation unless intended.", MessageType.Warning);
                    }
                }
            }
        }
    }
}