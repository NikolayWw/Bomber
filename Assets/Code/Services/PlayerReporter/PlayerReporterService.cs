using Code.Player;
using Unity.Netcode;

namespace Code.Services.PlayerReporter
{
    public class PlayerReporterService : NetworkBehaviour, IPlayerReporterService
    {
        private NetworkManager _networkManager;
        public NetworkList<NetworkObjectReference> ServerPlayerList { get; } = new();

        public void Construct(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }

        [ServerRpc]
        public void AddPlayerServerRpc(NetworkObjectReference playerReference)
        {
            ServerPlayerList.Add(playerReference);
        }

        [ServerRpc]
        public void SetPlayersGameOverServerRpc()
        {
            foreach (NetworkClient client in _networkManager.ConnectedClients.Values)
            {
                if (client == null || client.PlayerObject == null)
                    continue;

                client.PlayerObject.GetComponent<PlayerMove>().OnGameOverClientRpc();
                client.PlayerObject.GetComponent<PlayerHealth>().OnGameOverClientRpc();
                client.PlayerObject.GetComponent<PlayerSetsBomb>().OnGameOverClientRpc();
            }
        }

        public void SetPlayerStartGame()
        {
            foreach (NetworkClient client in _networkManager.ConnectedClients.Values)
            {
                client.PlayerObject.GetComponent<PlayerMove>().OnGameStartClientRpc();
                client.PlayerObject.GetComponent<PlayerSetsBomb>().OnGameStartClientRpc();
            }
        }
    }
}