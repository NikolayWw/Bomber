using Code.Services;
using Code.Services.CountdownTimer;
using TMPro;
using UnityEngine;

namespace Code.UI.Windows.HUD
{
    public class CountdownTimerWindow : MonoBehaviour
    {
        private ICountdownTimerService _countdownTimer;
        [SerializeField] private TMP_Text _timerText;

        private void Construct()
        {
            _countdownTimer = AllServices.Container.Single<ICountdownTimerService>();
        }

        private void Start()
        {
            Construct();
            _countdownTimer.CurrentSeconds.OnValueChanged += Refresh;
        }

        private void Refresh(int _, int newValue)
        {
            _timerText.text = $"Seconds: {newValue}";
        }
    }
}