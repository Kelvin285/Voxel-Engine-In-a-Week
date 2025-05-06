using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VoxelEngineCore.Terrain.Blocks;
using VoxelEngineCore.Terrain.Blocks.Registry;
using VoxelEngineCore.Terrain.Blocks.States.PropertyEnums;
using VoxelEngineCore.Terrain.Chunks;
using VoxelEngineCore.Terrain.Chunks.Generators;

namespace VoxelCommon.Terrain.Chunks.Structures
{
    public class TreeStructure : Structure
    {
        public TreeStructure()
        {
        }

        public override bool CanGenerate(int x, int y, int z, Chunk chunk, ChunkGenerator generator)
        {
            var state = AllBlocks.States[chunk.GetStateID(x, y, z)];
            var block = state.Parent;
            if (state.Parent is DirtBlock && state.GetValue(DirtBlock.Grassy) == (int)BooleanProperty.True)
            {
                return (generator.noise.GetWhiteNoise(x, z) + 1) * 50 <= 2;
            }
            return false;
        }

        public override void GenerateInChunk(StructureInstance instance, Chunk chunk, ChunkGenerator generator)
        {
            int log_height = (int)((generator.noise.GetNoise(instance.PlacedPos.x * 40, instance.PlacedPos.z * 40) + 1) * 3 + 3);

            for (int h = 0; h < log_height; h++)
            {
                instance.PlaceBlockInWorld(instance.PlacedPos.x, instance.PlacedPos.y + h, instance.PlacedPos.z, AllBlocks.Log.DefaultState, chunk);
            }
            for (int x = -1; x <= 1; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        instance.PlaceBlockInWorld(instance.PlacedPos.x + x, instance.PlacedPos.y + log_height + y, instance.PlacedPos.z + z, AllBlocks.Leaves.DefaultState, chunk);
                    }
                }
            }

            instance.PlaceBlockInWorld(instance.PlacedPos.x, instance.PlacedPos.y + log_height + 2, instance.PlacedPos.z, AllBlocks.Leaves.DefaultState, chunk);
            instance.PlaceBlockInWorld(instance.PlacedPos.x - 1, instance.PlacedPos.y + log_height + 2, instance.PlacedPos.z, AllBlocks.Leaves.DefaultState, chunk);
            instance.PlaceBlockInWorld(instance.PlacedPos.x + 1, instance.PlacedPos.y + log_height + 2, instance.PlacedPos.z, AllBlocks.Leaves.DefaultState, chunk);
            instance.PlaceBlockInWorld(instance.PlacedPos.x, instance.PlacedPos.y + log_height + 2, instance.PlacedPos.z - 1, AllBlocks.Leaves.DefaultState, chunk);
            instance.PlaceBlockInWorld(instance.PlacedPos.x, instance.PlacedPos.y + log_height + 2, instance.PlacedPos.z + 1, AllBlocks.Leaves.DefaultState, chunk);
            instance.PlaceBlockInWorld(instance.PlacedPos.x, instance.PlacedPos.y + log_height + 3, instance.PlacedPos.z, AllBlocks.Leaves.DefaultState, chunk);
        }

        public override void SetBounds(StructureInstance instance)
        {
            int max_log_height = 7;
            instance.StartPos = new ivec3(instance.PlacedPos.x - 3, instance.PlacedPos.y, instance.PlacedPos.z - 3);
            instance.EndPos = new ivec3(instance.PlacedPos.x + 3, instance.PlacedPos.y + max_log_height + 4, instance.PlacedPos.z + 3);
        }
    }
}
