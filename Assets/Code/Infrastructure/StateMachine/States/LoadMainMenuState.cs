using Code.UI.Services.UIFactoryService;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadMainMenuState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;

        public LoadMainMenuState(IGameStateMachine gameStateMachine, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.CreateStartGameWindow();
            _gameStateMachine.Enter<LoopState>();
        }

        public void Exit()
        { }
    }
}