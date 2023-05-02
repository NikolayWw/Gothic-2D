using CodeBase.Data.PlayerProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic
{
    public class UpdateUIHealth : MonoBehaviour
    {
        [SerializeField] private Image _imageFilled;

        private IDataHealth _dataHealth;
        private Transform _followTarget;

        public void Construct(IDataHealth dataHealth, Transform followTarget)
        {
            _dataHealth = dataHealth;
            _followTarget = followTarget;
            _dataHealth.OnChangeCurrentHealth += Refresh;
        }

        private void Start() =>
            Refresh();

        private void OnDestroy() =>
            _dataHealth.OnChangeCurrentHealth -= Refresh;

        private void FixedUpdate() =>
            transform.position = _followTarget.position;

        private void Refresh() =>
            _imageFilled.fillAmount = _dataHealth.CurrentHealth / (float)_dataHealth.MaxHealth;
    }
}