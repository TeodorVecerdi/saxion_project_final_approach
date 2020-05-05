using System.Collections.Generic;
using GXPEngine.Core;

namespace GXPEngine {
    /// <summary>
    ///     Representation of a textured mesh
    /// </summary>
    public class Mesh {
        private List<int> indices;

        private List<Vector2> uvs;

        private List<Vector3> vertices;

        public Mesh(Texture2D texture) : this(texture, new List<Vector3>(), new List<Vector2>(), new List<int>()) { }

        public Mesh(string texturePath) : this(Texture2D.GetInstance(texturePath, true), new List<Vector3>(), new List<Vector2>(),
            new List<int>()) { }

        public Mesh(Texture2D texture, Vector3[] vertices, Vector2[] uvs, int[] indices) : this(texture, new List<Vector3>(vertices),
            new List<Vector2>(uvs), new List<int>(indices)) { }

        public Mesh(string texturePath, Vector3[] vertices, Vector2[] uvs, int[] indices) : this(Texture2D.GetInstance(texturePath, true),
            new List<Vector3>(vertices), new List<Vector2>(uvs), new List<int>(indices)) { }

        public Mesh(Texture2D texture, List<Vector3> vertices, List<Vector2> uvs, List<int> indices) {
            Texture = texture;
            Vertices = vertices;
            Uvs = uvs;
            Indices = indices;
        }

        /// <summary>
        ///     The mesh vertices
        /// </summary>
        public List<Vector3> Vertices {
            get => vertices;
            set {
                vertices = new List<Vector3>(value);
                VertexArray = value.ToArray();
            }
        }

        /// <summary>
        ///     The mesh UVs
        /// </summary>
        public List<Vector2> Uvs {
            get => uvs;
            set {
                uvs = new List<Vector2>(value);
                UvArray = value.ToArray();
            }
        }

        /// <summary>
        ///     The mesh indices/triangles
        /// </summary>
        public List<int> Indices {
            get => indices;
            set {
                indices = new List<int>(value);
                IndexArray = value.ToArray();
            }
        }

        /// <summary>
        ///     Get the mesh's vertex array
        /// </summary>
        public Vector3[] VertexArray { get; private set; }

        /// <summary>
        ///     Get the mesh's uv array
        /// </summary>
        public Vector2[] UvArray { get; private set; }

        /// <summary>
        ///     Get the mesh's index/triangle array
        /// </summary>
        public int[] IndexArray { get; private set; }

        /// <summary>
        ///     The texture used by the mesh
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        ///     Clears the mesh vertices, uvs and indices
        /// </summary>
        public void Clear() {
            vertices.Clear();
            uvs.Clear();
            indices.Clear();
            VertexArray = null;
            UvArray = null;
            IndexArray = null;
        }
    }
}