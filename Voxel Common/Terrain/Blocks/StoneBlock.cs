using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngineCore.Terrain.Blocks;
using VoxelEngineCore.Terrain.Blocks.States;
using VoxelEngineCore.Terrain.Blocks.States.TextureHolders;

namespace VoxelCommon.Terrain.Blocks
{
    public class StoneBlock : Block
    {
        public StoneBlock(string name) : base(name)
        {
        }

        public override void BuildBlockState(BlockState state)
        {
            state.TextureHolder = new TextureHolderSingle(Name);
        }
    }
}
