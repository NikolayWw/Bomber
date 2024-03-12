using Code.Player;
using Code.Services;
using Code.Services.PlayerReporter;
using Code.UI.Services.UIFactoryService;
using Unity.Netcode;
using UnityEngine;

namespace Code.UI.Windows.HUD.PlayerSttus
{
    public class PlayerStatusWindow : MonoBehaviour
    {
        private IPlayerReporterService _playerReporterService;
        private IUIFactory _uiFactory;

        [SerializeField] private Transform _rootFields;

        private void Construct()
        {
            _playerReporterService = AllServices.Container.Single<IPlayerReporterService>();
            _uiFactory = AllServices.Container.Single<IUIFactory>();
        }

        private void Start()
        {
            Construct();
            Initialize(new NetworkListEvent<NetworkObjectReference>());
        }

        private void Initialize(NetworkListEvent<NetworkObjectReference> _)
        {
            _playerReporterService.ServerPlayerList.OnListChanged -= Initialize;

            if (_playerReporterService.ServerPlayerList.Count < 1)
            {
                _playerReporterService.ServerPlayerList.OnListChanged += Initialize;
            }
            else
            {
                InitFields();
                _playerReporterService.ServerPlayerList.OnListChanged += OnPlayersListChange;
            }
        }

        private void InitFields()
        {
            foreach (NetworkObjectReference reference in _playerReporterService.ServerPlayerList)
                AddField(reference);
        }

        private void OnPlayersListChange(NetworkListEvent<NetworkObjectReference> changeEvent) =>
            AddField(changeEvent.Value);

        private void AddField(NetworkObjectReference playerReference)
        {
            playerReference.TryGet(out NetworkObject networkPlayer);
            PlayerHealth health = networkPlayer != null ? networkPlayer.GetComponent<PlayerHealth>() : null;
            _uiFactory.CreatePlayerStatusField(_rootFields, health);
        }
    }
}