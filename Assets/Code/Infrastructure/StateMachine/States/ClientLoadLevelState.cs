using Code.UI.Services.UIFactoryService;
using System;
using Unity.Netcode;

namespace Code.Infrastructure.StateMachine.States
{
    public class ClientLoadLevelState : IPayloadState<Action>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly NetworkManager _networkManager;
        private readonly IUIFactory _uiFactory;
        private Action _onStarted;

        public ClientLoadLevelState(IGameStateMachine gameStateMachine, NetworkManager networkManager, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _networkManager = networkManager;
            _uiFactory = uiFactory;
        }

        public void Enter(Action onStarted)
        {
            _onStarted = onStarted;
            _networkManager.OnClientStarted += OnClientStarted;
            Entered();
        }

        public void Exit()
        {
            _networkManager.OnClientStarted -= OnClientStarted;
        }

        private void Entered()
        {
            _networkManager.StartClient();
            _uiFactory.CreateHUD();
            _gameStateMachine.Enter<LoopState>();
        }

        private void OnClientStarted()
        {
            _onStarted?.Invoke();
        }
    }
}