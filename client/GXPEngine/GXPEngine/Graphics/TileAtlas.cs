using GXPEngine.Core;

namespace GXPEngine {
    /// <summary>
    /// Representation of a Tile Atlas (sprite sheet)
    /// </summary>
    public class TileAtlas {
        /// <summary>
        /// Texture with the tiles
        /// </summary>
        public Texture2D TileAtlasTexture;

        /// <summary>
        /// Number of columns in the tile atlas
        /// </summary>
        public int Columns;

        /// <summary>
        /// Number of rows in the tile atlas
        /// </summary>
        public int Rows;

        /// <summary>
        /// UV size per tile (RO)
        /// </summary>
        public readonly Vector2 UvSize;

        public TileAtlas(string tileAtlasPath, int columns, int rows) : this(Texture2D.GetInstance(tileAtlasPath, true), columns, rows) { }

        public TileAtlas(Texture2D tileAtlasTexture, int columns, int rows) {
            TileAtlasTexture = tileAtlasTexture;
            Columns = columns;
            Rows = rows;
            UvSize = new Vector2(1f / Columns, 1f / Rows);
        }

        /// <summary>
        /// Get the UV for a specific tile using column and row for the tile
        /// </summary>
        /// <param name="column">The column of the tile</param>
        /// <param name="row">The row of the tile</param>
        /// <returns><see cref="Vector2"/> containing UV coordinates for the specified column and row</returns>
        public Vector2 GetUV(int column, int row) {
            return UvSize * new Vector2(column, row);
        }
    }
}