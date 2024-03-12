using Code.Services.CountdownTimer;
using Code.Services.ImprovementsSpawnService;
using Code.Services.PlayerReporter;
using Code.UI.Services.UIFactoryService;
using Unity.Collections;
using Unity.Netcode;
using static Code.Player.PlayerNameWindow;

namespace Code.Services.StartGameOver
{
    public class GameStartOverService : NetworkBehaviour, IGameStartOverService
    {
        private IUIFactory _uiFactory;
        private IPlayerReporterService _playerReporter;
        private ICountdownTimerService _countdownTimer;
        private NetworkManager _networkManager;
        private IImprovementsSpawner _improvementsSpawner;

        public NetworkVariable<bool> IsGameStarted { get; } = new();

        private int _maxPlayer;
        private int _currentPlayers;

        public void Construct(NetworkManager networkManager, IUIFactory uiFactory, ICountdownTimerService countdownTimer,
           IImprovementsSpawner improvementsSpawner, IPlayerReporterService playerReporter)
        {
            _networkManager = networkManager;
            _uiFactory = uiFactory;
            _countdownTimer = countdownTimer;
            _playerReporter = playerReporter;
            _improvementsSpawner = improvementsSpawner;
        }

        public void InitCheckStartGame(int maxPlayer)
        {
            _maxPlayer = maxPlayer;
            _networkManager.OnClientConnectedCallback += CheckAndStartGame;
            CheckAndStartGame(0);
        }

        public void AddPlayer()
        {
            _currentPlayers++;
        }

        public void RemovePlayer()
        {
            _currentPlayers--;
            CompareHappenedPlayers();
        }

        private void CheckAndStartGame(ulong _)
        {
            if (_networkManager.ConnectedClients.Count != _maxPlayer)
                return;

            _networkManager.OnClientConnectedCallback -= CheckAndStartGame;
            IsGameStarted.Value = true;
            StartGame();
        }

        private void StartGame()
        {
            _playerReporter.SetPlayerStartGame();
            _countdownTimer.StartTimerServerRpc();
            _improvementsSpawner.StartSpawn();
        }

        private void CompareHappenedPlayers()
        {
            if (_currentPlayers < 2)
            {
                PlayerWin();
            }
        }

        private void PlayerWin()
        {
            _playerReporter.SetPlayersGameOverServerRpc();
            _countdownTimer.StopTimerServerRpc();
            _improvementsSpawner.StopSpawn();

            foreach (NetworkObjectReference reference in _playerReporter.ServerPlayerList)
            {
                if (reference.TryGet(out NetworkObject networkPlayer))
                {
                    string winMessage = $"Winner: {NameKey} {networkPlayer.OwnerClientId + 1}";
                    SendWinServerRpc(new FixedString64Bytes(winMessage));
                    return;
                }
            }
            SendWinServerRpc(new FixedString64Bytes("Looks like you self-destructed"));
        }

        [ServerRpc]
        private void SendWinServerRpc(FixedString64Bytes context) =>
            SendWinClientRpc(context);

        [ClientRpc]
        private void SendWinClientRpc(FixedString64Bytes context) =>
            _uiFactory.CreateGameOverWindow(context.Value);
    }
}