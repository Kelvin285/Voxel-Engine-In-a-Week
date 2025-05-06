using VoxelEngineCore.Terrain.Blocks.States;
using VoxelEngineCore.Terrain.Blocks.States.PropertyEnums;
using VoxelEngineCore.Terrain.Blocks.States.TextureHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks
{
    public class LogBlock : Block
    {

        public LogBlock(string name) : base(name)
        {
        }

        public override void BuildPropertyTable()
        {
            
        }

        public override void BuildBlockState(BlockState state)
        {
            state.TextureHolder = new TextureHolderGrassy(Name + "_top", Name + "_top", Name + "_side");
        }
    }
}
