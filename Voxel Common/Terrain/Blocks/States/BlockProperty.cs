using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks.States
{
    public class BlockProperty<T> : IBlockProperty where T : Enum
    {
        public string Name;
        public BlockProperty(string Name)
        {
            this.Name = Name;
        }

        public int GetNumValues()
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        public int GetNumBits()
        {
            return (int)glm.Floor(glm.Log2((double)GetNumValues())) + 1;
        }

        public Enum GetValue(int index)
        {
            return (T)Enum.GetValues(typeof(T)).GetValue(index)!;
        }
    }
}
