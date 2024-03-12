using Code.Block;
using Code.Data;
using Code.Extension;
using Code.Logic;
using Code.Player;
using Code.Services;
using Code.Services.PersistentProgressService;
using Code.Services.PlayerReporter;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Code.Bomb
{
    public class BombAttack : NetworkBehaviour
    {
        public struct TakeDamageInfo
        {
            public ITakeDamage TakeDamage;
            public float Distance;
        }

        private BlocksProgressData _blocksData;
        private IPlayerReporterService _playerReporter;
        private float _attackRadius;
        private float _delayBeforeExplosion;

        [SerializeField] private CollisionKeeper _collisionKeeper;
        [SerializeField] private float _bodyColliderRadius;

        public void Construct(float attackRadius, float delayBeforeExplosion)
        {
            _blocksData = AllServices.Container.Single<IPersistentProgress>().BlocksProgress;
            _playerReporter = AllServices.Container.Single<IPlayerReporterService>();

            _attackRadius = attackRadius;
            _delayBeforeExplosion = delayBeforeExplosion;
        }

        private void Start()
        {
            IgnorePlayerColliders();

            if (IsServer == false)
                return;

            StartCoroutine(AttackTimer());
        }

        private IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(_delayBeforeExplosion);
            Attack();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            foreach (Collider2D collider1 in _collisionKeeper.Colliders)
                Physics2D.IgnoreCollision(collider1, other, false);
        }

        private void IgnorePlayerColliders()
        {
            Collider2D[] playersColliders = Physics2D.OverlapCircleAll(transform.position, _bodyColliderRadius);
            foreach (Collider2D collider1 in playersColliders)
            {
                foreach (Collider2D collider2 in _collisionKeeper.Colliders)
                    Physics2D.IgnoreCollision(collider1, collider2);
            }
        }

        private void Attack()
        {
            AttackBlock();
            AttackPlayers();
            NetworkObject.Despawn();
        }

        private void AttackPlayers()
        {
            List<TakeDamageInfo> players = new(_playerReporter.ServerPlayerList.Count);

            foreach (NetworkObjectReference reference in _playerReporter.ServerPlayerList)
            {
                if (false == reference.TryGet(out NetworkObject networkPlayer))
                    continue;

                float distance = Vector2.Distance(transform.position, networkPlayer.transform.position);
                if (distance > _attackRadius)
                    continue;

                players.Add(new TakeDamageInfo
                {
                    TakeDamage = networkPlayer.GetComponent<PlayerHealth>(),
                    Distance = distance
                });
            }

            players.ToTakeDamageSort();
            foreach (TakeDamageInfo damageInfo in players)
            {
                damageInfo.TakeDamage.TakeDamage(0);
            }
        }

        private void AttackBlock()
        {
            List<Vector2Int> removeBlocks = new();
            foreach (KeyValuePair<Vector2, BlockTakeDamage> entry in _blocksData.DestructibleRawPositionBlocks)
            {
                Vector2 position = entry.Key;
                if (Vector2.Distance(transform.position, position) > _attackRadius)
                    continue;

                removeBlocks.Add(new Vector2Int((int)position.x, (int)position.y));
                entry.Value.TakeDamage(0);
            }

            removeBlocks.ForEach(x => _blocksData.RemoveDestructibleBlock(x));
        }
    }
}