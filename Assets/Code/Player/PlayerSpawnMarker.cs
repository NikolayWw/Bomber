using UnityEngine;

namespace Code.Player
{
    public class PlayerSpawnMarker : MonoBehaviour
    {
        [field: SerializeField] public Transform[] Points { get; private set; }

        private void OnValidate()
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Transform point = Points[i];
                if (point != null)
                    point.gameObject.name = $"Point[{i + 1}]";
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (Transform point in Points)
            {
                if (point != null)
                    Gizmos.DrawSphere(point.position, 0.5f);
            }
        }
    }
}