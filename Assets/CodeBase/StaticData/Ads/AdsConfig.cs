using UnityEngine;

namespace CodeBase.StaticData.Ads
{
    [CreateAssetMenu(menuName = "Static Data/Ads config")]
    public class AdsConfig : ScriptableObject
    {
        [field: SerializeField] public float AdsDelay { get; private set; } = 5f;
        [field: SerializeField] public int RewardedGoldAmount { get; private set; } = 5;
    }
}