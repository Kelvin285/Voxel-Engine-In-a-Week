using GlmSharp;
using Silk.NET.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineClient.rendering
{
    public class Camera
    {
        public vec3 position;
        public float pitch;
        public float yaw;
        public float fov = 90;

        public mat4 GetProjection(float width, float height)
        {
            var mat = mat4.PerspectiveFov(glm.Radians(fov), width, height, 0.01f, 1024.0f);
            mat[0, 0] *= -1;
            return mat;
        }

        public quat GetYawQuat()
        {
            return quat.FromAxisAngle(glm.Radians(yaw), new(0, 1, 0));
        }

        public quat GetPitchQuat()
        {
            return quat.FromAxisAngle(glm.Radians(pitch), new(1, 0, 0));
        }

        public quat GetLookQuat()
        {
            return GetYawQuat() * GetPitchQuat();
        }

        public mat4 GetViewMat()
        {
            var look = GetLookQuat();
            return mat4.LookAt(position, position + look * vec3.UnitZ, look * vec3.UnitY);
        }

        public void ClampPitch()
        {
            pitch = glm.Clamp(pitch, -90, 90);
        }
    }
}
