using Code.Infrastructure.Logic;
using Code.Infrastructure.StateMachine;
using Code.Services.CountdownTimer;
using Code.Services.Factory;
using Code.Services.ImprovementsSpawnService;
using Code.Services.Input;
using Code.Services.PersistentProgressService;
using Code.Services.PlayerReporter;
using Code.Services.PlayerSpawnPoint;
using Code.Services.StartGameOver;
using Code.Services.StaticData;
using Code.UI.Services.UIFactoryService;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services
{
    public class RegisterServices
    {
        private const string NetworkManagerPrefabPath = "NetworkManager";

        public RegisterServices(ICoroutineRunner coroutineRunner)
        {
            NetworkManager networkManger = InitNetworkManager();
            AllServices allServices = AllServices.Container;

            GameObject monoContainer = InitMonoContainer();
            allServices.RegisterSingle<IStaticDataService>(new StaticDataService());
            RegisterGameFactory(monoContainer, allServices);
            RegisterPlayerSpawnPoint(monoContainer, allServices);
            IPlayerReporterService playerReporter = RegisterPLayerReporter(monoContainer, allServices);
            IGameStartOverService gameStartOver = RegisterCompareGameOver(monoContainer, allServices);
            ICountdownTimerService countdownTimer = RegisterCountdownTimer(monoContainer, allServices);
            allServices.RegisterSingle<IUIFactory>(new UIFactory(allServices));
            allServices.RegisterSingle<IInputService>(new InputService());
            allServices.RegisterSingle<IPersistentProgress>(new PersistentProgress());

            allServices.RegisterSingle<IImprovementsSpawner>(new ImprovementsSpawner(coroutineRunner, allServices.Single<IPlayerReporterService>(),
                allServices.Single<IGameFactory>(),
                allServices.Single<IPersistentProgress>(),
                allServices.Single<IStaticDataService>()));

            playerReporter.Construct(networkManger);
            gameStartOver.Construct(networkManger, allServices.Single<IUIFactory>(), allServices.Single<ICountdownTimerService>(),
                allServices.Single<IImprovementsSpawner>(),
                allServices.Single<IPlayerReporterService>());

            countdownTimer.Construct(allServices.Single<IUIFactory>(), allServices.Single<IImprovementsSpawner>(), allServices.Single<IStaticDataService>(),
                allServices.Single<IPlayerReporterService>());

            allServices.RegisterSingle<IGameStateMachine>(new GameStateMachine(allServices, networkManger));
        }

        private static ICountdownTimerService RegisterCountdownTimer(GameObject monoContainer, AllServices allServices)
        {
            CountdownTimerService countdown = monoContainer.AddComponent<CountdownTimerService>();
            allServices.RegisterSingle<ICountdownTimerService>(countdown);
            return countdown;
        }

        private static IGameStartOverService RegisterCompareGameOver(GameObject monoContainer, AllServices allServices)
        {
            GameStartOverService gameStartStartOver = monoContainer.AddComponent<GameStartOverService>();
            allServices.RegisterSingle<IGameStartOverService>(gameStartStartOver);
            return gameStartStartOver;
        }

        private static IPlayerReporterService RegisterPLayerReporter(GameObject monoContainer, AllServices allServices)
        {
            PlayerReporterService reporter = monoContainer.AddComponent<PlayerReporterService>();
            allServices.RegisterSingle<IPlayerReporterService>(reporter);
            return reporter;
        }

        private static void RegisterPlayerSpawnPoint(GameObject monoContainer, AllServices allServices)
        {
            PlayerSpawnPointService spawnPoint = monoContainer.AddComponent<PlayerSpawnPointService>();
            allServices.RegisterSingle<IPlayerSpawnPointService>(spawnPoint);
        }

        private static void RegisterGameFactory(GameObject monoContainer, AllServices allServices)
        {
            GameFactory factory = monoContainer.AddComponent<GameFactory>();
            factory.Construct(allServices);
            allServices.RegisterSingle<IGameFactory>(factory);
        }

        private static NetworkManager InitNetworkManager()
        {
            NetworkManager networkManagerPrefab = Resources.Load<NetworkManager>(NetworkManagerPrefabPath);
            return Object.Instantiate(networkManagerPrefab);
        }

        private static GameObject InitMonoContainer()
        {
            GameObject monoContainer = new GameObject("Services Container");
            monoContainer.AddComponent<NetworkObject>();
            return monoContainer;
        }
    }
}