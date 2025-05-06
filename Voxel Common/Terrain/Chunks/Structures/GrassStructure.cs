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
    public class GrassStructure : Structure
    {

        public GrassStructure()
        {
        }

        public override bool CanGenerate(int x, int y, int z, Chunk chunk, ChunkGenerator generator)
        {
            var state = AllBlocks.States[chunk.GetStateID(x, y, z)];
            var block = state.Parent;
            if (state.Parent is DirtBlock && state.GetValue(DirtBlock.Grassy) == (int)BooleanProperty.True)
            {
                return (generator.noise.GetWhiteNoise(x + 500, z + 1273) + 1) * 50 <= 30;
            }
            return false;
        }

        public override void GenerateInChunk(StructureInstance instance, Chunk chunk, ChunkGenerator generator)
        {
            var rand = (int)((generator.noise.GetWhiteNoise(instance.PlacedPos.x, instance.PlacedPos.z) + 1) * 50);

            var state = AllBlocks.TallGrass.DefaultState;
            
            if (rand <= 20)
            {
                if ((rand & 3) == 0)
                {
                    state = AllBlocks.BlueFlower.DefaultState;
                }
                else if ((rand & 3) == 1)
                {
                    state = AllBlocks.WhiteFlower.DefaultState;
                }
            }

            instance.PlaceBlockInWorld(instance.PlacedPos.x, instance.PlacedPos.y + 1, instance.PlacedPos.z, state, chunk);
        }

        public override void SetBounds(StructureInstance instance)
        {
            instance.StartPos = instance.PlacedPos + new ivec3(0, 1, 0);
            instance.EndPos = instance.PlacedPos + new ivec3(0, 1, 0) + 1;
        }
    }
}
