using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Items
{
    [CreateAssetMenu(fileName = "New ItemStaticData", menuName = "Static Data/Inventory Slots Container/Item Static Data", order = 0)]
    public class ItemContainer : ScriptableObject
    {
        [field: SerializeField] public None None { get; private set; }
        [field: SerializeField] public GoldConfig Gold { get; private set; }
        [field: SerializeField] public List<OtherItemsConfig> OtherItemConfigs { get; private set; }
        [field: SerializeField] public List<MeleeWeaponConfig> MeleeWeapons { get; private set; }
        [field: SerializeField] public List<FoodConfig> Foods { get; private set; }

        private void OnValidate()
        {
            None.OnValidate();
            Gold.OnValidate();
            OtherItemConfigs.ForEach(x => x.OnValidate());
            MeleeWeapons.ForEach(x => x.OnValidate());
            Foods.ForEach(x => x.OnValidate());
        }
    }
}