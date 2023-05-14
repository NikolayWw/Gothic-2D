#region

using CodeBase.AITree.Tasks;
using CodeBase.AITree.Tree;
using CodeBase.Data.PlayerProgress.Npc;
using CodeBase.Infrastructure.Logic;
using CodeBase.Logic.Move;
using CodeBase.Services.LogicFactory;
using CodeBase.Services.PersistentProgress;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace CodeBase.Npc
{
    public class NpcBehaviour : TreeAI, ICoroutineRunner
    {
        [SerializeField] private NpcMover _npcMover;
        [SerializeField] private MoverAnimation _moverAnimation;
        [SerializeField] private NpcHealth _npcHealth;

        private IPersistentProgressService _persistentProgressService;
        private ILogicFactoryService _logicFactory;
        private NpcCharacteristics _characteristics;
        public Transform Target { get; private set; }

        public void Construct(IPersistentProgressService persistentProgressService, ILogicFactoryService logicFactory, NpcCharacteristics npcCharacteristics)
        {
            _persistentProgressService = persistentProgressService;
            _logicFactory = logicFactory;
            _characteristics = npcCharacteristics;
            _npcHealth.Happened += Pause;
        }

        public void SetTarget(Transform target)
        {
            Target = target;
        }

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequencer(new List<Node>
                {
                    new CheckAttackDistanceTask(_npcMover, transform),
                    new AttackTask(this, this, _moverAnimation, _persistentProgressService)
                }),
                new Sequencer(new List<Node>
                {
                    new ApplyTargetTask(this),
                    new FollowTask(_npcMover,_characteristics.FollowSpeed)
                }),
            });
            return root;
        }

        private void Pause()
        {
            enabled = false;
        }
    }
}