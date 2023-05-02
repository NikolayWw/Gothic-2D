using System;
using CodeBase.Data.PlayerProgress.WorldData.ItemPiece;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.WorldData
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public ItemPieceDataDictionary ItemPieceDataDictionary;
        public WorldData(in string sceneKey, Vector2 playerInitialPosition)
        {
            PositionOnLevel = new PositionOnLevel(sceneKey, playerInitialPosition);
            ItemPieceDataDictionary = new ItemPieceDataDictionary();
        }
    }
}