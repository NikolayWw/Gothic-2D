#region

using CodeBase.Logic.Spawners.Npc;
using UnityEditor;
using UnityEngine;

#endregion

namespace CodeBase.Editor
{
    [CustomEditor(typeof(NpcSpawnMarker))]
    public class NpcSpawnMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(NpcSpawnMarker spawner, GizmoType gizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }
    }
}