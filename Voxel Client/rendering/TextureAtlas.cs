using RectpackSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineClient.rendering
{
    public class TextureAtlas
    {
        public Texture2D? texture;

        private struct Tex
        {
            public int Width;
            public int Height;
            public Rgba32[] pixels;
        }

        private List<Tex> textures = new();
        private List<string> added = new();


        public List<PackingRectangle> Packed = new();

        public int AddTexture(string tex)
        {
            if (added.Contains(tex))
            {
                return added.IndexOf(tex);
            }
            int i = added.Count;
            added.Add(tex);

            Tex t = new();

            using (Image<Rgba32> image = Image.Load<Rgba32>(tex))
            {
                t.Width = image.Width;
                t.Height = image.Height;

                Rgba32[] pixels = new Rgba32[t.Width * t.Height];

                for (int x = 0; x < t.Width; x++)
                {
                    for (int y = 0; y < t.Height; y++)
                    {
                        pixels[x + y * t.Width] = image[x, y];
                    }
                }
                t.pixels = pixels;
            }

            textures.Add(t);

            return i;
        }

        public void Build()
        {
            PackingRectangle[] packing = new PackingRectangle[textures.Count];

            for (int i = 0; i < textures.Count; i++)
            {
                var tex = textures[i];
                packing[i] = new(0, 0, (uint)tex.Width, (uint)tex.Height, i);
            }

            RectanglePacker.Pack(packing, out var bounds);

            Rgba32[] pixels = new Rgba32[bounds.Width * bounds.Height];

            for (int i = 0; i < textures.Count; i++)
            {
                Packed.Add(packing[i]);

                for (int x = (int)packing[i].X; x < packing[i].X + packing[i].Width; x++)
                {
                    for (int y = (int)packing[i].Y; y < packing[i].Y + packing[i].Height; y++)
                    {
                        pixels[x + y * bounds.Width] = textures[i].pixels[(x - (int)packing[i].X) + (y - (int)packing[i].Y) * textures[i].Width];
                    }
                }
            }

            texture = new Texture2D((int)bounds.Width, (int)bounds.Height, pixels);
        }


    }
}
