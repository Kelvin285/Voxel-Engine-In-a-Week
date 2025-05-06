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
    public class LeavesBlock : Block
    {
        public LeavesBlock(string name) : base(name)
        {
        }

        public override void BuildBlockState(BlockState state)
        {
            state.TextureHolder = new TextureHolderSingle(Name);
            state.Transparent = true;
            state.ShowAllFaces = true;
        }
    }
}
