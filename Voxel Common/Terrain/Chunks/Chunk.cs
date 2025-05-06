using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

            return result;
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
        }
    }
}
