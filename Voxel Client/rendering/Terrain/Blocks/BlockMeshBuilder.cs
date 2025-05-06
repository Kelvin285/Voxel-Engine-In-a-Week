using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelClient.rendering.Terrain.Blocks.Registry;
using VoxelEngineClient.rendering.Terrain;
using VoxelEngineClient.world;
using VoxelEngineCore.Terrain;
using VoxelEngineCore.Terrain.Blocks;
using VoxelEngineCore.Terrain.Blocks.Enums;
using VoxelEngineCore.Terrain.Blocks.States;
using static VoxelEngineClient.rendering.Terrain.TerrainMesh;

namespace VoxelClient.rendering.Terrain.Blocks
{
    public abstract class BlockMeshBuilder
    {
        public BlockMeshBuilder()
        {
            AllBlockMeshBuilders.BlockMeshBuilders.Add(this);
        }

        public void AddFaceCustom(BlockSideEnum side, vec3 pos, vec3 size, vec2 uv1, vec2 uv2, PrepareResult result, ref uint index, BlockState state, TerrainRenderer renderer)
        {
            result.indices.AddRange([index + 0, index + 1, index + 2, index + 2, index + 3, index + 0]);
            index += 4;
            var uv = renderer!.BlockTextureHolders[state.GlobalID];
            if (uv == null)
            {
                result.uvs.AddRange([new(-1), new(-1), new(-1), new(-1)]);
                var c = state.MapColor;
                result.colors.AddRange([
                    c, c, c, c
                ]);
            }
            else
            {
                var c = new vec3(1, 1, 1);
                result.colors.AddRange([
                    c, c, c, c
                    ]);
                var rect = uv[side];

                vec2 A = new vec2(rect.x1, rect.y1);
                vec2 B = new vec2(rect.x2, rect.y2);

                vec2 rectSize = B - A;
                vec2 newSize = (uv2 - uv1) * rectSize;

                B = A + newSize;
                A += uv1 * rectSize;

                result.uvs.AddRange([

                    new(rect.x1, rect.y2),
                    new(rect.x1, rect.y1),
                    new(rect.x2, rect.y1),
                    new(rect.x2, rect.y2)

                    ]);
            }

            var normal = side.GetNormal();
            result.normals.AddRange([normal, normal, normal, normal]);

            switch (side)
            {
                case BlockSideEnum.Left:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z + size.z),
                            new(pos.x, pos.y + size.y, pos.z + size.z),
                            new(pos.x, pos.y + size.y, pos.z),
                            new(pos.x, pos.y, pos.z)
                        ]);
                    break;
                case BlockSideEnum.Right:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z),
                            new(pos.x, pos.y + size.y, pos.z),
                            new(pos.x, pos.y + size.y, pos.z + size.z),
                            new(pos.x, pos.y, pos.z + size.z)
                        ]);
                    break;
                case BlockSideEnum.Front:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z),
                            new(pos.x, pos.y + size.y, pos.z),
                            new(pos.x + size.x, pos.y + size.y, pos.z),
                            new(pos.x + size.x, pos.y, pos.z)
                        ]);
                    break;
                case BlockSideEnum.Back:
                    result.verts.AddRange([
                            new(pos.x + size.x, pos.y, pos.z),
                            new(pos.x + size.x, pos.y + size.y, pos.z),
                            new(pos.x, pos.y + size.y, pos.z),
                            new(pos.x, pos.y, pos.z)
                        ]);
                    break;
                case BlockSideEnum.Bottom:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z + size.z),
                            new(pos.x, pos.y, pos.z),
                            new(pos.x + size.x, pos.y, pos.z),
                            new(pos.x + size.x, pos.y, pos.z + size.z)
                        ]);


                    break;
                case BlockSideEnum.Top:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z),
                            new(pos.x, pos.y, pos.z + size.z),
                            new(pos.x + size.x, pos.y, pos.z + size.z),
                            new(pos.x + size.x, pos.y, pos.z)
                        ]);
                    break;
            }
        }

        public void AddFaceCustomRotated(BlockSideEnum side, vec3 pos, vec3 size, vec2 uv1, vec2 uv2, vec3 origin, quat rotation, PrepareResult result, ref uint index, BlockState state, TerrainRenderer renderer)
        {
            result.indices.AddRange([index + 0, index + 1, index + 2, index + 2, index + 3, index + 0]);
            index += 4;
            var uv = renderer!.BlockTextureHolders[state.GlobalID];
            if (uv == null)
            {
                result.uvs.AddRange([new(-1), new(-1), new(-1), new(-1)]);
                var c = state.MapColor;
                result.colors.AddRange([
                    c, c, c, c
                ]);
            }
            else
            {
                var c = new vec3(1, 1, 1);
                result.colors.AddRange([
                    c, c, c, c
                    ]);
                var rect = uv[side];

                vec2 A = new vec2(rect.x1, rect.y1);
                vec2 B = new vec2(rect.x2, rect.y2);

                vec2 rectSize = B - A;
                vec2 newSize = (uv2 - uv1) * rectSize;

                B = A + newSize;
                A += uv1 * rectSize;

                result.uvs.AddRange([

                    new(rect.x1, rect.y2),
                    new(rect.x1, rect.y1),
                    new(rect.x2, rect.y1),
                    new(rect.x2, rect.y2)

                    ]);
            }

            var normal = side.GetNormal();
            result.normals.AddRange([normal, normal, normal, normal]);

            int v = result.verts.Count;
            switch (side)
            {
                case BlockSideEnum.Left:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z + size.z),
                            new(pos.x, pos.y + size.y, pos.z + size.z),
                            new(pos.x, pos.y + size.y, pos.z),
                            new(pos.x, pos.y, pos.z)
                        ]);
                    break;
                case BlockSideEnum.Right:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z),
                            new(pos.x, pos.y + size.y, pos.z),
                            new(pos.x, pos.y + size.y, pos.z + size.z),
                            new(pos.x, pos.y, pos.z + size.z)
                        ]);
                    break;
                case BlockSideEnum.Front:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z),
                            new(pos.x, pos.y + size.y, pos.z),
                            new(pos.x + size.x, pos.y + size.y, pos.z),
                            new(pos.x + size.x, pos.y, pos.z)
                        ]);
                    break;
                case BlockSideEnum.Back:
                    result.verts.AddRange([
                            new(pos.x + size.x, pos.y, pos.z),
                            new(pos.x + size.x, pos.y + size.y, pos.z),
                            new(pos.x, pos.y + size.y, pos.z),
                            new(pos.x, pos.y, pos.z)
                        ]);
                    break;
                case BlockSideEnum.Bottom:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z + size.z),
                            new(pos.x, pos.y, pos.z),
                            new(pos.x + size.x, pos.y, pos.z),
                            new(pos.x + size.x, pos.y, pos.z + size.z)
                        ]);


                    break;
                case BlockSideEnum.Top:
                    result.verts.AddRange([
                            new(pos.x, pos.y, pos.z),
                            new(pos.x, pos.y, pos.z + size.z),
                            new(pos.x + size.x, pos.y, pos.z + size.z),
                            new(pos.x + size.x, pos.y, pos.z)
                        ]);
                    break;
            }
            for (int i = v; i < v + 4; i++)
            {
                result.verts[i] -= origin;
                result.verts[i] = rotation * result.verts[i];
                result.verts[i] += origin;
                result.normals[i] = rotation * result.normals[i];
            }
        }

        public void AddFace(BlockSideEnum side, int x, int y, int z, PrepareResult result, ref uint index, BlockState state, TerrainRenderer renderer)
        {
            result.indices.AddRange([index + 0, index + 1, index + 2, index + 2, index + 3, index + 0]);
            index += 4;
            var uv = renderer!.BlockTextureHolders[state.GlobalID];
            if (uv == null)
            {
                result.uvs.AddRange([new(-1), new(-1), new(-1), new(-1)]);
                var c = state.MapColor;
                result.colors.AddRange([
                    c, c, c, c
                ]);
            }
            else
            {
                var c = new vec3(1, 1, 1);
                result.colors.AddRange([
                    c, c, c, c
                    ]);
                var rect = uv[side];
                result.uvs.AddRange([

                    new(rect.x1, rect.y2),
                                        new(rect.x1, rect.y1),
                                        new(rect.x2, rect.y1),
                                        new(rect.x2, rect.y2)

                    ]);
            }

            var normal = side.GetNormal();
            result.normals.AddRange([normal, normal, normal, normal]);

            switch (side)
            {
                case BlockSideEnum.Left:
                    result.verts.AddRange([
                        new(x, y, z + 1),
                                            new(x, y + 1, z + 1),
                                            new(x, y + 1, z),
                                            new(x, y, z)
                        ]);
                    break;
                case BlockSideEnum.Right:
                    result.verts.AddRange([
                        new(x + 1, y, z),
                                            new(x + 1, y + 1, z),
                                            new(x + 1, y + 1, z + 1),
                                            new(x + 1, y, z + 1)
                        ]);
                    break;
                case BlockSideEnum.Front:
                    result.verts.AddRange([
                        new(x, y, z),
                                            new(x, y + 1, z),
                                            new(x + 1, y + 1, z),
                                            new(x + 1, y, z)
                        ]);
                    break;
                case BlockSideEnum.Back:
                    result.verts.AddRange([
                        new(x + 1, y, z + 1),
                                            new(x + 1, y + 1, z + 1),
                                            new(x, y + 1, z + 1),
                                            new(x, y, z + 1)
                        ]);
                    break;
                case BlockSideEnum.Bottom:
                    result.verts.AddRange([
                        new(x, y, z + 1),
                                            new(x, y, z),
                                            new(x + 1, y, z),
                                            new(x + 1, y, z + 1)
                        ]);


                    break;
                case BlockSideEnum.Top:
                    result.verts.AddRange([
                        new(x, y + 1, z),
                                            new(x, y + 1, z + 1),
                                            new(x + 1, y + 1, z + 1),
                                            new(x + 1, y + 1, z)
                        ]);
                    break;
            }
        }

        public abstract void Build(ClientWorld world, int x, int y, int z, int wx, int wy, int wz, TerrainMesh.PrepareResult result, BlockState state, ref uint index, TerrainRenderer renderer);
    }
}
