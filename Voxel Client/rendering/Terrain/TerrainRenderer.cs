using GlmSharp;
using VoxelEngineCore.Terrain.Blocks.Enums;
using VoxelEngineCore.Terrain.Blocks.Registry;
using VoxelEngineCore.Terrain.Blocks.States.TextureHolders;
using VoxelEngineClient.rendering.shaders;
using VoxelEngineClient.world;
using RectpackSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VoxelClient.rendering.Terrain.Blocks.Registry;

namespace VoxelEngineClient.rendering.Terrain
{
    public class TerrainRenderer
    {
        public TextureAtlas BlockTextureAtlas = new TextureAtlas();

        public List<BlockTextureHolder?> BlockTextureHolders = new();

        public TerrainShader TerrainShader;

        public vec3 SunDir = new vec3(1, 1, 1).NormalizedSafe;

        public TerrainRenderer()
        {
            AllBlockMeshBuilders.BlockMeshBuilders.Clear();
            AllBlockMeshBuilders.Register();
            TerrainShader = new();
            TerrainShader.Use();
            TerrainShader.SetFloat3(TerrainShader.sun_dir, SunDir);

            List<int[]> tempTextures = new();
            foreach (var state in AllBlocks.States)
            {
                if (state.TextureHolder == null)
                {
                    tempTextures.Add(Array.Empty<int>());
                    continue;
                }

                TextureHolderBase holder = state.TextureHolder;

                int[] entries = new int[6];
                for (int i = 0; i < 6; i++)
                {
                    var tex = holder.GetTextureForSide((BlockSideEnum)i);

                    string file = "assets/textures/blocks/" + tex + ".png";
                    entries[i] = BlockTextureAtlas.AddTexture(file);
                }
                tempTextures.Add(entries);
            }

            BlockTextureAtlas.Build();

            if (BlockTextureAtlas.texture == null)
            {
                Console.WriteLine("Error! Block texture atlas was null!");
                return;
            }

            var width = 1.0f / BlockTextureAtlas.texture.Width;
            var height = 1.0f / BlockTextureAtlas.texture.Height;

            for (int i = 0; i < tempTextures.Count; i++)
            {
                var temp = tempTextures[i];
                if (temp.Length == 0)
                {
                    BlockTextureHolders.Add(null);
                    continue;
                }

                BlockTextureHolder.UV GetUV(int r)
                {
                    var rect = BlockTextureAtlas.Packed[r];
                    return new() {
                        x1 = width * rect.X,
                        y1 = height * rect.Y,
                        x2 = width * (rect.X + rect.Width),
                        y2 = height * (rect.Y + rect.Height)
                    };


                }

                BlockTextureHolder holder = new();
                holder.Top = GetUV(temp[0]);
                holder.Bottom = GetUV(temp[1]);
                holder.Left = GetUV(temp[2]);
                holder.Right = GetUV(temp[3]);
                holder.Front = GetUV(temp[4]);
                holder.Back = GetUV(temp[5]);
                BlockTextureHolders.Add(holder);
            }

        }

        public void Render(float delta, ClientWorld world)
        {
            var game = GameClient.GetInstance();
            var window = game.GetWindow();
            var camera = game.camera;

            var proj = camera.GetProjection(window.Size.X, window.Size.Y);
            var view = camera.GetViewMat();

            var proj_view = proj * view;

            TerrainShader.Use();
            TerrainShader.SetMat4(TerrainShader.proj_view_model, proj_view);
            BlockTextureAtlas.texture!.Bind();

            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        world.chunks[x, y, z].Render(this, proj_view);
                    }
                }
            }
        }
    }
}
