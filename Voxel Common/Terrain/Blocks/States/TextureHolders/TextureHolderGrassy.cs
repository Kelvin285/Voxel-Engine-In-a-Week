using VoxelEngineCore.Terrain.Blocks.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks.States.TextureHolders
{
    public class TextureHolderGrassy : TextureHolderBase
    {
        public string Top;
        public string Bottom;
        public string Side;

        public TextureHolderGrassy(string top, string bottom, string side)
        {
            Top = top;
            Bottom = bottom;
            Side = side;
        }

        public override string GetTextureForSide(BlockSideEnum side)
        {
            if (side == BlockSideEnum.Top)
            {
                return Top;
            } else if (side == BlockSideEnum.Bottom)
            {
                return Bottom;
            }
            return Side;
        }
    }
}
