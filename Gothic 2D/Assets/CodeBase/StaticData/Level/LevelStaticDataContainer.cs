using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Level
{
    [CreateAssetMenu(fileName = "New LevelDataContainer", menuName = "Static Data/Level/LevelDataContainer", order = 1)]
    public class LevelStaticDataContainer : ScriptableObject
    {
        [field: SerializeField] public List<LevelStaticData> LevelStaticDatas { get; private set; }
    }
}