using Code.Logic;
using Code.Services;
using System;
using Code.Services.StartGameOver;
using Unity.Netcode;

namespace Code.Player
{
    public class PlayerHealth : NetworkBehaviour, ITakeDamage
    {
        private IGameStartOverService _gameStartOver;
        public Action<NetworkObject> OnHappened;
        private bool _isGameOver;

        private void Construct()
        {
            _gameStartOver = AllServices.Container.Single<IGameStartOverService>();
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
                _gameStartOver.AddPlayer();
        }

        public override void OnNetworkDespawn()
        {
            OnHappened?.Invoke(NetworkObject);
        }

        public void TakeDamage(float value)
        {
            if (_isGameOver)
                return;

            DespawnProServerRpc(NetworkObject);
        }

        [ClientRpc]
        public void OnGameOverClientRpc()
        {
            _isGameOver = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnProServerRpc(NetworkObjectReference reference)
        {
            reference.TryGet(out var networkObject);
            networkObject.Despawn();
            _gameStartOver.RemovePlayer();
        }
    }
}