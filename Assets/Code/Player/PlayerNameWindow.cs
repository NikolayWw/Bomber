using Code.Services;
using Code.Services.PlayerReporter;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerNameWindow : NetworkBehaviour
    {
        public const string NameKey = "Player";

        private IPlayerReporterService _playerReporter;

        [SerializeField] private TMP_Text _nameText;

        private void Construct()
        {
            _playerReporter = AllServices.Container.Single<IPlayerReporterService>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                Construct();
            }
        }

        private void Start()
        {
            if (IsServer)
            {
                AddPlayerAndRefreshName();
            }
            if (IsOwner)
            {
                RefreshAllPlayers();
            }
        }

        private void AddPlayerAndRefreshName()
        {
            _playerReporter.AddPlayerServerRpc(NetworkObject);
            UpdateNameServerRpc(new FixedString64Bytes($"{NameKey} {NetworkObject.OwnerClientId + 1}"));
        }

        private static void RefreshAllPlayers()
        {
            IPlayerReporterService service = AllServices.Container.Single<IPlayerReporterService>();
            foreach (NetworkObjectReference reference in service.ServerPlayerList)
            {
                if (reference.TryGet(out NetworkObject networkObject))
                    networkObject.GetComponentInChildren<PlayerNameWindow>().RefreshName($"{NameKey} {networkObject.OwnerClientId + 1}");
            }
        }

        public void RefreshName(string playerNames) =>
            _nameText.text = playerNames;

        [ServerRpc(RequireOwnership = false)]
        private void UpdateNameServerRpc(FixedString64Bytes nameBytes) =>
            UpdateClientRpcClientRpc(new FixedString64Bytes(nameBytes));

        [ClientRpc]
        private void UpdateClientRpcClientRpc(FixedString64Bytes nameBytes) =>
            RefreshName(nameBytes.Value);
    }
}