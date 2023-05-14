using Plugins.Creator_Kit___RPG.Scripts.Tiles.Rule_Tile.Scripts;
using System;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.Tiles.Isometric_Rule_Tile
{
    public class IsometricRuleTile<T> : IsometricRuleTile
    {
        public override sealed Type m_NeighborType
        { get { return typeof(T); } }
    }

    [Serializable]
    [CreateAssetMenu(fileName = "New Isometric Rule Tile", menuName = "Tiles/Isometric Rule Tile")]
    public class IsometricRuleTile : RuleTile
    {
        // This has no differences with the RuleTile
    }
}