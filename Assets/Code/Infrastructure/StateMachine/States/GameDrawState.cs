using Code.UI.Services.UIFactoryService;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameDrawState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;

        public GameDrawState(IGameStateMachine gameStateMachine, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _gameStateMachine.Enter<LoopState>();
        }

        public void Exit()
        { }
    }
}