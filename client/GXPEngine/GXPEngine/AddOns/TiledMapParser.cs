using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace TiledMapParser {
    /// <summary>
    ///     Call the method MapParser.ReadMap, with as argument a Tiled file exported as xml (file extension: .tmx),
    ///     to get an object of type Map.
    ///     This object, together with its nested objects, contains most of the information contained in the Tiled file.
    ///     The nesting of objects mimics the structure of the Tiled file exactly.
    ///     (For instance, a Map can contain multiple (tile) Layers, ObjectgroupLayers, ImageLayers, which
    ///     all can have a PropertyList, etc.)
    ///     You should extend this class yourself if you want to read more information from the Tiled file
    ///     (such as tile rotations, geometry objects, ...). See
    ///     http://docs.mapeditor.org/en/stable/reference/tmx-map-format/
    ///     for details.
    /// </summary>
    public class MapParser {
        private static readonly XmlSerializer serial = new XmlSerializer(typeof(Map));

        public static Map ReadMap(string filename) {
            TextReader reader = new StreamReader(filename);
            var myMap = serial.Deserialize(reader) as Map;
            reader.Close();

            return myMap;
        }

        public static void WriteMap(string filename, Map map) {
            TextWriter writer = new StreamWriter(filename);
            serial.Serialize(writer, map);
            writer.Close();
        }
    }

    [XmlRootAttribute("map")]
    public class Map {
        [XmlAttribute("height")] public int Height;
        [XmlElement("layer")] public Layer[] Layers;
        [XmlElement("objectgroup")] public ObjectGroup[] ObjectGroups;
        [XmlElement("imagelayer")] public ImageLayer[] ImageLayers;

        [XmlText] public string InnerXML; // This should be empty

        [XmlAttribute("nextobjectid")] public int NextObjectId;

        [XmlAttribute("orientation")] public string Orientation;
        [XmlAttribute("renderorder")] public string RenderOrder;
        [XmlAttribute("tileheight")] public int TileHeight;

        [XmlElement("tileset")] public TileSet[] TileSets;

        [XmlAttribute("tilewidth")] public int TileWidth;

        [XmlAttribute("version")] public string Version;
        [XmlAttribute("width")] public int Width;

        public override string ToString() {
            var output = "Map of width " + Width + " and height " + Height + ".\n";

            output += "TILE LAYERS:\n";
            foreach (var l in Layers)
                output += l.ToString();

            output += "IMAGE LAYERS:\n";
            foreach (var l in ImageLayers)
                output += l.ToString();

            output += "TILE SETS:\n";
            foreach (var t in TileSets)
                output += t.ToString();

            output += "OBJECT GROUPS:\n";
            foreach (var g in ObjectGroups)
                output += g.ToString();

            return output;
        }

        /// <summary>
        ///     A helper function that returns the tile set that belongs to the tile ID read from the layer data:
        /// </summary>
        public TileSet GetTileSet(int tileID) {
            if (tileID < 0)
                return null;
            var index = 0;
            while (TileSets[index].FirstGId + TileSets[index].TileCount <= tileID) {
                index++;
                if (index >= TileSets.Length)
                    return null;
            }

            return TileSets[index];
        }
    }

    [XmlRootAttribute("tileset")]
    public class TileSet {
        [XmlAttribute("columns")] public int Columns;

        /// <summary>
        /// This is the number of the first tile. Usually 1 (so 0 means empty/no tile).
        //// When multiple tilesets are used, this is the total number of previous tiles + 1.
        /// </summary>
        [XmlAttribute("firstgid")] public int FirstGId;
        [XmlElement("image")] public Image Image;

        [XmlAttribute("name")] public string Name;
        [XmlAttribute("tilecount")] public int TileCount;
        [XmlAttribute("tileheight")] public int TileHeight;
        [XmlAttribute("tilewidth")] public int TileWidth;

        public int Rows {
            get {
                if (TileCount % Columns == 0)
                    return TileCount / Columns;
                return TileCount / Columns + 1;
            }
        }

        public override string ToString() {
            return "Tile set: Name: " + Name + " Image: " + Image + " Tile dimensions: " + TileWidth + "x" + TileHeight +
                   " Grid dimensions: " + Columns + "x" + (int) Math.Ceiling(1f * TileCount / Columns) + "\n";
        }
    }

    public class PropertyContainer {
        [XmlElement("properties")] public PropertyList propertyList;

        public bool HasProperty(string key, string type) {
            if (propertyList == null)
                return false;
            foreach (var p in propertyList.properties)
                if (p.Name == key && p.Type == type)
                    return true;
            return false;
        }

        public string GetStringProperty(string key, string defaultValue = "") {
            if (propertyList == null)
                return defaultValue;
            foreach (var p in propertyList.properties)
                if (p.Name == key)
                    return p.Value;
            return defaultValue;
        }

        public float GetFloatProperty(string key, float defaultValue = 1) {
            if (propertyList == null)
                return defaultValue;
            foreach (var p in propertyList.properties)
                if (p.Name == key && p.Type == "float")
                    return float.Parse(p.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            return defaultValue;
        }

        public int GetIntProperty(string key, int defaultValue = 1) {
            if (propertyList == null)
                return defaultValue;
            foreach (var p in propertyList.properties)
                if (p.Name == key && p.Type == "int")
                    return int.Parse(p.Value);
            return defaultValue;
        }

        public bool GetBoolProperty(string key, bool defaultValue = false) {
            if (propertyList == null)
                return defaultValue;
            foreach (var p in propertyList.properties)
                if (p.Name == key && p.Type == "bool")
                    return bool.Parse(p.Value);
            return defaultValue;
        }

        public uint GetColorProperty(string key, uint defaultvalue = 0xffffffff) {
            if (propertyList == null)
                return defaultvalue;
            foreach (var p in propertyList.properties)
                if (p.Name == key && p.Type == "color")
                    return TiledUtils.GetColor(p.Value);
            return defaultvalue;
        }
    }

    [XmlRootAttribute("imagelayer")]
    public class ImageLayer : PropertyContainer {
        [XmlElement("image")] public Image Image;
        [XmlAttribute("name")] public string Name;
        [XmlAttribute("offsetx")] public float offsetX;
        [XmlAttribute("offsety")] public float offsetY;

        public override string ToString() {
            return "Image layer: " + Name + " Image: " + Image + "\n";
        }
    }

    [XmlRootAttribute("image")]
    public class Image {
        [XmlAttribute("source")] // AnimSprite file name
        public string FileName;
        [XmlAttribute("height")] // height in pixels
        public int Height;
        [XmlAttribute("width")] // width in pixels
        public int Width;

        public override string ToString() {
            return FileName + " (dim: " + Width + "x" + Height + ")";
        }
    }

    [XmlRootAttribute("layer")]
    public class Layer : PropertyContainer {
        [XmlElement("data")] public Data Data;
        [XmlAttribute("height")] public int Height;
        [XmlAttribute("name")] public string Name;
        [XmlAttribute("width")] public int Width;

        public override string ToString() {
            var output = " Layer name: " + Name;
            output += "Properties:\n" + propertyList;
            output += "Data:" + Data;
            return output;
        }

        /// <summary>
        ///     Returns the tile data from this layer as a 2-dimensional array of shorts.
        ///     It's a column-major array, so use [column,row] as indices.
        ///     This method does a lot of string parsing and memory allocation, so use it only once,
        ///     during level loading.
        /// </summary>
        /// <returns>The tile array.</returns>
        public short[,] GetTileArray() {
            var grid = new short[Width, Height];
            var lines = Data.innerXML.Split('\n');
            var row = 0;

            foreach (var line in lines) {
                if (line.Length <= 1)
                    continue;
                var parseLine = line;
                if (line[line.Length - 1] == ',')
                    parseLine = line.Substring(0, line.Length - 1);

                var chars = parseLine.Split(',');
                for (var col = 0; col < chars.Length; col++)
                    if (col < Width) {
                        var tileNum = short.Parse(chars[col]);
                        grid[col, row] = tileNum;
                    }

                row++;
            }

            return grid;
        }
    }

    [XmlRootAttribute("data")]
    public class Data {
        [XmlAttribute("encoding")] public string Encoding;
        [XmlText] public string innerXML;

        public override string ToString() {
            return innerXML;
        }
    }

    [XmlRootAttribute("properties")]
    public class PropertyList {
        [XmlElement("property")] public Property[] properties;

        public override string ToString() {
            var output = "";
            foreach (var p in properties)
                output += p.ToString();
            return output;
        }
    }

    [XmlRootAttribute("property")]
    public class Property {
        [XmlAttribute("name")] public string Name;
        [XmlAttribute("type")] public string Type = "string";
        [XmlAttribute("value")] public string Value;

        public override string ToString() {
            return "Property: Name: " + Name + " Type: " + Type + " Value: " + Value + "\n";
        }
    }

    [XmlRootAttribute("objectgroup")]
    public class ObjectGroup : PropertyContainer {
        [XmlAttribute("name")] public string Name;
        [XmlElement("object")] public TiledObject[] Objects;

        public override string ToString() {
            var output = "Object group: Name: " + Name + " Objects:\n";
            foreach (var obj in Objects)
                output += obj.ToString();

            return output;
        }
    }

    [XmlRootAttribute("text")]
    public class Text {
        [XmlAttribute("bold")] public int bold;
        [XmlAttribute("color")] public string color = "#FF000000"; // Tiled default
        [XmlAttribute("fontfamily")] public string font;
        [XmlAttribute("pixelsize")] public int fontSize = 16;
        [XmlAttribute("halign")] public string horizontalAlign = "left";
        [XmlAttribute("italic")] public int italic;
        [XmlText] public string text;
        [XmlAttribute("valign")] public string verticalAlign = "top";
        [XmlAttribute("wrap")] public int wrap;

        public uint Color => TiledUtils.GetColor(color);

        public override string ToString() {
            return text;
        }
    }

    [XmlRootAttribute("object")]
    public class TiledObject : PropertyContainer {
        [XmlAttribute("gid")] public int GID = -1;
        [XmlAttribute("height")] // height in pixels
        public float Height;
        [XmlAttribute("id")] public int ID;
        [XmlAttribute("name")] public string Name;
        [XmlElement("text")] public Text textField;
        [XmlAttribute("type")] public string Type;
        [XmlAttribute("width")] // width in pixels
        public float Width;
        [XmlAttribute("x")] public float X;
        [XmlAttribute("y")] public float Y;

        public override string ToString() {
            return "Object: " + Name + " ID: " + ID + " Type: " + Type + " coordinates: (" + X + "," + Y + ") dimensions: (" + Width + "," +
                   Height + ")\n";
        }
    }

    public class TiledUtils {
        /// <summary>
        ///     This translates a Tiled color string to a uint that can be used as a GXPEngine Sprite color.
        /// </summary>
        public static uint GetColor(string htmlColor) {
            if (htmlColor.Length == 9)
                return (uint) (
                    (Convert.ToInt32(htmlColor.Substring(3, 2), 16) << 24) + // R
                    (Convert.ToInt32(htmlColor.Substring(5, 2), 16) << 16) + // G
                    (Convert.ToInt32(htmlColor.Substring(7, 2), 16) << 8) + // B
                    Convert.ToInt32(htmlColor.Substring(1, 2), 16)); // Alpha
            if (htmlColor.Length == 7)
                return (uint) (
                    (Convert.ToInt32(htmlColor.Substring(1, 2), 16) << 24) + // R
                    (Convert.ToInt32(htmlColor.Substring(3, 2), 16) << 16) + // G
                    (Convert.ToInt32(htmlColor.Substring(5, 2), 16) << 8) + // B
                    0xFF); // Alpha
            throw new Exception("Cannot recognize color string: " + htmlColor);
        }
    }
}