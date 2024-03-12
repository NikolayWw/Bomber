using Code.Player;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static Code.Player.PlayerNameWindow;

namespace Code.UI.Windows.HUD.PlayerSttus
{
    public class PlayerStatusField : MonoBehaviour
    {
        private const string UnknownPlayerKey = "unknown player";

        [SerializeField] private TMP_Text _nameText;
        public void Construct(PlayerHealth playerHealth)
        {
            if (playerHealth != null)
            {
                playerHealth.OnHappened += OnHappened;
                _nameText.text = $"{NameKey} {playerHealth.NetworkObject.OwnerClientId + 1}";
            }
            else
            {
                _nameText.text = UnknownPlayerKey;
                OnHappened(null);
            }
        }

        private void OnHappened(NetworkObject _)
        {
            GetComponent<Image>().color = Color.red;
        }
    }
}