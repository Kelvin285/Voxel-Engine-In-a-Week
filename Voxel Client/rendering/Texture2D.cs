using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace VoxelEngineClient.rendering
{
    public class Texture2D : AutoDispose
    {
        public uint Handle { get; private set; }

        public int Width, Height;

        public Texture2D(string path)
        {
            var gl = GameClient.GetInstance().GetGL();

            Handle = gl.GenTexture();

            gl.BindTexture(GLEnum.Texture2D, Handle);

            using (Image<Rgba32> image = Image.Load<Rgba32>("path/to/image.png"))
            {
                Width = image.Width;
                Height = image.Height;

                Rgba32[] pixels = new Rgba32[Width * Height];

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        pixels[x + y * Width] = image[x, y];
                    }
                }

                unsafe
                {
                    fixed (void* data = pixels)
                    {
                        gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)Width, (uint)Height, 0, PixelFormat.Rgba, GLEnum.UnsignedByte, data);
                    }
                }

                gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            }
        }

        public Texture2D(int width, int height, Rgba32[] pixels)
        {
            Width = width;
            Height = height;

            var gl = GameClient.GetInstance().GetGL();

            Handle = gl.GenTexture();

            gl.BindTexture(GLEnum.Texture2D, Handle);

            unsafe
            {
                fixed (void* data = pixels)
                {
                    gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)Width, (uint)Height, 0, PixelFormat.Rgba, GLEnum.UnsignedByte, data);
                }
            }

            gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            gl.TexParameter(GLEnum.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }

        public void Bind(int location = 0)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.ActiveTexture(GLEnum.Texture0 + location);

            gl.BindTexture(GLEnum.Texture2D, Handle);
        }

        public override void CleanUpMemory(GL gl)
        {
            gl.DeleteTexture(Handle);
        }
    }
}
