using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngineClient.rendering.Terrain;
using VoxelEngineClient.world;
using VoxelEngineCore.Terrain.Blocks.States;

namespace VoxelClient.rendering.Terrain.Blocks
{
    public class CrossMeshBuilder : BlockMeshBuilder
    {
        public readonly quat rotation = quat.FromAxisAngle(glm.Radians(45.0f), new vec3(0, 1, 0));
        public override void Build(ClientWorld world, int x, int y, int z, int wx, int wy, int wz, TerrainMesh.PrepareResult result, BlockState state, ref uint index, TerrainRenderer renderer)
        {
            vec3 origin = new vec3(x, y, z) + 0.5f;
            AddFaceCustomRotated(VoxelEngineCore.Terrain.Blocks.Enums.BlockSideEnum.Left, new vec3(x + 0.5f, y, z), new vec3(1.0f), new vec2(0), new vec2(1), origin, rotation, result, ref index, state, renderer);
            AddFaceCustomRotated(VoxelEngineCore.Terrain.Blocks.Enums.BlockSideEnum.Right, new vec3(x + 0.5f, y, z), new vec3(1.0f), new vec2(0), new vec2(1), origin, rotation, result, ref index, state, renderer);

            AddFaceCustomRotated(VoxelEngineCore.Terrain.Blocks.Enums.BlockSideEnum.Front, new vec3(x, y, z + 0.5f), new vec3(1.0f), new vec2(0), new vec2(1), origin, rotation, result, ref index, state, renderer);
            AddFaceCustomRotated(VoxelEngineCore.Terrain.Blocks.Enums.BlockSideEnum.Back, new vec3(x, y, z + 0.5f), new vec3(1.0f), new vec2(0), new vec2(1), origin, rotation, result, ref index, state, renderer);
        }
    }
}
