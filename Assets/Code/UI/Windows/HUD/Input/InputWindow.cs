using Code.Services;
using Code.Services.Input;
using UnityEngine;

namespace Code.UI.Windows.HUD.Input
{
    public class InputWindow : MonoBehaviour
    {
        [SerializeField] private InputButton _leftButton;
        [SerializeField] private InputButton _rightButton;
        [SerializeField] private InputButton _upButton;
        [SerializeField] private InputButton _downButton;
        [SerializeField] private InputButton _attackButton;

        private void Construct()
        {
            IInputService inputService = AllServices.Container.Single<IInputService>();
            _leftButton.OnClick += press => { inputService.SetMoveAxis(press ? new Vector2(-1, 0) : Vector2.zero); };
            _rightButton.OnClick += press => { inputService.SetMoveAxis(press ? new Vector2(1, 0) : Vector2.zero); };
            _upButton.OnClick += press => { inputService.SetMoveAxis(press ? new Vector2(0, 1) : Vector2.zero); };
            _downButton.OnClick += press => { inputService.SetMoveAxis(press ? new Vector2(0, -1) : Vector2.zero); };
            _attackButton.OnClick += press => { inputService.SetAttack(press); };
        }

        private void Start()
        {
            Construct();
        }
    }
}