using Code.Services.ImprovementsSpawnService;
using Code.Services.PlayerReporter;
using Code.Services.StaticData;
using Code.UI.Services.UIFactoryService;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.CountdownTimer
{
    public class CountdownTimerService : NetworkBehaviour, ICountdownTimerService
    {
        private readonly WaitForSeconds _wait = new(1);
        private IPlayerReporterService _playerReporterService;
        private IUIFactory _uiFactory;
        private IEnumerator _timerEnumerator;
        private IStaticDataService _dataService;
        private IImprovementsSpawner _improvementsSpawner;
        public NetworkVariable<int> CurrentSeconds { get; } = new();

        public void Construct(IUIFactory uiFactory, IImprovementsSpawner improvementsSpawner, IStaticDataService dataService, IPlayerReporterService playerReporterService)
        {
            _uiFactory = uiFactory;
            _improvementsSpawner = improvementsSpawner;
            _dataService = dataService;
            _playerReporterService = playerReporterService;
        }

        [ServerRpc]
        public void StartTimerServerRpc()
        {
            CurrentSeconds.Value = _dataService.CountdownTimerData.Seconds;
            StartCoroutine(_timerEnumerator = Timer());
        }

        [ServerRpc]
        public void StopTimerServerRpc()
        {
            if (_timerEnumerator != null)
                StopCoroutine(_timerEnumerator);
        }

        [ClientRpc]
        private void GameDrawClientRpc()
        {
            _uiFactory.CreateDrawWindow();
        }

        private IEnumerator Timer()
        {
            while (CurrentSeconds.Value > 0)
            {
                yield return _wait;
                CurrentSeconds.Value--;
            }
            _improvementsSpawner.StopSpawn();
            _playerReporterService.SetPlayersGameOverServerRpc();
            GameDrawClientRpc();
        }
    }
}