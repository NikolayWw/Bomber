using Code.Block;
using Code.Extension;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Data
{
    public class BlocksProgressData
    {
        private readonly Dictionary<Vector2Int, BlockTakeDamage> _destructibleBlocks = new();
        private readonly Dictionary<Vector2Int, NonBreakableBlock> _nonBreakableBlock = new();
        public List<Vector2Int> WalkableBlocks { get; } = new();
        public Dictionary<Vector2, BlockTakeDamage> DestructibleRawPositionBlocks { get; } = new();

        public void InitBlocks(DestructibleBlockPrefabContainer blockPrefabContainer)
        {
            FillDestructibleBlocks(blockPrefabContainer);
            FillNonBreakableBlocks(blockPrefabContainer);
            FillWalkableBlocks(blockPrefabContainer);
        }

        private void FillWalkableBlocks(DestructibleBlockPrefabContainer blockPrefabContainer)
        {
            foreach (WalkableBlock block in blockPrefabContainer.WalkableBlocks)
            {
                Vector2Int position = block.transform.position.ToVector2Int();
                if (_nonBreakableBlock.ContainsKey(position) || _destructibleBlocks.ContainsKey(position))
                    continue;

                WalkableBlocks.Add(position);
            }
        }

        public void RemoveDestructibleBlock(Vector2Int position)
        {
            _destructibleBlocks.Remove(position);
            DestructibleRawPositionBlocks.Remove(position);
            WalkableBlocks.Add(position);
        }

        private void FillNonBreakableBlocks(DestructibleBlockPrefabContainer blockPrefabContainer)
        {
            foreach (NonBreakableBlock block in blockPrefabContainer.NonBreakableBlocks)
            {
                Vector2Int position = block.transform.position.ToVector2Int();
                _nonBreakableBlock.Add(position, block);
            }
        }

        private void FillDestructibleBlocks(DestructibleBlockPrefabContainer blockPrefabContainer)
        {
            foreach (BlockTakeDamage block in blockPrefabContainer.DestructibleBlocks)
            {
                Vector2Int position = block.transform.position.ToVector2Int();
                _destructibleBlocks.Add(position, block);
                DestructibleRawPositionBlocks.Add(position, block);
            }
        }
    }
}