using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelCommon.Terrain.Chunks.Structures.Registry;
using VoxelEngineCore.Terrain.Blocks.States;
using VoxelEngineCore.Terrain.Chunks;
using VoxelEngineCore.Terrain.Chunks.Generators;

namespace VoxelCommon.Terrain.Chunks.Structures
{
    public abstract class Structure
    {
        public readonly int GlobalID;
        public Structure()
        {
            GlobalID = AllStructures.Structures.Count;
            AllStructures.Structures.Add(this);
        }

        public abstract void SetBounds(StructureInstance instance);

        public abstract void GenerateInChunk(StructureInstance instance, Chunk chunk, ChunkGenerator generator);

        public abstract bool CanGenerate(int x, int y, int z, Chunk chunk, ChunkGenerator generator);
    }
}
