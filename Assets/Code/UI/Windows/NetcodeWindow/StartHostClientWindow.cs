using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.States;
using Code.Services;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows.NetcodeWindow
{
    public class StartHostClientWindow : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;

        [SerializeField] private TMP_InputField _maxPlayerCountInputField;
        [SerializeField] private Button _startHostButton;
        [SerializeField] private Button _startClientButton;

        private void Construct()
        {
            _gameStateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void Awake()
        {
            Construct();
        }

        private void Start()
        {
            _startHostButton.onClick.AddListener(StartHost);
            _startClientButton.onClick.AddListener(StartClient);
        }

        private void StartClient()
        {
            _gameStateMachine.Enter<ClientLoadLevelState, Action>(CloseThis);
        }

        private void StartHost()
        {
            if (int.TryParse(_maxPlayerCountInputField.text, out int maxPlayer) == false)
            {
                Debug.LogError("I can't convert this to int");
                return;
            }

            _gameStateMachine.Enter<HostLoadLevelState, Action, int>(CloseThis, maxPlayer);
        }

        private void CloseThis()
        {
            Destroy(gameObject);
        }
    }
}