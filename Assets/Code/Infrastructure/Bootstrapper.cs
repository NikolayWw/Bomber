using Code.Infrastructure.Logic;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.States;
using Code.Services;
using UnityEngine;

namespace Code.Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private void Awake()
        {
            StartGame();
        }

        public void StartGame()
        {
            RegisterServices _ = new(this);
            AllServices.Container.Single<IGameStateMachine>().Enter<LoadMainMenuState>();
        }
    }
}