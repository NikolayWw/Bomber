using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.StaticData.Bomb
{
    [Serializable]
    public class BombConfig
    {
        [SerializeField] private string _inspectorName;
        [field: SerializeField] public BombId Id { get; private set; }
        [field: SerializeField] public NetworkObject Prefab { get; private set; }
        [field: SerializeField] public float DelayBeforeExplosion { get; private set; }
        public void OnValidate()
        {
            _inspectorName = Id.ToString();
        }
    }
}