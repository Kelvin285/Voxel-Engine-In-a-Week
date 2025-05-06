using GlmSharp;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Silk.NET.Core.Native.WinString;

namespace VoxelEngineClient.rendering.shaders
{
    public class GraphicsShader : AutoDispose
    {
        public uint Handle { get; private set; }
        public GraphicsShader(string name)
        {
            string vert_file = "assets/shaders/" + name + ".vert";
            string frag_file = "assets/shaders/" + name + ".frag";

            var gl = GameClient.GetInstance().GetGL();
            Handle = gl.CreateProgram();

            var vert = gl.CreateShader(ShaderType.VertexShader);
            var frag = gl.CreateShader(ShaderType.FragmentShader);

            gl.UseProgram(Handle);

            gl.ShaderSource(vert, File.ReadAllText(vert_file));
            gl.CompileShader(vert);

            gl.GetShader(vert, ShaderParameterName.CompileStatus, out int status);

            if (status != (int)GLEnum.True)
            {
                string infoLog = gl.GetShaderInfoLog(vert);
                Console.WriteLine("Shader compilation failed for " + vert_file);
                Console.WriteLine(infoLog);
            }

            gl.ShaderSource(frag, File.ReadAllText(frag_file));
            gl.CompileShader(frag);

            gl.GetShader(frag, ShaderParameterName.CompileStatus, out status);

            if (status != (int)GLEnum.True)
            {
                string infoLog = gl.GetShaderInfoLog(frag);
                Console.WriteLine("Shader compilation failed for " + frag_file);
                Console.WriteLine(infoLog);
            }

            gl.AttachShader(Handle, vert);
            gl.AttachShader(Handle, frag);
            gl.LinkProgram(Handle);

            gl.GetProgram(Handle, ProgramPropertyARB.LinkStatus, out int linkStatus);
            if (linkStatus != (int)GLEnum.True)
            {
                string log = gl.GetProgramInfoLog(Handle);
                Console.WriteLine("Program link failed: " + name);
                Console.WriteLine(log);
                throw new Exception("Shader program linking failed");
            }

            gl.DeleteShader(vert);
            gl.DeleteShader(frag);

            foreach (var val in GetType().GetFields())
            {
                if (val.GetCustomAttribute<UniformLocationAttribute>() != null)
                {
                    val.SetValue(this, gl.GetUniformLocation(Handle, val.Name));
                }
            }
        }

        public void Use()
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.UseProgram(Handle);
        }

        public override void CleanUpMemory(GL gl)
        {
            gl.DeleteProgram(Handle);
        }

        public void SetInt(int location, int value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.Uniform1(location, value);
        }

        public void SetInt2(int location, ivec2 value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.Uniform2(location, value.x, value.y);
        }

        public void SetInt3(int location, ivec3 value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.Uniform3(location, value.x, value.y, value.z);
        }

        public void SetInt4(int location, ivec4 value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.Uniform4(location, value.x, value.y, value.z, value.w);
        }



        public void SetFloat(int location, float value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.Uniform1(location, value);
        }

        public void SetFloat2(int location, vec2 value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.Uniform2(location, value.x, value.y);
        }

        public void SetFloat3(int location, vec3 value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.Uniform3(location, value.x, value.y, value.z);
        }

        public void SetFloat4(int location, vec4 value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.Uniform4(location, value.x, value.y, value.z, value.w);
        }


        public void SetMat4(int location, mat4 value)
        {
            var gl = GameClient.GetInstance().GetGL();
            gl.UniformMatrix4(location, false, value.ToArray());
        }
    }
}
