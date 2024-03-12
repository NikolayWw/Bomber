using Code.Logic;
using Unity.Netcode;

namespace Code.Block
{
    public class BlockTakeDamage : NetworkBehaviour, ITakeDamage
    {
        public void TakeDamage(float _)
        {
            if (IsServer == false)
                return;

            NetworkObject.Despawn();
        }
    }
}