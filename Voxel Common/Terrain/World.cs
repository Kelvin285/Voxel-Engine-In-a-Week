using VoxelEngineCore.Entities;
using VoxelEngineCore.Terrain.Blocks.Registry;
using VoxelEngineCore.Terrain.Blocks.States;
using VoxelEngineCore.Terrain.Chunks;
using VoxelEngineCore.Terrain.Chunks.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelCommon.Terrain.Enums;

namespace VoxelEngineCore.Terrain
{
    public class World
    {
        public Dictionary<Guid, Entity> entities = new();

        public ChunkGenerator generator = new();

        public enum ThreadGroup
        {
            Any, First, Second
        }

        public class ThreadPoolEntry
        {
            public bool done = false;
            public Action<int, object?[]> Action;
            public object?[] output;
            public ThreadGroup group = ThreadGroup.Any;
            public ThreadPoolEntry(Action<int, object?[]> action, int num_outputs)
            {
                Action = action;
                output = new object?[num_outputs];
            }
        }

        public class ThreadPoolThread
        {
            public ThreadPoolEntry? entry;
            public bool running;
            public Thread thread;

            public ThreadPoolThread(int index)
            {
                running = true;

                thread = new Thread(() =>
                {
                    while (running)
                    {
                        if (entry != null)
                        {
                            entry.Action.Invoke(index, entry.output);
                            entry.done = true;
                            entry = null;
                        }
                        Thread.Sleep(1);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
        }


        public List<ThreadPoolEntry> ThreadPoolQueue = new();
        public List<ThreadPoolThread> Threads = new();
        public ThreadPoolEntry QueueAction(ThreadPoolEntry entry)
        {
            ThreadPoolQueue.Add(entry);

            return entry;
        }

        public World()
        {
            for (int i = 0; i < 16; i++)
            {
                Threads.Add(new ThreadPoolThread(i));
            }
        }

        public void SpawnEntity(Entity entity, Guid? guid = null)
        {
            if (guid.HasValue)
            {
                entity.id = guid.Value;
            } else
            {
                entity.id = Guid.NewGuid();
            }

            entities.Add(entity.id, entity);
        }

        public virtual void Update()
        {
            if (ThreadPoolQueue.Count > 0)
            {
                for (int i = 0; i < Threads.Count; i++)
                {
                    if (Threads[i].entry == null)
                    {
                        for (int j = 0; j < ThreadPoolQueue.Count; j++)
                        {
                            var first = ThreadPoolQueue[j];
                            bool launch = first.group == ThreadGroup.Any || i < Threads.Count / 2 && first.group == ThreadGroup.First || i >= Threads.Count / 2 && first.group == ThreadGroup.Second;
                            
                            if (launch)
                            {
                                ThreadPoolQueue.RemoveAt(j);
                                Threads[i].entry = first;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public virtual void FixedUpdate()
        {
            foreach (var pair in entities)
            {
                pair.Value.Update();
            }
        }

        public virtual Chunk? GetChunk(int x, int y, int z)
        {
            return null;
        }

        public BlockState GetBlockState(int x, int y, int z)
        {
            var chunk = GetChunk(x >> Chunk.size_SHIFT, y >> Chunk.size_SHIFT, z >> Chunk.size_SHIFT);
            if (chunk != null)
            {
                if (chunk.x == x >> Chunk.size_SHIFT && chunk.y == y >> Chunk.size_SHIFT && chunk.z == z >> Chunk.size_SHIFT)
                {
                    return AllBlocks.States[chunk.GetStateID(x & Chunk.size_AND, y & Chunk.size_AND, z & Chunk.size_AND)];
                }
            }
            return AllBlocks.Air.DefaultState;
        }

        public EnumPlaceResult SetBlockState(int x, int y, int z, BlockState state)
        {
            var chunk = GetChunk(x >> Chunk.size_SHIFT, y >> Chunk.size_SHIFT, z >> Chunk.size_SHIFT);
            if (chunk != null)
            {
                if (chunk.x == x >> Chunk.size_SHIFT && chunk.y == y >> Chunk.size_SHIFT && chunk.z == z >> Chunk.size_SHIFT)
                {
                    if (chunk.SetStateID(x & Chunk.size_AND, y & Chunk.size_AND, z & Chunk.size_AND, state.GlobalID))
                    {
                        OnBlockSet(x, y, z);
                        return EnumPlaceResult.Success;
                    }
                    return EnumPlaceResult.FailSameBlock;
                }
            }
            return EnumPlaceResult.FailNoChunk;
        }

        public virtual void OnBlockSet(int x, int y, int z)
        {
            var wx = x >> Chunk.size_SHIFT;
            var wy = y >> Chunk.size_SHIFT;
            var wz = z >> Chunk.size_SHIFT;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        GetChunk(wx + i, wy + j, wz + k)!.OnAnyBlockUpdated();
                    }
                }
            }
        }

        public virtual void CloseWorld()
        {
            for (int i = 0; i < Threads.Count; i++)
            {
                Threads[i].running = false;
            }
        }
    }
}
