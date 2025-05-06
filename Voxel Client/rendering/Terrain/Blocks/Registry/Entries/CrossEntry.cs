using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngineCore.Terrain.Blocks;
using VoxelEngineCore.Terrain.Blocks.States;

namespace VoxelClient.rendering.Terrain.Blocks.Registry.Entries
{
    public class CrossEntry : BlockMeshBuilderEntry
    {
        public CrossEntry(Block block) : base(block)
        {
        }

        public override BlockMeshBuilder? GetMeshBuilderForState(BlockState state)
        {
            return new CrossMeshBuilder();
        }
    }
}
