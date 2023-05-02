using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.GamePlayMessage
{
    public class GameplayMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void Construct(string context, float lifeTime)
        {
            _text.text = context;
            Destroy(gameObject, lifeTime);
        }
    }
}