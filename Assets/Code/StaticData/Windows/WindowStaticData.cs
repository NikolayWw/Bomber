using Code.UI.Windows.GameOver;
using Code.UI.Windows.HUD;
using Code.UI.Windows.HUD.PlayerSttus;
using Code.UI.Windows.NetcodeWindow;
using UnityEngine;

namespace Code.StaticData.Windows
{
    [CreateAssetMenu(menuName = "Static Data/Windows", order = 0)]
    public class WindowStaticData : ScriptableObject
    {
        [field: SerializeField] public GameObject UIRootPrefab { get; private set; }
        [field: SerializeField] public GameObject HUDPrefab { get; private set; }
        [field: SerializeField] public GameObject DrawWindowPrefab { get; private set; }
        [field: SerializeField] public GameOverWindow GameOverWindowPrefab { get; private set; }
        [field: SerializeField] public StartHostClientWindow StartGameWindowPrefab { get; private set; }
        [field: SerializeField] public PlayerStatusField PlayerStatusFieldPrefab { get; private set; }
    }
}