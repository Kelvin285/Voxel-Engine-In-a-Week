using VoxelEngineCore.Terrain.Blocks.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks
{
    public class AirBlock : Block
    {
        public AirBlock() : base("air")
        {
        }

        public override void BuildBlockState(BlockState state)
        {
            state.Visible = false;
            state.Transparent = true;
        }
    }
}
