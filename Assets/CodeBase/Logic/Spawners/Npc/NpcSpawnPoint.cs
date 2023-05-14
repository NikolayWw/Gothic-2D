using CodeBase.Data.PlayerProgress;
using CodeBase.Data.PlayerProgress.Npc;
using CodeBase.Npc;
using CodeBase.Services.GameFactory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Npc;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Logic.Spawners.Npc
{
    public class NpcSpawnPoint : MonoBehaviour
    {
        private IGameFactory _gameFactory;
        private NpcData _npcData;
        private NpcId _id;
        private NpcHealth _health;
        private KillData _killData;
        private IUIFactory _uiFactory;
        private IWindowService _windowService;

        public void Construct(NpcId id, IPersistentProgressService persistentProgressService, IGameFactory gameFactory, IStaticDataService staticDataService, IUIFactory uiFactory, IWindowService windowService)
        {
            _id = id;
            _gameFactory = gameFactory;
            _killData = persistentProgressService.PlayerProgress.KillData;
            _uiFactory = uiFactory;
            _windowService = windowService;
            LoadProgressOrInitNew(id, persistentProgressService, staticDataService);
        }

        private async void Start()
        {
            if (_killData.Npc.Contains(_id) == false)
                await Spawn();
        }

        private async Task Spawn()
        {
            GameObject npc = await _gameFactory.CreateNpc(_id, transform, _npcData, _uiFactory, _windowService);
            _health = npc.GetComponentInChildren<NpcHealth>();
            if (_health != null)
                _health.Happened += Slay;
        }

        private void Slay()
        {
            if (_health != null)
                _health.Happened -= Slay;
            _killData.Npc.Add(_id);
        }

        private void LoadProgressOrInitNew(NpcId id, IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            if (persistentProgressService.PlayerProgress.NpcDataDictionary.Dictionary.TryGetValue(id, out _npcData) == false)
            {
                var config = staticDataService.ForNpc(id);
                _npcData = new NpcData
                {
                    NpcCharacteristics = new NpcCharacteristics
                    (
                        config.MaxHealth,
                        config.CurrentHealth,
                        config.PatrolSpeed, config.WalkingSpeed,
                        config.FollowSpeed
                    ),
                    DialogNpcKnows = new List<string>(),
                };
                persistentProgressService.PlayerProgress.NpcDataDictionary.Dictionary.Add(id, _npcData);
            }
        }
    }
}