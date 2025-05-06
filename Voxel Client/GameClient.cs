using GlmSharp;
using VoxelEngineClient.rendering;
using VoxelEngineClient.rendering.Terrain;
using VoxelEngineClient.world;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineClient
{
    public class GameClient
    {
        private static GameClient? instance;

        private IWindow window;
        private GL? gl;
        private Glfw? glfw;

        public static GameClient GetInstance()
        {
            return instance!;
        }

        public IWindow GetWindow()
        {
            return window;
        }

        public GL GetGL()
        {
            return gl!;
        }

        public Glfw GetGlfw()
        {
            return glfw!;
        }

        public IInputContext GetInput()
        {
            return inputContext!;
        }


        public TerrainRenderer? TerrainRenderer;

        public ClientWorld world;

        public Camera camera = new();

        private IInputContext? inputContext;
        private ImGuiController? imGuiController;

        public vec2 mouse_pos;
        public vec2 mouse_delta;

        public bool MouseCaptured { get; private set; }

        public GameClient()
        {
            instance = this;

            var options = WindowOptions.Default;
            options.Size = new(800, 600);
            options.Title = "Voxel Engine in a Week";

            window = Window.Create(options);

            window.Load += OnLoad;
            window.Render += OnRender;
            window.Closing += OnClose;

            world = new();

            window.Run();
        }

        public void LockCamera()
        {

        }

        public void OnLoad()
        {
            gl = GL.GetApi(window);
            glfw = Glfw.GetApi();
            inputContext = window.CreateInput();
            imGuiController = new(gl, window, inputContext);

            TerrainRenderer = new();
        }


        public const float FixedDelta = 1.0f / 20.0f;
        private double accum = 0;
        private void OnRender(double delta)
        {
            if (delta > 1)
            {
                delta = 1;
            }
            accum += delta;

            vec2 current_mouse = new vec2(inputContext!.Mice[0].Position.X, inputContext!.Mice[0].Position.Y);

            mouse_delta = current_mouse - mouse_pos;

            mouse_pos = current_mouse;

            while (accum > FixedDelta)
            {
                FixedUpdate();
                accum -= FixedDelta;
            }

            Update((float)delta);

            RenderImGUI((float)delta);
            Render((float)delta);

            if (imGuiController != null)
            {
                imGuiController.Render();
            }
        }

        public void UpdateCam()
        {
            if (inputContext == null)
            {
                return;
            }

            if (MouseCaptured)
            {
                float lookSpeed = 0.5f;
                camera.pitch += mouse_delta.y * lookSpeed;
                camera.yaw += mouse_delta.x * lookSpeed;

                float speed = 0.5f / 8.0f;
                if (inputContext.Keyboards[0].IsKeyPressed(Key.W))
                {
                    camera.position += camera.GetLookQuat() * new vec3(0, 0, 1) * speed;
                }
                if (inputContext.Keyboards[0].IsKeyPressed(Key.S))
                {
                    camera.position -= camera.GetLookQuat() * new vec3(0, 0, 1) * speed;
                }
                if (inputContext.Keyboards[0].IsKeyPressed(Key.A))
                {
                    camera.position -= camera.GetLookQuat() * new vec3(1, 0, 0) * speed;
                }
                if (inputContext.Keyboards[0].IsKeyPressed(Key.D))
                {
                    camera.position += camera.GetLookQuat() * new vec3(1, 0, 0) * speed;
                }

            }
            camera.ClampPitch();
        }

        public void Update(float delta)
        {
            if (inputContext == null)
            {
                return;
            }

            if (inputContext.Mice[0].IsButtonPressed(Silk.NET.Input.MouseButton.Left))
            {
                CaptureMouse();
            }
            if (inputContext.Keyboards[0].IsKeyPressed(Key.Escape))
            {
                ReleaseMouse();
            }

            UpdateCam();
            world.Update();
        }

        public void FixedUpdate()
        {
            
        }

        public void Render(float delta)
        {
            if (gl == null)
            {
                return;
            }

            gl.Viewport(0, 0, (uint)window.Size.X, (uint)window.Size.Y);
            gl.ClearColor(0, 0, 0, 1);

            gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
            gl.Enable(EnableCap.DepthTest);

            TerrainRenderer!.Render(delta, world);
        }

        public void RenderImGUI(float delta)
        {
            if (imGuiController == null)
            {
                return;
            }

            imGuiController.Update(delta);
        }

        public void OnClose()
        {
            
            if (gl == null)
            {
                return;
            }
            foreach (var d in AutoDispose.Disposables)
            {
                d.CleanUpMemory(gl);
            }
            AutoDispose.Disposables.Clear();
            world.CloseWorld();
        }

        public void CaptureMouse()
        {
            unsafe
            {
                MouseCaptured = true;
                glfw!.SetInputMode((WindowHandle*)window.Handle.ToPointer(), CursorStateAttribute.Cursor, CursorModeValue.CursorDisabled);
            }
        }

        public void ReleaseMouse()
        {
            unsafe
            {
                MouseCaptured = false;
                glfw!.SetInputMode((WindowHandle*)window.Handle.ToPointer(), CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
            }
        }
    }
}
