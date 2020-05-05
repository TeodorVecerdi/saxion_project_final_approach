using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using GXPEngine.OpenGL;

namespace GXPEngine.Core {
    public class Texture2D {
        private const int UNDEFINED_GLTEXTURE = 0;
        private static readonly Hashtable LoadCache = new Hashtable();
        private static Texture2D lastBound;

        private int[] _glTexture;
        private int count;
        private bool stayInCache;

        //------------------------------------------------------------------------------------------------------------------------
        //														Texture2D()
        //------------------------------------------------------------------------------------------------------------------------
        public Texture2D(int width, int height) {
            if (width == 0)
                if (height == 0)
                    return;
            SetBitmap(new Bitmap(width, height));
        }

        public Texture2D(string filename) {
            Load(filename);
        }

        public Texture2D(Bitmap bitmap) {
            SetBitmap(bitmap);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														bitmap
        //------------------------------------------------------------------------------------------------------------------------
        public Bitmap bitmap { get; private set; }

        //------------------------------------------------------------------------------------------------------------------------
        //														filename
        //------------------------------------------------------------------------------------------------------------------------
        public string filename { get; private set; } = "";

        //------------------------------------------------------------------------------------------------------------------------
        //														width
        //------------------------------------------------------------------------------------------------------------------------
        public int width => bitmap.Width;

        //------------------------------------------------------------------------------------------------------------------------
        //														height
        //------------------------------------------------------------------------------------------------------------------------
        public int height => bitmap.Height;

        public bool wrap {
            set {
                GL.BindTexture(GL.TEXTURE_2D, _glTexture[0]);
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, value ? GL.GL_REPEAT : GL.GL_CLAMP_TO_EDGE_EXT);
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, value ? GL.GL_REPEAT : GL.GL_CLAMP_TO_EDGE_EXT);
                GL.BindTexture(GL.TEXTURE_2D, 0);
                lastBound = null;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetInstance()
        //------------------------------------------------------------------------------------------------------------------------
        public static Texture2D GetInstance(string filename, bool keepInCache = false) {
            var tex2d = LoadCache[filename] as Texture2D;
            if (tex2d == null) {
                tex2d = new Texture2D(filename);
                LoadCache[filename] = tex2d;
            }

            tex2d.stayInCache |= keepInCache; // setting it once to true keeps it in cache
            tex2d.count++;
            return tex2d;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RemoveInstance()
        //------------------------------------------------------------------------------------------------------------------------
        public static void RemoveInstance(string filename) {
            if (LoadCache.ContainsKey(filename)) {
                var tex2D = LoadCache[filename] as Texture2D;
                tex2D.count--;
                if (tex2D.count == 0 && !tex2D.stayInCache) LoadCache.Remove(filename);
            }
        }

        public void Dispose() {
            if (filename != "") RemoveInstance(filename);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Bind()
        //------------------------------------------------------------------------------------------------------------------------
        public void Bind() {
            if (lastBound == this) return;
            lastBound = this;
            GL.BindTexture(GL.TEXTURE_2D, _glTexture[0]);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Unbind()
        //------------------------------------------------------------------------------------------------------------------------
        public void Unbind() {
//			GL.BindTexture (GL.TEXTURE_2D, 0);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Load()
        //------------------------------------------------------------------------------------------------------------------------
        private void Load(string filename) {
            this.filename = filename;
            Bitmap bitmap;
            try {
                bitmap = new Bitmap(filename);
            } catch {
                throw new Exception("Image " + filename + " cannot be found.");
            }

            SetBitmap(bitmap);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														SetBitmap()
        //------------------------------------------------------------------------------------------------------------------------
//        private void SetBitmap(Bitmap bitmap) {
        public void SetBitmap(Bitmap bitmap) {
            this.bitmap = bitmap;
            CreateGLTexture();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														CreateGLTexture()
        //------------------------------------------------------------------------------------------------------------------------
        private void CreateGLTexture() {
            if (_glTexture != null)
                if (_glTexture.Length > 0)
                    if (_glTexture[0] != UNDEFINED_GLTEXTURE)
                        destroyGLTexture();

            _glTexture = new int[1];
            if (bitmap == null)
                bitmap = new Bitmap(64, 64);

            GL.GenTextures(1, _glTexture);

            GL.BindTexture(GL.TEXTURE_2D, _glTexture[0]);
            if (Game.main.PixelArt) {
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.NEAREST);
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.NEAREST);
            } else {
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.LINEAR);
                GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.LINEAR);
            }

            GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, GL.GL_CLAMP_TO_EDGE_EXT);
            GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, GL.GL_CLAMP_TO_EDGE_EXT);

            UpdateGLTexture();
            GL.BindTexture(GL.TEXTURE_2D, 0);
            lastBound = null;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														UpdateGLTexture()
        //------------------------------------------------------------------------------------------------------------------------
        public void UpdateGLTexture() {
            var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            GL.BindTexture(GL.TEXTURE_2D, _glTexture[0]);
            GL.TexImage2D(GL.TEXTURE_2D, 0, GL.RGBA, bitmap.Width, bitmap.Height, 0,
                GL.BGRA, GL.UNSIGNED_BYTE, data.Scan0);

            bitmap.UnlockBits(data);
            lastBound = null;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														destroyGLTexture()
        //------------------------------------------------------------------------------------------------------------------------
        private void destroyGLTexture() {
            GL.DeleteTextures(1, _glTexture);
            _glTexture[0] = UNDEFINED_GLTEXTURE;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Clone()
        //------------------------------------------------------------------------------------------------------------------------
        public Texture2D Clone(bool deepCopy = false) {
            Bitmap bitmap;
            if (deepCopy)
                bitmap = this.bitmap.Clone() as Bitmap;
            else
                bitmap = this.bitmap;
            var newTexture = new Texture2D(0, 0);
            newTexture.SetBitmap(bitmap);
            return newTexture;
        }

        public static string GetDiagnostics() {
            var output = "";
            output += "Number of textures in cache: " + LoadCache.Keys.Count + '\n';
            return output;
        }
    }
}