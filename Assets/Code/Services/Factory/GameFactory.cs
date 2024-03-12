using Code.Bomb;
using Code.Services.StaticData;
using Code.StaticData.Bomb;
using Code.StaticData.Improvements;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.Factory
{
    public class GameFactory : NetworkBehaviour, IGameFactory
    {
        private AllServices _services;
        public List<NetworkObject> SpawnedBombs { get; } = new();
        public List<NetworkObject> SpawnedImprovement { get; } = new();

        public void Construct(AllServices services)
        {
            _services = services;
        }

        [ServerRpc(RequireOwnership = false)]
        public void CreateBombServerRpc(BombId id, Vector2 at, float attackRadius)
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            BombConfig bombConfig = dataService.ForBomb(id);

            NetworkObject bombInstance = Instantiate(bombConfig.Prefab, at, Quaternion.identity);
            bombInstance.GetComponent<BombAttack>().Construct(attackRadius, bombConfig.DelayBeforeExplosion);
            SpawnedBombs.Add(bombInstance);
            bombInstance.Spawn();
        }

        [ServerRpc]
        public void CreateImprovementServerRpc(ImprovementsId id, Vector2 at)
        {
            IStaticDataService dataService = GetService<IStaticDataService>();
            BaseImprovementConfig config = dataService.ForImprovement(id);

            NetworkObject improvementInstance = Instantiate(config.Prefab, at, Quaternion.identity);
            SpawnedImprovement.Add(improvementInstance);
            improvementInstance.Spawn();
        }

        private TService GetService<TService>() where TService : IService =>
            _services.Single<TService>();
    }
}