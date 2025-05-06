using VoxelEngineCore.Terrain.Blocks.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks.States.TextureHolders
{
    public class TextureHolderSingle : TextureHolderBase
    {
        public string Texture;

        public TextureHolderSingle(string texture)
        {
            Texture = texture;
        }

        public override string GetTextureForSide(BlockSideEnum side)
        {
            return Texture;
        }
    }
}
