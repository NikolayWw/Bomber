using Code.StaticData.Bomb;
using Code.StaticData.CountdownTimer;
using Code.StaticData.Improvements;
using Code.StaticData.Improvements.Spawner;
using Code.StaticData.Player;
using Code.StaticData.Windows;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string PlayerDataPath = "Player/PlayerStaticData";
        private const string WindowDataPath = "Window/WindowStaticData";
        private const string BombDataPath = "Bomb/BombStaticData";
        private const string ImprovementsSpawnDataPath = "Improvements/ImprovementsSpawner/ImprovementsSpawnStaticData";
        private const string ImprovementsDataPath = "Improvements/ImprovementsStaticData";
        private const string CountdownTimerDataPath = "CountdownTimer/CountdownTimerStaticData";

        public PlayerStaticData PlayerData { get; private set; }
        public WindowStaticData WindowData { get; private set; }
        public ImprovementsSpawnStaticData ImprovementsSpawnData { get; private set; }
        public ImprovementsStaticData ImprovementsData { get; private set; }
        public CountdownTimerStaticData CountdownTimerData { get; private set; }
        private Dictionary<BombId, BombConfig> _bombConfigs;
        private Dictionary<ImprovementsId, BaseImprovementConfig> _improvementConfigs;

        public StaticDataService()
        {
            Load();
        }

        private void Load()
        {
            PlayerData = Resources.Load<PlayerStaticData>(PlayerDataPath);
            WindowData = Resources.Load<WindowStaticData>(WindowDataPath);
            CountdownTimerData = Resources.Load<CountdownTimerStaticData>(CountdownTimerDataPath);
            ImprovementsSpawnData = Resources.Load<ImprovementsSpawnStaticData>(ImprovementsSpawnDataPath);
            _bombConfigs = Resources.Load<BombStaticData>(BombDataPath).Configs.ToDictionary(x => x.Id, x => x);
            LoadImprovement();
        }

        private void LoadImprovement()
        {
            ImprovementsStaticData improvementsStaticData = Resources.Load<ImprovementsStaticData>(ImprovementsDataPath);
            _improvementConfigs = improvementsStaticData.Configs
                .ToDictionary(x => x.Id, x => x);
            ImprovementsData = improvementsStaticData;
        }

        public BaseImprovementConfig ForImprovement(ImprovementsId id) =>
            _improvementConfigs.TryGetValue(id, out BaseImprovementConfig cfg) ? cfg : null;

        public BombConfig ForBomb(BombId id) =>
            _bombConfigs.TryGetValue(id, out BombConfig cfg) ? cfg : null;
    }
}