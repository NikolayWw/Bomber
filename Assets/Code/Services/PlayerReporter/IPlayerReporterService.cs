using Unity.Netcode;

namespace Code.Services.PlayerReporter
{
    public interface IPlayerReporterService : IService
    {
        NetworkList<NetworkObjectReference> ServerPlayerList { get; }
        void AddPlayerServerRpc(NetworkObjectReference playerReference);
        void SetPlayersGameOverServerRpc();
        void SetPlayerStartGame();
        void Construct(NetworkManager networkManager);
    }
}