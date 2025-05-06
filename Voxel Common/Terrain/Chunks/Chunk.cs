using GlmSharp;
using Medallion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VoxelCommon.Terrain.Chunks.Structures;

namespace VoxelEngineCore.Terrain.Chunks
{
    public class Chunk
    {
        public static readonly int size = 64;
        public static readonly int size_AND = size - 1;
        public static readonly int size_SHIFT = (int)glm.Floor(glm.Log2((float)size));

        public int x, y, z;

        public bool Generated { get; private set; } = false;
        public bool Modified { get; private set; } = false;

        protected bool BlockJustUpdated = false;

        public ushort[,,] Blocks = new ushort[size, size, size];

        public Mutex mutex = new();

        public World world;

        public World.ThreadPoolEntry? GeneratingTask;

        public HashSet<StructureInstance> StructuresToGive = new();
        public HashSet<StructureInstance> StructuresTaken = new();

        public World.ThreadPoolEntry? DecoratingTask;

        public ulong[,] BlockColumn = new ulong[size, size];


        public Chunk(World world)
        {
            this.world = world;
        }

        public void MarkGenerated()
        {
            Generated = true;
        }

        /// <summary>
        /// Gets the current block state ID at the requested location
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public int GetStateID(int x, int y, int z)
        {
            x &= size_AND;
            y &= size_AND;
            z &= size_AND;

            return Blocks[x, y, z];
        }

        /// <summary>
        /// Sets the state ID at the current block location and returns whether or not it was successful
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SetStateID(int x, int y, int z, int id)
        {
            x &= size_AND;
            y &= size_AND;
            z &= size_AND;

            bool result = (ushort)id != Blocks[x, y, z];
            Blocks[x, y, z] = (ushort)id;

            Modified |= Generated && result;
            BlockJustUpdated = true && result;

            if (id == 0)
            {
                BlockColumn[x, z] = BlockColumn[x, z].ClearBit(y);
            } else
            {
                BlockColumn[x, z] = BlockColumn[x, z].SetBit(y);
            }

            return result;
        }

        public int GetHeightAt(int x, int z)
        {
            x &= size_AND;
            z &= size_AND;
            var column = BlockColumn[x, z];
            var leading = Bits.LeadingZeroBitCount(column);
            return size_AND - leading;
        }

        public virtual void OnAnyBlockUpdated()
        {
        }

        public virtual void Update()
        {
            if (BlockJustUpdated)
            {
                BlockJustUpdated = false;
                OnAnyBlockUpdated();
            }
            if (DecoratingTask == null)
            {
                if (StructuresToGive.Count > 0)
                {
                    List<StructureInstance> removing = new();
                    foreach (var structure in StructuresToGive)
                    {
                        List<ivec3> R = new();
                        foreach (var test in structure.NextToUse)
                        {
                            int X = test.x;
                            int Y = test.y;
                            int Z = test.z;

                            if (structure.UsedInChunk.Contains(test))
                            {
                                R.Add(test);
                                continue;
                            }

                            var chunk = world.GetChunk(X, Y, Z);
                            if (chunk != null)
                            {
                                if (chunk.x == X && chunk.y == Y && chunk.z == Z)
                                {
                                    if (chunk.DecoratingTask == null && chunk.Generated)
                                    {
                                        R.Add(test);
                                        chunk.StructuresTaken.Add(structure);
                                    }
                                }
                            }
                        }
                        foreach (var test in R)
                        {
                            structure.NextToUse.Remove(test);
                        }
                        if (structure.NextToUse.Count == 0)
                        {
                            removing.Add(structure);
                        }
                    }
                    foreach (var r in removing)
                    {
                        StructuresToGive.Remove(r);
                    }
                }
                if (StructuresTaken.Count > 0)
                {
                    DecoratingTask = new((index, result) =>
                    {
                        world.generator.GenerateStructures(this);
                    }, 1);

                    world.QueueAction(DecoratingTask);
                }
            } else
            {
                if (DecoratingTask.done)
                {
                    DecoratingTask = null;
                }
            }
        }
    }
}
