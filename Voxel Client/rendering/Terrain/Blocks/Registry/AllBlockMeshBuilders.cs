using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelClient.rendering.Terrain.Blocks.Registry.Entries;
using VoxelEngineCore.Terrain.Blocks.Registry;

namespace VoxelClient.rendering.Terrain.Blocks.Registry
{
    public class AllBlockMeshBuilders
    {
        public static List<BlockMeshBuilder?> BlockMeshBuilders = new();

        public static void Register()
        {
            for (int i = 0; i < AllBlocks.States.Count; i++)
            {
                BlockMeshBuilders.Add(null);
            }

            new CubeEntry(AllBlocks.Dirt);
            new CubeEntry(AllBlocks.Log);
            new CubeEntry(AllBlocks.Leaves);
            new CubeEntry(AllBlocks.Stone);
            new CrossEntry(AllBlocks.TallGrass);
            new CrossEntry(AllBlocks.BlueFlower);
            new CrossEntry(AllBlocks.WhiteFlower);
        }
    }
}
