using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineClient.rendering
{
    public abstract class AutoDispose
    {
        public static HashSet<AutoDispose> Disposables = new();

        public bool Disposed { get; private set; } = false;

        public AutoDispose()
        {
            Disposables.Add(this);
        }

        public abstract void CleanUpMemory(GL gl);

        public void Dispose()
        {
            var gl = GameClient.GetInstance().GetGL();
            if (!Disposed)
            {
                Disposed = true;
                CleanUpMemory(gl);
                Disposables.Remove(this);
            }
        }
    }
}
