#region

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace CodeBase.UI.Windows.Dialog
{
    public class DialogButton : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }

        public void Close() =>
            Destroy(gameObject);
    }
}