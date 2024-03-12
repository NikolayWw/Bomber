using Code.Extension;
using Code.Services;
using Code.Services.Factory;
using Code.Services.Input;
using Code.Services.StaticData;
using Code.StaticData.Bomb;
using Code.StaticData.Improvements;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerSetsBomb : NetworkBehaviour
    {
        private IGameFactory _gameFactory;
        private IInputService _inputService;
        private IStaticDataService _dataService;

        [SerializeField] private PlayerCollectImprovement _collectImprovement;
        private float _currentRadius = 1.2f;//start radius
        private bool _inputToggle;

        private void Construct()
        {
            AllServices services = AllServices.Container;
            _inputService = services.Single<IInputService>();
            _gameFactory = services.Single<IGameFactory>();
            _collectImprovement.OnCollect += OnCollectImprovement;
            _dataService = services.Single<IStaticDataService>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner == false)
                return;

            Construct();
            Pause();
        }

        private void Update()
        {
            if (IsOwner == false)
                return;

            UpdateSetBomb();
        }

        [ClientRpc]
        public void OnGameStartClientRpc()
        {
            enabled = true;
        }

        [ClientRpc]
        public void OnGameOverClientRpc()
        {
            Pause();
        }

        private void UpdateSetBomb()
        {
            if (_inputService.AttackPress && _inputToggle)
            {
                _inputToggle = false;
                _gameFactory.CreateBombServerRpc(BombId.Bomb1, transform.position.ToVector2Int(), _currentRadius);
            }
            else if (_inputService.AttackPress == false && _inputToggle == false)
            {
                _inputToggle = true;
            }
        }

        private void OnCollectImprovement(ImprovementsId id)
        {
            if (ImprovementsId.BombRange != id)
                return;

            ImprovementConfigBombRadius config = _dataService.ForImprovement(id) as ImprovementConfigBombRadius;
            if (config == null)
            {
                Debug.LogError("I can't cast");
                return;
            }

            _currentRadius += config.Radius;
        }

        private void Pause()
        {
            enabled = false;
        }
    }
}