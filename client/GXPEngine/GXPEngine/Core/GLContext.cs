using System;
using System.Collections.Generic;
using GXPEngine.OpenGL;

namespace GXPEngine.Core {
    internal class WindowSize {
        public static WindowSize instance = new WindowSize();
        public int width, height;
    }

    public class GLContext {
        private const int MAXKEYS = 65535;
        private const int MAXBUTTONS = 255;

        private static readonly bool[] keys = new bool[MAXKEYS + 1];
        private static readonly bool[] keydown = new bool[MAXKEYS + 1];
        private static readonly bool[] keyup = new bool[MAXKEYS + 1];
        private static readonly bool[] buttons = new bool[MAXBUTTONS + 1];
        private static readonly bool[] mousehits = new bool[MAXBUTTONS + 1];
        private static readonly bool[] mouseup = new bool[MAXBUTTONS + 1]; //mouseup kindly donated by LeonB

        public static int mouseX;
        public static int mouseY;

        private static double _realToLogicWidthRatio;
        private static double _realToLogicHeightRatio;

        private readonly Game _owner;
        private int _frameCount;
        private long _lastFPSTime;
        private long _lastFrameTime;

        private int _targetFrameRate = 60;
        private bool _vsyncEnabled;

        //------------------------------------------------------------------------------------------------------------------------
        //														RenderWindow()
        //------------------------------------------------------------------------------------------------------------------------
        public GLContext(Game owner) {
            _owner = owner;
            currentFps = _targetFrameRate;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Width
        //------------------------------------------------------------------------------------------------------------------------
        public int width => WindowSize.instance.width;

        //------------------------------------------------------------------------------------------------------------------------
        //														Height
        //------------------------------------------------------------------------------------------------------------------------
        public int height => WindowSize.instance.height;

        public int currentFps { get; private set; }

        public int targetFps {
            get => _targetFrameRate;
            set {
                if (value < 1)
                    _targetFrameRate = 1;
                else
                    _targetFrameRate = value;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														setupWindow()
        //------------------------------------------------------------------------------------------------------------------------
        public void CreateWindow(int width, int height, bool fullScreen, bool vSync, int realWidth, int realHeight, string title) {
            // This stores the "logical" width, used by all the game logic:
            WindowSize.instance.width = width;
            WindowSize.instance.height = height;
            _realToLogicWidthRatio = (double) realWidth / width;
            _realToLogicHeightRatio = (double) realHeight / height;
            _vsyncEnabled = vSync;

            GL.glfwInit();

            GL.glfwOpenWindowHint(GL.GLFW_FSAA_SAMPLES, 8);
            GL.glfwOpenWindow(realWidth, realHeight, 8, 8, 8, 8, 24, 0, fullScreen ? GL.GLFW_FULLSCREEN : GL.GLFW_WINDOWED);
            GL.glfwSetWindowTitle(title);
            GL.glfwSwapInterval(vSync);

            GL.glfwSetKeyCallback(
                (_key, _mode) => {
                    var press = _mode == 1;
                    if (press) keydown[_key] = true;
                    else keyup[_key] = true;
                    keys[_key] = press;
                });

            GL.glfwSetMouseButtonCallback(
                (_button, _mode) => {
                    var press = _mode == 1;
                    if (press) mousehits[_button] = true;
                    else mouseup[_button] = true;
                    buttons[_button] = press;
                });

            GL.glfwSetWindowSizeCallback((newWidth, newHeight) => {
                GL.Viewport(0, 0, newWidth, newHeight);
                GL.Enable(GL.MULTISAMPLE);
                GL.Enable(GL.TEXTURE_2D);
                GL.Enable(GL.BLEND);
                GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
                GL.Hint(GL.PERSPECTIVE_CORRECTION, GL.FASTEST);

                //GL.Enable (GL.POLYGON_SMOOTH);
                GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

                // Load the basic projection settings:
                GL.MatrixMode(GL.PROJECTION);
                GL.LoadIdentity();

                // Here's where the conversion from logical width/height to real width/height happens: 
                GL.Ortho(0.0f, newWidth / _realToLogicWidthRatio, newHeight / _realToLogicHeightRatio, 0.0f, 0.0f, 1000.0f);

                lock (WindowSize.instance) {
                    WindowSize.instance.width = (int) (newWidth / _realToLogicWidthRatio);
                    WindowSize.instance.height = (int) (newHeight / _realToLogicHeightRatio);
                }

                if (Game.main != null) Game.main.RenderRange = new Rectangle(0, 0, WindowSize.instance.width, WindowSize.instance.height);
            });
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														ShowCursor()
        //------------------------------------------------------------------------------------------------------------------------
        public void ShowCursor(bool enable) {
            if (enable)
                GL.glfwEnable(GL.GLFW_MOUSE_CURSOR);
            else
                GL.glfwDisable(GL.GLFW_MOUSE_CURSOR);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														SetScissor()
        //------------------------------------------------------------------------------------------------------------------------
        public void SetScissor(int x, int y, int width, int height) {
            if (width == WindowSize.instance.width && height == WindowSize.instance.height)
                GL.Disable(GL.SCISSOR_TEST);
            else
                GL.Enable(GL.SCISSOR_TEST);

            GL.Scissor(
                (int) (x * _realToLogicWidthRatio),
                (int) (y * _realToLogicHeightRatio),
                (int) (width * _realToLogicWidthRatio),
                (int) (height * _realToLogicHeightRatio)
            );

            //GL.Scissor(x, y, width, height);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Close()
        //------------------------------------------------------------------------------------------------------------------------
        public void Close() {
            GL.glfwCloseWindow();
            GL.glfwTerminate();
            Environment.Exit(0);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Run()
        //------------------------------------------------------------------------------------------------------------------------
        public void Run() {
            //Update();
            GL.glfwSetTime(0.0);
            do {
                if (_vsyncEnabled || Time.time - _lastFrameTime > 1000 / _targetFrameRate) {
                    _lastFrameTime = Time.time;

                    //actual fps count tracker
                    _frameCount++;
                    if (Time.time - _lastFPSTime > 1000) {
                        currentFps = (int) (_frameCount / ((Time.time - _lastFPSTime) / 1000.0f));
                        _lastFPSTime = Time.time;
                        _frameCount = 0;
                    }

                    UpdateMouseInput();
                    _owner.Step();

                    ResetHitCounters();
                    Display();

                    Time.newFrame();
                    GL.glfwPollEvents();
                }
            } while (GL.glfwGetWindowParam(GL.GLFW_ACTIVE) == 1);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														display()
        //------------------------------------------------------------------------------------------------------------------------
        private void Display() {
            GL.Clear(GL.COLOR_BUFFER_BIT);

            GL.MatrixMode(GL.MODELVIEW);
            GL.LoadIdentity();

            _owner.Render(this);

            GL.glfwSwapBuffers();
            if (GetKey(Key.ESCAPE)) Close();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														SetColor()
        //------------------------------------------------------------------------------------------------------------------------
        public void SetColor(byte r, byte g, byte b, byte a) {
            GL.Color4ub(r, g, b, a);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														PushMatrix()
        //------------------------------------------------------------------------------------------------------------------------
        public void PushMatrix(float[] matrix) {
            GL.PushMatrix();
            GL.MultMatrixf(matrix);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														PopMatrix()
        //------------------------------------------------------------------------------------------------------------------------
        public void PopMatrix() {
            GL.PopMatrix();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														DrawQuad()
        //------------------------------------------------------------------------------------------------------------------------
        public void DrawQuad(float[] vertices, float[] uv) {
            GL.EnableClientState(GL.TEXTURE_COORD_ARRAY);
            GL.EnableClientState(GL.VERTEX_ARRAY);
            GL.TexCoordPointer(2, GL.FLOAT, 0, uv);
            GL.VertexPointer(2, GL.FLOAT, 0, vertices);
            GL.DrawArrays(GL.QUADS, 0, 4);
            GL.DisableClientState(GL.VERTEX_ARRAY);
            GL.DisableClientState(GL.TEXTURE_COORD_ARRAY);
        }

        /// <summary>
        ///     Draws triangles using 2D coordinates for vertices
        /// </summary>
        public void DrawTriangles2D(float[] vertices, int[] indices, float[] uvs) {
            GL.EnableClientState(GL.TEXTURE_COORD_ARRAY);
            GL.EnableClientState(GL.VERTEX_ARRAY);
            GL.TexCoordPointer(2, GL.FLOAT, 0, uvs);
            GL.VertexPointer(2, GL.FLOAT, 0, vertices);
            GL.DrawElements(GL.TRIANGLES, indices.Length, GL.UNSIGNED_INT, indices);
            GL.DisableClientState(GL.VERTEX_ARRAY);
            GL.DisableClientState(GL.TEXTURE_COORD_ARRAY);
        }

        /// <summary>
        ///     Draws triangles using 3D coordinates for vertices
        /// </summary>
        public void DrawTriangles(float[] vertices, int[] indices, float[] uvs) {
            GL.EnableClientState(GL.TEXTURE_COORD_ARRAY);
            GL.EnableClientState(GL.VERTEX_ARRAY);
            GL.TexCoordPointer(2, GL.FLOAT, 0, uvs);
            GL.VertexPointer(3, GL.FLOAT, 0, vertices);
            GL.DrawElements(GL.TRIANGLES, indices.Length, GL.UNSIGNED_INT, indices);
            GL.DisableClientState(GL.VERTEX_ARRAY);
            GL.DisableClientState(GL.TEXTURE_COORD_ARRAY);
        }

        public void DrawMesh(Mesh mesh) {
            var vertices = new List<float>();
            var uvs = new List<float>();
            foreach (var vertex in mesh.Vertices) {
                vertices.Add(vertex.x);
                vertices.Add(vertex.y);
                vertices.Add(vertex.z);
            }

            foreach (var uv in mesh.Uvs) {
                uvs.Add(uv.x);
                uvs.Add(uv.y);
            }

            mesh.Texture.Bind();
            DrawTriangles(vertices.ToArray(), mesh.IndexArray, uvs.ToArray());
            mesh.Texture.Unbind();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetKey()
        //------------------------------------------------------------------------------------------------------------------------
        public static bool GetKey(int key) {
            return keys[key];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetKeyDown()
        //------------------------------------------------------------------------------------------------------------------------
        public static bool GetKeyDown(int key) {
            return keydown[key];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetKeyUp()
        //------------------------------------------------------------------------------------------------------------------------
        public static bool GetKeyUp(int key) {
            return keyup[key];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetMouseButton()
        //------------------------------------------------------------------------------------------------------------------------
        public static bool GetMouseButton(int button) {
            return buttons[button];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetMouseButtonDown()
        //------------------------------------------------------------------------------------------------------------------------
        public static bool GetMouseButtonDown(int button) {
            return mousehits[button];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetMouseButtonUp()
        //------------------------------------------------------------------------------------------------------------------------
        public static bool GetMouseButtonUp(int button) {
            return mouseup[button];
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														ResetHitCounters()
        //------------------------------------------------------------------------------------------------------------------------
        public static void ResetHitCounters() {
            Array.Clear(keydown, 0, MAXKEYS);
            Array.Clear(keyup, 0, MAXKEYS);
            Array.Clear(mousehits, 0, MAXBUTTONS);
            Array.Clear(mouseup, 0, MAXBUTTONS);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														UpdateMouseInput()
        //------------------------------------------------------------------------------------------------------------------------
        public static void UpdateMouseInput() {
            GL.glfwGetMousePos(out mouseX, out mouseY);
            mouseX = (int) (mouseX / _realToLogicWidthRatio);
            mouseY = (int) (mouseY / _realToLogicHeightRatio);
        }
    }
}