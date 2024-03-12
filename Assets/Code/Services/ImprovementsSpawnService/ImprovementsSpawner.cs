using Code.Extension;
using Code.Infrastructure.Logic;
using Code.Services.Factory;
using Code.Services.PersistentProgressService;
using Code.Services.PlayerReporter;
using Code.Services.StaticData;
using Code.StaticData.Improvements;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.ImprovementsSpawnService
{
    public class ImprovementsSpawner : IImprovementsSpawner
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPlayerReporterService _playerReporter;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgress _persistentProgress;
        private readonly IStaticDataService _dataService;
        private readonly List<Vector2Int> _availableSpawnPositions = new();

        private IEnumerator _spawnEnumerator;
        private WaitForSeconds _delaySpawn;

        public ImprovementsSpawner(ICoroutineRunner coroutineRunner, IPlayerReporterService playerReporter, IGameFactory gameFactory, IPersistentProgress persistentProgress, IStaticDataService dataService)
        {
            _coroutineRunner = coroutineRunner;
            _playerReporter = playerReporter;
            _gameFactory = gameFactory;
            _persistentProgress = persistentProgress;
            _dataService = dataService;
        }

        public void StartSpawn()
        {
            _delaySpawn = new WaitForSeconds(_dataService.ImprovementsSpawnData.DelayInSeconds);
            _coroutineRunner.StartCoroutine(_spawnEnumerator = Spawn());
        }

        public void StopSpawn()
        {
            _coroutineRunner.StopCoroutine(_spawnEnumerator);
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                yield return _delaySpawn;

                _availableSpawnPositions.Clear();
                AddAvailablePosition(_persistentProgress.BlocksProgress.WalkableBlocks);
                RemoveAvailablePosition(_gameFactory.SpawnedBombs);
                RemoveAvailablePosition(_gameFactory.SpawnedImprovement);
                RemoveAvailablePositionWithOffset(_playerReporter.ServerPlayerList);

                int improvementIndex = Random.Range(0, _dataService.ImprovementsData.Configs.Count);
                ImprovementsId improvementId = _dataService.ImprovementsData.Configs[improvementIndex].Id;

                int blocksCount = _availableSpawnPositions.Count;
                if (blocksCount < 1)
                {
                    Debug.LogError("there is no place to place Improvement");
                    continue;
                }

                int blockIndex = Random.Range(0, blocksCount);
                Vector2Int blockPosition = _availableSpawnPositions[blockIndex];

                _gameFactory.CreateImprovementServerRpc(improvementId, blockPosition);
            }
        }

        private void RemoveAvailablePositionWithOffset(NetworkList<NetworkObjectReference> objects)
        {
            Vector2Int[] directionsOne = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
                                            new(-1, 1) ,new(1,1),new(1,-1),new(-1,-1)};
            Vector2Int[] directionsTwo = { Vector2Int.up*2, Vector2Int.down*2, Vector2Int.left*2, Vector2Int.right*2,
                                             new Vector2Int(-1, 1)*2 ,new Vector2Int(1,1)*2, new Vector2Int(1,-1)*2, new Vector2Int(-1,-1)*2};

            for (int i = 0; i < objects.Count; i++)
            {
                NetworkObjectReference objReference = objects[i];
                if (objReference.TryGet(out var networkObject) == false)
                    continue;

                Vector2Int startKey = networkObject.transform.position.ToVector2Int();
                _availableSpawnPositions.Remove(startKey);

                foreach (Vector2Int directionOne in directionsOne)
                {
                    Vector2Int key = startKey + directionOne;
                    _availableSpawnPositions.Remove(key);
                }
                foreach (Vector2Int directionTow in directionsTwo)
                {
                    Vector2Int key = startKey + directionTow;
                    _availableSpawnPositions.Remove(key);
                }
            }
        }

        private void RemoveAvailablePosition(List<NetworkObject> objects)
        {
            foreach (NetworkObject obj in objects)
            {
                if (obj == null)
                    continue;

                Vector2Int key = obj.transform.position.ToVector2Int();
                _availableSpawnPositions.Remove(key);
            }
        }

        private void AddAvailablePosition(List<Vector2Int> vector2Ints)
        {
            foreach (Vector2Int vector2 in vector2Ints)
            {
                if (_availableSpawnPositions.Contains(vector2))
                    continue;

                _availableSpawnPositions.Add(vector2);
            }
        }
    }
}