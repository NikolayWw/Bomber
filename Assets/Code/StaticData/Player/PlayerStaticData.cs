using Unity.Netcode;
using UnityEngine;

namespace Code.StaticData.Player
{
    [CreateAssetMenu(menuName = "Static Data/Player", order = 0)]
    public class PlayerStaticData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}