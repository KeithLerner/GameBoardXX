using System;
using System.IO;
using System.Drawing;

public class TilePalette
{
    public struct Tile
    {
        public byte[] value;
        public byte[] tile;
        public Color color;
        public string name;

        public Tile(byte[] value)
        {
            this.value = value;
            tile = new byte[value.Length - 16];
            Array.Copy(value, 0, tile, 0, value.Length - 16);
            color = Color.FromArgb(value[value.Length - 16]);
            char[] nameBytes = new char[12];
            Array.Copy(value, value.Length - 12, nameBytes, 0, 12);
            name = new string(nameBytes);
        }
    }
    #region Data
    public byte[] Value;
    private byte[] m_Value; 
        // Value have normal tile size bytes followed by
        // 4 bytes / 32 bit for color
        // 12 chars / bytes for name
    public readonly byte m_TileSize;
    private readonly byte m_SplotchLength;

    public int TileCount => (m_Value.Length / m_SplotchLength) - 1;
    #endregion

    #region Constructors
    public TilePalette(byte tileSize)
    {
        m_TileSize = tileSize;
        m_SplotchLength = (byte)(tileSize + 4 + 12);
        m_Value = new byte[32 * m_SplotchLength];
    }

    public TilePalette(byte[] value, byte tileSize)
    {
        m_Value = value;
        m_TileSize = tileSize;
        m_SplotchLength = (byte)(tileSize + 4 + 12);
    }
    #endregion

    #region Core
    public byte[] GetTile(byte index)
    {
        byte[] tile = new byte[m_TileSize];
        Array.Copy(m_Value, index * m_SplotchLength, tile, 0, m_TileSize);
        return tile;
    }

    public void SetTile(byte[] tile, byte index, Color tileColor, string tileName)
    {
        Array.Copy(tile, 0, m_Value, index * m_SplotchLength, m_TileSize);
        Array.Copy(BitConverter.GetBytes(tileColor.ToArgb()), 0, m_Value, 
            index * m_SplotchLength + tile.Length, 4);
        Array.Copy(tileName.ToCharArray(), 0, m_Value,
            m_SplotchLength - 12, 12);
    }

    public byte[] PopTile(byte index)
    {
        byte[] popped = new byte[m_SplotchLength];
        Array.Copy(m_Value, index * m_SplotchLength, popped, 0, 
            m_SplotchLength);
        Array.Copy(new byte[m_SplotchLength], 0, m_Value, 
            index * m_SplotchLength, m_SplotchLength);
        return popped;
    }
    #endregion

    #region Import and Export
    public static TilePalette ImportTilePaletteFile(string path = "..\\MyBoard.cbtp")
    {
        byte[] fileBytes = File.ReadAllBytes(path);
        byte[] valueBytes = new byte[fileBytes.Length - 1];
        Array.Copy(fileBytes, 0, valueBytes, 0, fileBytes.Length - 1);
        return new TilePalette(valueBytes, fileBytes[fileBytes.Length - 1]);
    }

    public void ExportTilePaletteFile(string path = "..\\MyBoard.cbtp")
    {
        byte[] fileBytes = new byte[m_Value.Length + 1];
        fileBytes[fileBytes.Length - 1] = m_TileSize;
        File.WriteAllBytes(path, Value);
    }

    public static TilePalette defaultPalette08
    {
        get
        {
            return new TilePalette(08);
        }
    }
    public static TilePalette defaultPalette16
    {
        get
        {
            return new TilePalette(16);
        }
    }
    public static TilePalette defaultPalette32
    {
        get
        {
            return new TilePalette(32);
        }
    }
    public static TilePalette defaultPalette64
    {
        get
        {
            return new TilePalette(64);
        }
    }
    #endregion
}
