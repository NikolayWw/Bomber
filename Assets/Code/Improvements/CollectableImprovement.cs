using Code.StaticData.Improvements;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Code.Improvements
{
    public class CollectableImprovement : NetworkBehaviour
    {
        [field: SerializeField] public ImprovementsId Id { get; private set; }
        public readonly NetworkVariable<bool> TimeSpawnElapsed = new();

        public override void OnNetworkSpawn()
        {
            if (IsServer == false)
                return;

            StartCoroutine(DelayToCollect());
        }

        public void Collect()
        {
            DespawnThisServerRpc(NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnThisServerRpc(NetworkObjectReference reference)
        {
            reference.TryGet(out NetworkObject networkObject);
            networkObject.Despawn();
        }

        private IEnumerator DelayToCollect()
        {
            yield return new WaitForSeconds(0.1f);
            TimeSpawnElapsed.Value = true;
        }
    }
}