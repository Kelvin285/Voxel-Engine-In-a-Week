using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks.Enums
{
    public static class BlockSideEnumFunctions
    {
        public static vec3 GetNormal(this BlockSideEnum side)
        {
            switch (side)
            {
                case BlockSideEnum.Left: return new(-1, 0, 0);
                case BlockSideEnum.Right: return new(1, 0, 0);
                case BlockSideEnum.Top: return new(0, 1, 0);
                case BlockSideEnum.Bottom: return new(0, -1, 0);
                case BlockSideEnum.Front: return new(0, 0, -1);
                case BlockSideEnum.Back: return new(0, 0, 1);
            }
            return new(0, 1, 0);
        }
    }
}
