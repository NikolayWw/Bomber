using Code.Infrastructure.StateMachine.States;
using Code.Services;
using Code.Services.CountdownTimer;
using Code.Services.ImprovementsSpawnService;
using Code.Services.PersistentProgressService;
using Code.Services.PlayerSpawnPoint;
using Code.UI.Services.UIFactoryService;
using System;
using System.Collections.Generic;
using Code.Services.StartGameOver;
using Unity.Netcode;

namespace Code.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitable> _states;
        private IExitable _activeState = new LoopState();

        public GameStateMachine(AllServices allServices, NetworkManager networkManager)
        {
            _states = new Dictionary<Type, IExitable>
            {
                [typeof(LoopState)] = new LoopState(),
                [typeof(LoadMainMenuState)] = new LoadMainMenuState(this, allServices.Single<IUIFactory>()),
                [typeof(GameDrawState)] = new GameDrawState(this, allServices.Single<IUIFactory>()),
                [typeof(ClientLoadLevelState)] = new ClientLoadLevelState(this, networkManager, allServices.Single<IUIFactory>()),
                [typeof(HostLoadLevelState)] = new HostLoadLevelState(this, networkManager,
                    allServices.Single<IGameStartOverService>(),
                    allServices.Single<IPlayerSpawnPointService>(),
                    allServices.Single<IPersistentProgress>(),
                    allServices.Single<IUIFactory>()),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2) where TState : class, IPayloadState<TPayload1, TPayload2>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload1, payload2);
        }

        private TState ChangeState<TState>() where TState : class, IExitable
        {
            _activeState.Exit();
            TState state = _states[typeof(TState)] as TState;
            _activeState = state;
            return state;
        }
    }
}