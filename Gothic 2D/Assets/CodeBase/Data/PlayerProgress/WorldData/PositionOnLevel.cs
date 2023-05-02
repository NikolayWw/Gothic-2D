using System;
using UnityEngine;

namespace CodeBase.Data.PlayerProgress.WorldData
{
    [Serializable]
    public class PositionOnLevel
    {
        public string Level = string.Empty;
        public Vector2 Position;

        public PositionOnLevel(in string level, Vector2 position)
        {
            Level = level;
            Position = position;
        }
    }
}