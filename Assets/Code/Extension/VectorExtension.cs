using UnityEngine;

namespace Code.Extension
{
    public static class VectorExtension
    {
        public static Vector2Int ToVector2Int(this Vector3 vector3)
        {
            return new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
        }
    }
}