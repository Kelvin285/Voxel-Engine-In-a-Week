using DotnetNoise;
using VoxelEngineCore.Terrain.Blocks;
using VoxelEngineCore.Terrain.Blocks.Registry;
using VoxelEngineCore.Terrain.Blocks.States.PropertyEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Chunks.Generators
{
    public class ChunkGenerator
    {
        public FastNoise noise = new(new Random().Next());

        public ChunkGenerator()
        {
            noise.Frequency *= 0.5f;
            noise.FractalTypeMethod = FastNoise.FractalType.Fbm;
            noise.Octaves += 2;
        }

        public void GenerateChunk(Chunk chunk)
        {
            var dirt = AllBlocks.Log.DefaultState.GlobalID;
            var grass = AllBlocks.Dirt.DefaultState.With(DirtBlock.Grassy, BooleanProperty.True).GlobalID;
            var stone = AllBlocks.Stone.DefaultState.GlobalID;

            for (int x = 0; x < Chunk.size; x++)
            {
                for (int z = 0; z < Chunk.size; z++)
                {
                    int wx = chunk.x * Chunk.size + x;
                    int wz = chunk.z * Chunk.size + z;

                    float noise_h = noise.GetSimplexFractal(wx, wz) * 64;

                    int height = (int)noise_h;
                    for (int y = 0; y < Chunk.size; y++)
                    {
                        int wy = chunk.y * Chunk.size + y;

                        if (wy <= height)
                        {
                            if (wy == height)
                            {
                                chunk.SetStateID(x, y, z, grass);
                            } else
                            {
                                if (wy > height - 3)
                                {
                                    chunk.SetStateID(x, y, z, dirt);
                                } else
                                {
                                    chunk.SetStateID(x, y, z, stone);
                                }
                            }
                        }
                    }
                }
            }
            chunk.MarkGenerated();
        }
    }
}
