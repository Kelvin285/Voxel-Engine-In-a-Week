using GlmSharp;
using VoxelEngineCore.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Entities
{
    public abstract class Entity
    {
        public vec3 position;
        public float pitch;
        public float yaw;

        public vec3 velocity;

        public World world;

        public Guid id;

        public Entity(World world)
        {
            this.world = world;
        }

        public abstract void Update();
    }
}
