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
    public class DirtBlock : Block
    {

        public static BlockProperty<BooleanProperty> Grassy = new("Grassy");

        public DirtBlock(string name) : base(name)
        {
        }

        public override void BuildPropertyTable()
        {
            AddProperty(Grassy);
        }

        public override void BuildBlockState(BlockState state)
        {
            if (state.GetValue(Grassy) == (int)BooleanProperty.True)
            {
                state.TextureHolder = new TextureHolderGrassy(Name + "_grass_top", Name, Name + "_grass_side");
            } else
            {
                state.TextureHolder = new TextureHolderSingle(Name);
            }
        }
    }
}
