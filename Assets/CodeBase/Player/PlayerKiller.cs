using CodeBase.Data.PlayerProgress.InventoryData;
using CodeBase.Data.PlayerProgress.Player;
using CodeBase.Infrastructure.Logic;
using CodeBase.Logic;
using CodeBase.Logic.Move;
using CodeBase.Player.Move;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Items;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerKiller : MonoBehaviour, ICoroutineRunner
    {
        private const int DamageLayer = 1 << 9;

        [SerializeField] private PlayerCheckDead _checkDead;
        [SerializeField] private MoverAudio _moverAudio;
        [SerializeField] private MoverAnimation _moverAnimation;
        [SerializeField] private PlayerLookDirection _lookDirection;
        [SerializeField] private float _upPosition, _downPosition;
        [SerializeField] private Vector2 _horizontalPosition;
        [SerializeField] private Vector2 _boxSizeVertical, _boxSizeHorizontal;

        private readonly Collider2D[] _colliders = new Collider2D[5];
        private PlayerCharacteristics _characteristics;
        private Timer _timer;

        private InventorySlotsContainer _inventorySlotsContainer;

        private IStaticDataService _dataService;

        public void Construct(IPersistentProgressService persistentProgressService, IStaticDataService dataService)
        {
            _dataService = dataService;
            _inventorySlotsContainer = persistentProgressService.PlayerProgress.PlayerData.SlotsContainer;
            _characteristics = persistentProgressService.PlayerProgress.PlayerData.PlayerCharacteristics;
            _timer = new Timer(this);
        }

        public bool ConditionToHit()
        {
            return IsEquipWeapon()
                   && _timer.IsTimerElapsed
                   && _checkDead.IsDead == false;
        }

        public void Hit()
        {
            _moverAnimation.PlayAttack();
            _moverAudio.PlaySwing();
            _timer.StartTimer(_characteristics.AttackDelay);

            var hitPoint = GetHitPoint();
            int count = Physics2D.OverlapBoxNonAlloc(hitPoint.position, hitPoint.size, 0, _colliders, DamageLayer);

            for (int i = 0; i < count; i++)
                if (_colliders[i].TryGetComponent(out IGetDamage target))
                    if (target.IsDead() == false)
                    {
                        int damage = (int)(_characteristics.Strength * WeaponDamage());
                        target.GetDamage(damage);
                        _moverAudio.PlayHit();
                        break;
                    }
        }

        private (Vector2 position, Vector2 size) GetHitPoint()
        {
            var position = transform.position;

            if (_lookDirection.LookDirection == Vector2.up)
            {
                position.y += _upPosition;
                return (position, _boxSizeVertical);
            }

            if (_lookDirection.LookDirection == Vector2.down)
            {
                position.y += _downPosition;
                return (position, _boxSizeVertical);
            }

            position.x += _horizontalPosition.x * transform.localScale.x;
            position.y += _horizontalPosition.y;

            return (position, _boxSizeHorizontal);
        }

        private bool IsEquipWeapon() =>
            _inventorySlotsContainer.CurrentEquipSlot != null && _inventorySlotsContainer.CurrentEquipSlot.IsEquip;

        private float WeaponDamage() =>
            ((MeleeWeaponConfig)_dataService.ForItem(_inventorySlotsContainer.CurrentEquipSlot.ItemId)).StrengthScale;

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            var hitPoint = GetHitPoint();
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(hitPoint.position, hitPoint.size);
        }

#endif
    }
}