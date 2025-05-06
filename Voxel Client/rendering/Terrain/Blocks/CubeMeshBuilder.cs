using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VoxelEngineClient.rendering.Terrain;
using VoxelEngineClient.world;
using VoxelEngineCore.Terrain.Blocks.Enums;
using VoxelEngineCore.Terrain.Blocks.States;

namespace VoxelClient.rendering.Terrain.Blocks
{
    public class CubeMeshBuilder : BlockMeshBuilder
    {
        public override void Build(ClientWorld world, int x, int y, int z, int wx, int wy, int wz, TerrainMesh.PrepareResult result, BlockState state, ref uint index, TerrainRenderer renderer)
        {
            if (state.Visible)
            {

                if (state.ShowAllFaces)
                {
                    AddFace(BlockSideEnum.Left, x, y, z, result, ref index, state, renderer!);
                    AddFace(BlockSideEnum.Right, x, y, z, result, ref index, state, renderer!);
                    AddFace(BlockSideEnum.Bottom, x, y, z, result, ref index, state, renderer!);
                    AddFace(BlockSideEnum.Top, x, y, z, result, ref index, state, renderer!);
                    AddFace(BlockSideEnum.Front, x, y, z, result, ref index, state, renderer!);
                    AddFace(BlockSideEnum.Back, x, y, z, result, ref index, state, renderer!);
                }
                else
                {
                    var test = world.GetBlockState(wx - 1, wy, wz);
                    if (!test.Visible || test.Transparent && !state.Transparent || state.Transparent && test != state)
                    {
                        AddFace(BlockSideEnum.Left, x, y, z, result, ref index, state, renderer!);
                    }

                    test = world.GetBlockState(wx + 1, wy, wz);
                    if (!test.Visible || test.Transparent && !state.Transparent || state.Transparent && test != state)
                    {
                        AddFace(BlockSideEnum.Right, x, y, z, result, ref index, state, renderer!);
                    }

                    test = world.GetBlockState(wx, wy - 1, wz);
                    if (!test.Visible || test.Transparent && !state.Transparent || state.Transparent && test != state)
                    {
                        AddFace(BlockSideEnum.Bottom, x, y, z, result, ref index, state, renderer!);
                    }

                    test = world.GetBlockState(wx, wy + 1, wz);
                    if (!test.Visible || test.Transparent && !state.Transparent || state.Transparent && test != state)
                    {
                        AddFace(BlockSideEnum.Top, x, y, z, result, ref index, state, renderer!);
                    }

                    test = world.GetBlockState(wx, wy, wz - 1);
                    if (!test.Visible || test.Transparent && !state.Transparent || state.Transparent && test != state)
                    {
                        AddFace(BlockSideEnum.Front, x, y, z, result, ref index, state, renderer!);
                    }

                    test = world.GetBlockState(wx, wy, wz + 1);
                    if (!test.Visible || test.Transparent && !state.Transparent || state.Transparent && test != state)
                    {
                        AddFace(BlockSideEnum.Back, x, y, z, result, ref index, state, renderer!);
                    }
                }
            }
        }
    }
}
