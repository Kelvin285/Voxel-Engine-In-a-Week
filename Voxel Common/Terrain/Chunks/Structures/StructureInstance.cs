using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngineCore.Terrain.Blocks.States;
using VoxelEngineCore.Terrain.Chunks;
using VoxelEngineCore.Terrain.Chunks.Generators;

namespace VoxelCommon.Terrain.Chunks.Structures
{
    public class StructureInstance
    {
        public ivec3 PlacedPos;
        public Structure structure;
        public ivec3 StartPos;
        public ivec3 EndPos;
        public HashSet<ivec3> UsedInChunk = new();
        public HashSet<ivec3> NextToUse = new();

        public Mutex mutex = new();

        public StructureInstance(ivec3 placedPos, Structure structure)
        {
            PlacedPos = placedPos;
            this.structure = structure;
        }

        public void GenerateInChunk(Chunk chunk, ChunkGenerator generator)
        {
            mutex.WaitOne();
            UsedInChunk.Add(new ivec3(chunk.x, chunk.y, chunk.z));
            mutex.ReleaseMutex();
            structure.GenerateInChunk(this, chunk, generator);
        }

        public void GiveStructureAway(Chunk chunk)
        {
            if (!chunk.StructuresToGive.Contains(this))
            {
                chunk.StructuresToGive.Add(this);
            }
        }

        public void PlaceBlockInWorld(int x, int y, int z, BlockState state, Chunk chunk)
        {
            if (x >= chunk.x * Chunk.size && y >= chunk.y * Chunk.size && z >= chunk.z * Chunk.size && x < chunk.x * Chunk.size + Chunk.size && y < chunk.y * Chunk.size + Chunk.size && z < chunk.z * Chunk.size + Chunk.size)
            {
                if (chunk.world.SetBlockState(x, y, z, state) == Enums.EnumPlaceResult.FailNoChunk)
                {
                    var pos = new ivec3(x >> Chunk.size_SHIFT, y >> Chunk.size_SHIFT, z >> Chunk.size_SHIFT);

                    mutex.WaitOne();
                    if (!NextToUse.Contains(pos) && !UsedInChunk.Contains(pos))
                    {
                        NextToUse.Add(pos);
                        GiveStructureAway(chunk);
                    }
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                var pos = new ivec3(x >> Chunk.size_SHIFT, y >> Chunk.size_SHIFT, z >> Chunk.size_SHIFT);

                mutex.WaitOne();
                if (!NextToUse.Contains(pos) && !UsedInChunk.Contains(pos))
                {
                    NextToUse.Add(pos);
                    GiveStructureAway(chunk);
                }
                mutex.ReleaseMutex();
            }
        }
    }
}
