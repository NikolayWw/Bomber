using TMPro;
using UnityEngine;

namespace Code.UI.Windows.GameOver
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;

        public void Construct(string context) =>
            _nameText.text = context;
    }
}