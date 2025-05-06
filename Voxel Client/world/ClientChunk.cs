using GlmSharp;
using VoxelEngineCore.Terrain;
using VoxelEngineCore.Terrain.Chunks;
using VoxelEngineClient.rendering.Terrain;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineClient.world
{
    public class ClientChunk : Chunk
    {
        public TerrainMesh? mesh;

        public mat4 pos_mat;

        public bool NeedsToRebuild = false;

        public World.ThreadPoolEntry? BuildTask;

        public ClientChunk(World world) : base(world)
        {
        }

        public override void OnAnyBlockUpdated()
        {
            base.OnAnyBlockUpdated();
            NeedsToRebuild = true;
        }

        public void Render(TerrainRenderer renderer, mat4 proj_view)
        {
            var gl = GameClient.GetInstance().GetGL();

            if (NeedsToRebuild)
            {
                if (BuildTask == null)
                {
                    NeedsToRebuild = false;

                    int x = this.x;
                    int y = this.y;
                    int z = this.z;
                    BuildTask = new(new((index, result) =>
                    {
                        result[0] = TerrainMesh.PrepareFromChunk((ClientWorld)world, x, y, z);
                    }), 1);

                    BuildTask.group = World.ThreadGroup.First;

                    world.QueueAction(BuildTask);
                }
            }

            if (BuildTask != null)
            {
                if (BuildTask.done)
                {
                    pos_mat = mat4.Translate(x * size, y * size, z * size);
                    if (mesh != null)
                    {
                        mesh.Dispose();
                    }

                    TerrainMesh.PrepareResult? result = (TerrainMesh.PrepareResult?)BuildTask.output[0];

                    if (result != null)
                    {
                        mesh = new TerrainMesh(result.verts, result.colors, result.uvs, result.normals, result.indices);
                    }

                    BuildTask = null;
                }
            }

            if (mesh != null)
            {
                renderer.TerrainShader.SetMat4(renderer.TerrainShader.proj_view_model, proj_view * pos_mat);
                mesh.Render();
            }
        }

        public void Dispose()
        {
            if (mesh != null)
            {
                mesh.Dispose();
                mesh = null;
            }
        }
    }
}
