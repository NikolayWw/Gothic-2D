#region

using CodeBase.Data.PlayerProgress.Npc;
using CodeBase.Logic;
using CodeBase.Logic.Move;
using System;
using UnityEngine;

#endregion

namespace CodeBase.Npc
{
    public class NpcHealth : MonoBehaviour, IGetDamage
    {
        [SerializeField] private MoverAnimation _animation;
        public Action Happened;
        private NpcCharacteristics _characteristics;

        public void Construct(NpcCharacteristics characteristics)
        {
            _characteristics = characteristics;
        }

        public void GetDamage(int damage)
        {
            if (_characteristics.CurrentHealth <= 0)
                return;

            _characteristics.Decrement(damage);
            if (_characteristics.CurrentHealth <= 0)
            {
                Happened?.Invoke();
                _animation.PlayDead();
            }
        }

        public bool IsDead() =>
            _characteristics.CurrentHealth <= 0;
    }
}