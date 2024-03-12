using Unity.Netcode;
using UnityEngine;

namespace Code.StaticData.Improvements
{
    public abstract class BaseImprovementConfig : ScriptableObject
    {
        [SerializeField] private string _inspectorName;
        public abstract ImprovementsId Id { get; protected set; }
        [field: SerializeField] public NetworkObject Prefab { get; private set; }

        public void OnValidate()
        {
            _inspectorName = Id.ToString();
        }
    }
}