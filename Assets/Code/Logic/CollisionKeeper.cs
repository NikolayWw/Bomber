using UnityEngine;

namespace Code.Logic
{
    public class CollisionKeeper : MonoBehaviour
    {
        [field: SerializeField] public Collider2D[] Colliders { get; private set; }
    }
}