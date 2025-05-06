using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineClient.rendering.shaders
{
    public class TerrainShader : GraphicsShader
    {
        [UniformLocation]
        public int proj_view_model;
        [UniformLocation]
        public int sun_dir;
        [UniformLocation]
        public int tex;
        public TerrainShader() : base("terrain")
        {
            
        }
    }
}
