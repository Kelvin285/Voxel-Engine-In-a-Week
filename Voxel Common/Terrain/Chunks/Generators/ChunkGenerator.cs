using DotnetNoise;
using VoxelEngineCore.Terrain.Blocks;
using VoxelEngineCore.Terrain.Blocks.Registry;
using VoxelEngineCore.Terrain.Blocks.States.PropertyEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelCommon.Terrain.Chunks.Structures.Registry;
using VoxelCommon.Terrain.Chunks.Structures;
using GlmSharp;

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
            var dirt = AllBlocks.Dirt.DefaultState.GlobalID;
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
            PlaceStructuresInitial(chunk);
            //GenerateStructures(chunk);
            chunk.MarkGenerated();
        }

        public void PlaceStructuresInitial(Chunk chunk)
        {
            for (int x = 0; x < Chunk.size; x++)
            {
                for (int z = 0; z < Chunk.size; z++)
                {
                    var y = chunk.GetHeightAt(x, z);

                    for (int i = 0; i < AllStructures.Structures.Count; i++)
                    {
                        var s = AllStructures.Structures[i];
                        if (s.CanGenerate(x, y, z, chunk, this))
                        {
                            int wx = x + chunk.x * Chunk.size;
                            int wy = y + chunk.y * Chunk.size;
                            int wz = z + chunk.z * Chunk.size;
                            StructureInstance instance = new StructureInstance(new ivec3(wx, wy, wz), s);

                            chunk.StructuresTaken.Add(instance);
                            break;
                        }
                    }
                }
            }
        }

        public void GenerateStructures(Chunk chunk)
        {
            foreach (var structure in chunk.StructuresTaken)
            {
                structure.GenerateInChunk(chunk, this);
            }
            chunk.StructuresTaken.Clear();
        }
    }
}
