using System;
using UnityEngine;

namespace CodeBase.StaticData.Items
{
    [Serializable]
    public class FoodConfig : BaseItem
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int CurrentHealth { get; private set; }
        [field: SerializeField] public int Strength { get; private set; }
    }
}