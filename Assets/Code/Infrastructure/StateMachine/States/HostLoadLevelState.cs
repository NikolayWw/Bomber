using Code.Logic;
using Code.Services.PersistentProgressService;
using Code.Services.PlayerSpawnPoint;
using Code.UI.Services.UIFactoryService;
using System;
using System.Linq;
using Code.Services.StartGameOver;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Infrastructure.StateMachine.States
{
    public class HostLoadLevelState : IPayloadState<Action, int>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPlayerSpawnPointService _playerSpawnPoint;
        private readonly IPersistentProgress _persistentProgress;
        private readonly IUIFactory _uiFactory;
        private readonly NetworkManager _networkManager;
        private readonly IGameStartOverService _gameStartOver;
        private Action _onStarted;

        public HostLoadLevelState(IGameStateMachine gameStateMachine, NetworkManager networkManager, IGameStartOverService gameStartOver,
            IPlayerSpawnPointService playerSpawnPoint, IPersistentProgress persistentProgress, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _playerSpawnPoint = playerSpawnPoint;
            _persistentProgress = persistentProgress;
            _uiFactory = uiFactory;
            _networkManager = networkManager;
            _gameStartOver = gameStartOver;
        }

        public void Enter(Action onStarted, int maxPlayers)
        {
            _onStarted = onStarted;
            _networkManager.OnServerStarted += OnServerStarted;
            Entered(maxPlayers);
        }

        public void Exit()
        {
            _networkManager.OnServerStarted -= OnServerStarted;
        }

        private void Entered(int maxPlayers)
        {
            MapComponentContainer mapComponentContainer = Object.FindObjectOfType<MapComponentContainer>();
            Vector2[] positions = mapComponentContainer.PlayerSpawnMarker.Points.Select(point => new Vector2(point.position.x, point.position.y)).ToArray();

            _networkManager.StartHost();
            _playerSpawnPoint.SetPositions(positions);
            _persistentProgress.BlocksProgress.InitBlocks(mapComponentContainer.DestructibleBlockPrefabContainer);
            _uiFactory.CreateHUD();
            _gameStartOver.InitCheckStartGame(maxPlayers);

            _gameStateMachine.Enter<LoopState>();
        }

        private void OnServerStarted()
        {
            _onStarted?.Invoke();
        }
    }
}