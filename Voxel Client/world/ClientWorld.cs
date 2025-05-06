using GlmSharp;
using VoxelEngineCore.Terrain;
using VoxelEngineCore.Terrain.Chunks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineClient.world
{
    public class ClientWorld : World
    {
        public ClientChunk[,,] chunks = new ClientChunk[16, 8, 16];
        public override Chunk? GetChunk(int x, int y, int z)
        {
            return chunks[x & 15, y & 7, z & 15];
        }

        public ClientWorld() : base()
        {
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        chunks[x, y, z] = new ClientChunk(this);
                        chunks[x, y, z].x = int.MaxValue;
                        chunks[x, y, z].y = int.MaxValue;
                        chunks[x, y, z].z = int.MaxValue;
                    }
                }
            }
        }

        public override void Update()
        {
            base.Update();

            var camera = GameClient.GetInstance().camera;

            int cx = (int)glm.Floor(camera.position.x / Chunk.size);
            int cy = (int)glm.Floor(camera.position.y / Chunk.size);
            int cz = (int)glm.Floor(camera.position.z / Chunk.size);

            int view_y = 4;
            int view_x = 8;
            int remade = 0;
            for (int x = -view_x; x < view_x; x++)
            {
                for (int y = -view_y; y < view_y; y++)
                {
                    for (int z = -view_x; z < view_x; z++)
                    {
                        int wx = cx + x;
                        int wy = cy + y;
                        int wz = cz + z;

                        int lx = wx & 15;
                        int ly = wy & 7;
                        int lz = wz & 15;

                        var chunk = chunks[lx, ly, lz];

                        if (chunk.x != wx || chunk.y != wy || chunk.z != wz)
                        {
                            chunk.Dispose();
                            chunk = new ClientChunk(this);
                            chunk.x = wx;
                            chunk.y = wy;
                            chunk.z = wz;
                            chunks[lx, ly, lz] = chunk;
                            remade++;
                        }

                        if (chunk.Generated == false)
                        {
                            if (chunk.GeneratingTask == null)
                            {
                                chunk.GeneratingTask = new ThreadPoolEntry((index, output) =>
                                {
                                    var watch = Stopwatch.StartNew();
                                    generator.GenerateChunk((ClientChunk)output[0]!);
                                    Console.WriteLine("Gen time: " + watch.ElapsedMilliseconds);
                                }, 1);

                                ClientChunk clone = new ClientChunk(this);
                                clone.x = wx;
                                clone.y = wy;
                                clone.z = wz;

                                chunk.GeneratingTask.output[0] = clone;

                                QueueAction(chunk.GeneratingTask);

                                
                            } else
                            {
                                if (chunk.GeneratingTask.done)
                                {
                                    chunks[lx, ly, lz] = (ClientChunk)chunk.GeneratingTask.output[0]!;

                                    for (int i = -1; i <= 1; i++)
                                    {
                                        for (int j = -1; j <= 1; j++)
                                        {
                                            for (int k = -1; k <= 1; k++)
                                            {
                                                ((ClientChunk)GetChunk(wx + i, wy + j, wz + k)!).NeedsToRebuild = true;
                                            }
                                        }
                                    }
                                }
                            }
                            
                        } else
                        {
                            chunk.Update();
                        }
                    }
                }
            }
            if (remade > 0)
            {
                Console.WriteLine(remade);
            }
        }

        public override void CloseWorld()
        {
            base.CloseWorld();
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        chunks[x, y, z].Dispose();
                    }
                }
            }
        }
    }
}
