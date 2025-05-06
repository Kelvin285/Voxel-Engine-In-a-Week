using VoxelEngineCore.Terrain.Blocks.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelCommon.Terrain.Blocks;

namespace VoxelEngineCore.Terrain.Blocks.Registry
{
    public class AllBlocks
    {
        public static List<Block> Blocks = new();
        public static List<BlockState> States = new();

        public static AirBlock Air = new();
        public static DirtBlock Dirt = new("dirt");
        public static StoneBlock Stone = new("stone");
        public static LogBlock Log = new("log");
        public static LeavesBlock Leaves = new("leaves");
        public static TallGrassBlock TallGrass = new("tall_grass");
        public static TallGrassBlock BlueFlower = new("blue_flower");
        public static TallGrassBlock WhiteFlower = new("white_flower");
    }
}
