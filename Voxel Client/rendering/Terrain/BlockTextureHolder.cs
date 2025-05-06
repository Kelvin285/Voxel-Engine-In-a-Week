using VoxelEngineCore.Terrain.Blocks.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineClient.rendering.Terrain
{
    public class BlockTextureHolder
    {
        public struct UV
        {
            public float x1;
            public float x2;
            public float y1;
            public float y2;
        }

        public UV Top, Bottom, Left, Right, Front, Back;

        public UV this[BlockSideEnum side]
        {
            get => GetUV(side);
        }

        public UV GetUV(BlockSideEnum side)
        {
            switch (side)
            {
                case BlockSideEnum.Left: return Left;
                case BlockSideEnum.Right: return Right;
                case BlockSideEnum.Front: return Front;
                case BlockSideEnum.Back: return Back;
                case BlockSideEnum.Top: return Top;
                case BlockSideEnum.Bottom: return Bottom;
            }
            return Top;
        }
    }
}
