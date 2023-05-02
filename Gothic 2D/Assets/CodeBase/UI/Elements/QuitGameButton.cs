using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class QuitGameButton : MonoBehaviour
    {
        [SerializeField] private Button _quitButton;

        private void Start() => 
            _quitButton.onClick.AddListener(Application.Quit);
    }
}