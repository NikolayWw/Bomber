using Code.Block;
using Code.Player;
using UnityEngine;

namespace Code.Logic
{
    public class MapComponentContainer : MonoBehaviour
    {
        [field: SerializeField] public PlayerSpawnMarker PlayerSpawnMarker { get; private set; }
        [field: SerializeField] public DestructibleBlockPrefabContainer DestructibleBlockPrefabContainer { get; private set; }
    }
}