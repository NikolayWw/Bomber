using Unity.Netcode;
using UnityEngine;

namespace Code.Player
{
    public class PlayerAnimation : NetworkBehaviour
    {
        private readonly int IdleMoveBlendHash = Animator.StringToHash("Move");
        [SerializeField] private Animator _animator;

        [ServerRpc]
        public void UpdateIdleServerRpc()
        {
            _animator.SetFloat(IdleMoveBlendHash, 0);
        }

        [ServerRpc]
        public void UpdateMoveServerRpc()
        {
            _animator.SetFloat(IdleMoveBlendHash, 1);
        }
    }
}