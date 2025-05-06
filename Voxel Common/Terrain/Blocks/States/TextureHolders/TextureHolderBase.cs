using VoxelEngineCore.Terrain.Blocks.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks.States.TextureHolders
{
    public abstract class TextureHolderBase
    {
        public abstract string GetTextureForSide(BlockSideEnum side);
    }
}
