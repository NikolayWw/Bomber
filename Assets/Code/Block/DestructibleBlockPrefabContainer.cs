#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

namespace Code.Block
{
    public class DestructibleBlockPrefabContainer : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int MapSize { get; private set; }
        [field: SerializeField] public BlockTakeDamage[] DestructibleBlocks { get; private set; }
        [field: SerializeField] public NonBreakableBlock[] NonBreakableBlocks { get; private set; }
        [field: SerializeField] public WalkableBlock[] WalkableBlocks { get; private set; }

        [ContextMenu("Collect")]
        private void FindAllDestructibleBlocks()
        {
            DestructibleBlocks = transform.GetComponentsInChildren<BlockTakeDamage>();
            NonBreakableBlocks = transform.GetComponentsInChildren<NonBreakableBlock>();
            WalkableBlocks = transform.GetComponentsInChildren<WalkableBlock>();
#if UNITY_EDITOR

            EditorUtility.SetDirty(this);
#endif
        }
    }
}