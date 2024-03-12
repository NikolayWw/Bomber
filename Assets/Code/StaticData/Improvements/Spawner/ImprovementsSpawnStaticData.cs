using UnityEngine;

namespace Code.StaticData.Improvements.Spawner
{
    [CreateAssetMenu(menuName = "Static Data/Improvements/Spawn Data", order = -1)]
    public class ImprovementsSpawnStaticData : ScriptableObject
    {
        [field: SerializeField] public float DelayInSeconds { get; private set; } = 5;
    }
}