using Code.Services.CountdownTimer;
using Code.Services.ImprovementsSpawnService;
using Code.Services.PlayerReporter;
using Code.UI.Services.UIFactoryService;
using Unity.Netcode;

namespace Code.Services.StartGameOver
{
    public interface IGameStartOverService : IService
    {
        void AddPlayer();
        void RemovePlayer();
        void Construct(NetworkManager networkManager, IUIFactory uiFactory, ICountdownTimerService countdownTimer,
            IImprovementsSpawner improvementsSpawner,
            IPlayerReporterService playerReporterService);

        void InitCheckStartGame(int maxPlayer);
        NetworkVariable<bool> IsGameStarted { get; }
    }
}