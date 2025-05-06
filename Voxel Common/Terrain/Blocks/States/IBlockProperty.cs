using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks.States
{
    public interface IBlockProperty
    {
        public int GetNumValues();

        public int GetNumBits();

        public Enum GetValue(int index);
    }
}
