using TMPro;
using UnityEngine;

namespace Code.UI.Windows.NetcodeWindow
{
    public class LimitedInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        private void Start()
        {
            _inputField.onValueChanged.AddListener(CheckAndLimit);
        }

        private void CheckAndLimit(string arg)
        {
            if (int.TryParse(arg, out int value))
            {
                value = Mathf.Clamp(value, 1, 4);
                _inputField.text = value.ToString();
            }
            else
                _inputField.text = 1.ToString();
        }
    }
}