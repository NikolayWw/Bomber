using Code.Services;
using Code.Services.Input;
using Code.Services.PlayerSpawnPoint;
using Code.Services.StaticData;
using Code.StaticData.Improvements;
using Code.StaticData.Player;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerMove : NetworkBehaviour
    {
        private IInputService _inputService;
        private IStaticDataService _dataService;
        private IEnumerator _improvementTimerEnumerator;
        private PlayerStaticData _playerData;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private PlayerCollectImprovement _collectImprovement;
        [SerializeField] private GameObject _playerPoint;
        private float _improveSpeed = 1;

        private void Construct()
        {
            _playerPoint.SetActive(true);
            _collectImprovement.OnCollect += OnCollectImprovement;
            _inputService = AllServices.Container.Single<IInputService>();
            _dataService = AllServices.Container.Single<IStaticDataService>();
            _playerData = AllServices.Container.Single<IStaticDataService>().PlayerData;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                SetStartPosition();
            }

            if (IsOwner == false)
                return;

            Construct();
            Pause();
        }

        private void FixedUpdate()
        {
            if (IsOwner == false)
                return;

            Vector2 inputAxis = _inputService.MoveAxis;
            float speed = _playerData.Speed * _improveSpeed;

            if (IsServer)
            {
                UpdateMove(inputAxis, speed);
                UpdateAnimationsServerRpc(inputAxis);
            }
            else if (IsClient)
            {
                UpdateMoveServerRpc(inputAxis, speed);
                UpdateAnimations(inputAxis);
            }
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

        [ServerRpc]
        private void UpdateMoveServerRpc(Vector2 inputAxis, float speed)
        {
            UpdateMove(inputAxis, speed);
        }

        private void UpdateMove(Vector3 inputAxis, float speed)
        {
            Vector3 position = transform.position + speed * Time.fixedDeltaTime * inputAxis;
            _rigidbody.MovePosition(position);
        }

        [ServerRpc]
        private void UpdateAnimationsServerRpc(Vector2 inputAxis)
        {
            UpdateAnimations(inputAxis);
        }

        private void UpdateAnimations(Vector2 inputAxis)
        {
            if (inputAxis == Vector2.zero)
                _animation.UpdateIdleServerRpc();
            else
                _animation.UpdateMoveServerRpc();
        }

        private void OnCollectImprovement(ImprovementsId id)
        {
            if (ImprovementsId.PlayerSpeed != id)
                return;

            ImprovementConfigPlayerSpeed config = _dataService.ForImprovement(id) as ImprovementConfigPlayerSpeed;
            if (config == null)
            {
                Debug.LogError("I can't cast");
                return;
            }

            if (_improvementTimerEnumerator != null)
                StopCoroutine(_improvementTimerEnumerator);

            StartCoroutine(_improvementTimerEnumerator = ImprovementTimer(config));
        }

        private IEnumerator ImprovementTimer(ImprovementConfigPlayerSpeed config)
        {
            _improveSpeed = config.Speed;
            yield return new WaitForSeconds(config.Duration);
            _improveSpeed = 1;
        }

        private void SetStartPosition()
        {
            IPlayerSpawnPointService pointService = AllServices.Container.Single<IPlayerSpawnPointService>();

            if (pointService.Initialized == false)
                pointService.OnInitialized += SetPosition;
            else
                SetPosition();

            return;
            void SetPosition()
            {
                transform.position = pointService.GetPosition();
            }
        }

        private void Pause()
        {
            enabled = false;
            _rigidbody.velocity = Vector2.zero;
        }
    }
}