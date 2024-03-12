using System.Collections.Generic;
using Code.StaticData.Bomb;
using Code.StaticData.Improvements;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.Factory
{
    public interface IGameFactory : IService
    {
        void CreateBombServerRpc(BombId id, Vector2 at, float attackRadius);
        void CreateImprovementServerRpc(ImprovementsId id, Vector2 at);
        List<NetworkObject> SpawnedBombs { get; }
        List<NetworkObject> SpawnedImprovement { get; }
    }
}