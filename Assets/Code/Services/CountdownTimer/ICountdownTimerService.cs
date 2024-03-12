using Code.Services.ImprovementsSpawnService;
using Code.Services.PlayerReporter;
using Code.Services.StaticData;
using Code.UI.Services.UIFactoryService;
using Unity.Netcode;

namespace Code.Services.CountdownTimer
{
    public interface ICountdownTimerService : IService
    {
        void Construct(IUIFactory uiFactory, IImprovementsSpawner improvementsSpawner, IStaticDataService dataService,
            IPlayerReporterService playerReporterService);
        void StartTimerServerRpc();
        NetworkVariable<int> CurrentSeconds { get; } //start time
        void StopTimerServerRpc();
    }
}