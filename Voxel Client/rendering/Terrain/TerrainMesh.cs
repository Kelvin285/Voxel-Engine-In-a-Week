using GlmSharp;
using VoxelEngineCore.Terrain;
using VoxelEngineCore.Terrain.Blocks.Enums;
using VoxelEngineCore.Terrain.Blocks.States;
using VoxelEngineCore.Terrain.Chunks;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VoxelClient.rendering.Terrain.Blocks.Registry;
using VoxelEngineClient.world;

namespace VoxelEngineClient.rendering.Terrain
{
    public class TerrainMesh : AutoDispose
    {
        public uint Handle;
        public uint VertexHandle;
        public uint UvHandle;
        public uint ColorHandle;
        public uint NormalHandle;
        public uint IndexHandle;

        public uint IndexCount { get; private set; }

        public unsafe TerrainMesh(List<vec3> verts, List<vec3> colors, List<vec2> uvs, List<vec3> normals, List<uint> indices)
        {
            IndexCount = (uint)indices.Count;

            var gl = GameClient.GetInstance().GetGL();

            Handle = gl.CreateVertexArray();

            VertexHandle = gl.GenBuffer();
            UvHandle = gl.GenBuffer();
            ColorHandle = gl.GenBuffer();
            NormalHandle = gl.GenBuffer();
            IndexHandle = gl.GenBuffer();

            gl.BindVertexArray(Handle);

            gl.BindBuffer(BufferTargetARB.ArrayBuffer, VertexHandle);
            fixed (void* data = verts.ToArray())
            {
                gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(vec3) * verts.Count), data, BufferUsageARB.StaticDraw);
            }
            gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            gl.EnableVertexAttribArray(0);


            gl.BindBuffer(BufferTargetARB.ArrayBuffer, UvHandle);
            fixed (void* data = uvs.ToArray())
            {
                gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(vec2) * uvs.Count), data, BufferUsageARB.StaticDraw);
            }
            gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, 0);
            gl.EnableVertexAttribArray(1);


            gl.BindBuffer(BufferTargetARB.ArrayBuffer, NormalHandle);
            fixed (void* data = normals.ToArray())
            {
                gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(vec3) * normals.Count), data, BufferUsageARB.StaticDraw);
            }
            gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            gl.EnableVertexAttribArray(2);


            gl.BindBuffer(BufferTargetARB.ArrayBuffer, ColorHandle);
            fixed (void* data = colors.ToArray())
            {
                gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(vec3) * colors.Count), data, BufferUsageARB.StaticDraw);
            }
            gl.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            gl.EnableVertexAttribArray(3);


            gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, IndexHandle);
            unsafe
            {
                fixed (uint* i = indices.ToArray())
                {
                    gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Count * sizeof(uint)), i, BufferUsageARB.StaticDraw);
                }
            }

            gl.BindVertexArray(0);
            gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
        }

        public void Render()
        {
            var gl = GameClient.GetInstance().GetGL();

            gl.BindVertexArray(Handle);

            gl.EnableVertexAttribArray(0);
            gl.EnableVertexAttribArray(1);
            gl.EnableVertexAttribArray(2);
            gl.EnableVertexAttribArray(3);
            
            unsafe
            {
                gl.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, (void*)0);
            }

            gl.DisableVertexAttribArray(3);
            gl.DisableVertexAttribArray(2);
            gl.DisableVertexAttribArray(1);
            gl.DisableVertexAttribArray(0);
        }

        public override void CleanUpMemory(GL gl)
        {
            gl.DeleteVertexArray(Handle);
            gl.DeleteBuffer(VertexHandle);
            gl.DeleteBuffer(UvHandle);
            gl.DeleteBuffer(ColorHandle);
            gl.DeleteBuffer(NormalHandle);
            gl.DeleteBuffer(IndexHandle);
        }

        public class PrepareResult
        {
            public List<vec3> verts = new();
            public List<vec3> colors = new();
            public List<vec2> uvs = new();
            public List<vec3> normals = new();
            public List<uint> indices = new();

            public PrepareResult()
            {

            }
        }

        public static PrepareResult? PrepareFromChunk(ClientWorld world, int cx, int cy, int cz)
        {
            var chunk = world.GetChunk(cx, cy, cz);
            if (chunk == null)
            {
                return null;
            }

            PrepareResult result = new();
            uint index = 0;

            var renderer = GameClient.GetInstance().TerrainRenderer;

            for (int x = 0; x < Chunk.size; x++)
            {
                for (int y = 0; y < Chunk.size; y++)
                {
                    for (int z = 0; z < Chunk.size; z++)
                    {
                        int wx = x + cx * Chunk.size;
                        int wy = y + cy * Chunk.size;
                        int wz = z + cz * Chunk.size;

                        var state = world.GetBlockState(wx, wy, wz);

                        var builder = AllBlockMeshBuilders.BlockMeshBuilders[state.GlobalID];
                        if (builder != null)
                        {
                            builder.Build(world, x, y, z, wx, wy, wz, result, state, ref index, renderer!);
                        }
                    }
                }
            }

            return result;

        }
    }
}
