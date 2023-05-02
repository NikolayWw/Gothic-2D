using CodeBase.Data.PlayerProgress.Player;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.Characteristics
{
    public class CharacteristicsWindow : BaseWindow
    {
        [SerializeField] private TMP_Text _levelValueText;
        [SerializeField] private TMP_Text _experienceValueText;
        [SerializeField] private TMP_Text _experienceToNextLevelValueText;
        [SerializeField] private TMP_Text _lpValueText;
        [SerializeField] private TMP_Text _strengthValueText;
        [SerializeField] private TMP_Text _maxHealthValueText;
        [SerializeField] private TMP_Text _currentHealthValueText;

        private PlayerCharacteristics _data;

        public void Construct(IPersistentProgressService persistentProgressService)
        {
            _data = persistentProgressService.PlayerProgress.PlayerData.PlayerCharacteristics;
            _data.OnChangeCharacteristics += Refresh;
        }

        private void Start() =>
            Refresh();

        private void OnDestroy() =>
            Clean();

        private void Refresh()
        {
            _levelValueText.text = _data.Level.ToString();
            _experienceValueText.text = _data.Experience.ToString();
            _experienceToNextLevelValueText.text = _data.NextLevel.ToString();
            _lpValueText.text = _data.LP.ToString();
            _strengthValueText.text = _data.Strength.ToString();
            _maxHealthValueText.text = _data.MaxHealth.ToString();
            _currentHealthValueText.text = _data.CurrentHealth.ToString();
        }

        private void Clean()
        {
            if (_data != null)
                _data.OnChangeCharacteristics += Refresh;
        }
    }
}