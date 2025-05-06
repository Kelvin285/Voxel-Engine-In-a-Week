using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngineCore.Terrain.Blocks;
using VoxelEngineCore.Terrain.Blocks.States;

namespace VoxelClient.rendering.Terrain.Blocks.Registry
{
    public abstract class BlockMeshBuilderEntry
    {
        public BlockMeshBuilderEntry(Block block)
        {
            for (int i = 0; i < block.States.Count; i++)
            {
                var builder = GetMeshBuilderForState(block.States[i]);
                AllBlockMeshBuilders.BlockMeshBuilders[block.States[i].GlobalID] = builder;
            }
        }

        public abstract BlockMeshBuilder? GetMeshBuilderForState(BlockState state);
    }
}
