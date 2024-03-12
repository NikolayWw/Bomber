using Code.Improvements;
using Code.Logic;
using Code.StaticData.Improvements;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerCollectImprovement : NetworkBehaviour
    {
        public Action<ImprovementsId> OnCollect;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsOwner == false)
                return;

            if (other.TryGetComponent(out LinkToRootOnCollider linkRoot) == false
                || linkRoot.Root.TryGetComponent(out CollectableImprovement collectable) == false
                || collectable.TimeSpawnElapsed.Value == false)
                return;

            OnCollect?.Invoke(collectable.Id);
            collectable.Collect();
        }
    }
}