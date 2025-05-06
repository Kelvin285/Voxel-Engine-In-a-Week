using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelCommon.Terrain.Chunks.Structures.Registry
{
    public class AllStructures
    {
        public static List<Structure> Structures = new();

        public static TreeStructure Tree = new();
        public static GrassStructure Grass = new GrassStructure();
    }
}
