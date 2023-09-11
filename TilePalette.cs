using System;
using System.IO;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePalette
{
    public static string FileExtension = ".gbtp";

    #region Data
    public byte[] Value;
    private byte[] m_Value; 

    private readonly byte m_TileSize;
    public byte TileSize => m_TileSize;
    
    private byte m_SplotchLength => (byte)(m_TileSize + 20);
    public byte PaletteSize => (byte)(m_Value.Length / m_TileSize);

    //TPXX
    public byte[] Header
    {
        get
        {
            byte[] header = new byte[4];
            Array.Copy(Value, 0, header, 0, 4);
            return header;
        }
    }
    public byte[] Body
    {
        get
        {
            byte[] body = new byte[TileSize];
            Array.Copy(Value, Header.Length, body, 0, body.Length);
            return body;
        }

    }
    public byte[] Footer
    {
        get
        {
            byte[] footer = new byte[16];
            Array.Copy(Value, 4 + TileSize, footer, 0, 16);
            return footer;
        }
    }

    public int TileCount => (m_Value.Length / m_SplotchLength) - 1;
    #endregion

    #region Constructors
    public TilePalette(byte[] value)
    {
        m_Value = value;
        byte[] tileXX = new byte[2];
        Array.Copy(m_Value, 2, tileXX, 0, 2);
        m_TileSize = BitConverter.ToInt16(tileXX, 0) switch
        {
            0x3830 => 8,
            0x3631 => 16,
            0x3233 => 32,
            0x3436 => 64,
            _ => throw new Exception("Invalid Tile Size."),
        };
    }
    public TilePalette(Tile[] splotches)
    {
        m_TileSize = splotches[0].TileSize;
        byte[] splotchBytes = new byte[splotches.Length * m_TileSize];
        for (int i = 0; i < splotches.Length; i++)
        {
            if (splotches[i].TileSize != m_TileSize)
                throw new Exception("Invalid Tile Size: " + i);
            Array.Copy(splotches[i].Value, 0, splotchBytes, 
                m_SplotchLength * i, splotches[i].Value.Length);
        }
    }
    public TilePalette(byte tileSize, byte paletteSize)
    {
        m_TileSize = tileSize switch
        {
            8 => 8,
            16 => 16,
            32 => 32,
            64 => 64,
            _ => throw new Exception("Invalid Tile Size."),
        };
        m_Value = new byte[paletteSize * m_SplotchLength];
    }
    #endregion

    #region Core
    public byte[] GetTile(int index)
    {
        byte[] tile = new byte[m_TileSize];
        Array.Copy(m_Value, index * m_SplotchLength, tile, 0, m_TileSize);
        return tile;
    }

    public void SetTile(byte[] tile, int index, Color32 tileColor, string tileName)
    {
        ushort tSize = tile.Length switch
        {
            8 => 0x3830,
            16 => 0x3631,
            32 => 0x3233,
            64 => 0x3436,
            _ => throw new Exception("Invalid Tile Size."),
        };
        byte[] tSizeIEC = BitConverter.GetBytes(tSize);
        m_Value[index * m_SplotchLength] = 0x54;
        m_Value[index * m_SplotchLength + 1] = tSizeIEC[0];
        m_Value[index * m_SplotchLength + 2] = tSizeIEC[1];
        m_Value[index * m_SplotchLength + 3] = m_TileSize;
        Array.Copy(tile, 0, m_Value, index * m_SplotchLength + 4, tile.Length);
        Array.Copy(new byte[] { tileColor[0], tileColor[1], 
            tileColor[2], tileColor[3] }, 0, m_Value,
            index * m_SplotchLength + 4 + m_TileSize, 4);
        Array.Copy(tileName.ToCharArray(), 0, m_Value,
            m_SplotchLength - 12, 12);
    }

    public void SetTile(Tile tile, int index)
    {
        Array.Copy(tile.Value, 0, m_Value, index * m_SplotchLength, 
            tile.Value.Length);
        Array.Copy(new byte[] { tile.PreviewColor[0], tile.PreviewColor[1],
            tile.PreviewColor[2], tile.PreviewColor[3] }, 0, m_Value,
            index * m_SplotchLength + 4 + m_TileSize, 4);
        Array.Copy(tile.Name.ToCharArray(), 0, m_Value,
            m_SplotchLength - 12, 12);
    }

    public byte[] PopTile(int index)
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
    public bool Verify()
    {
        byte[] header =
            {
                0x54,
                0x50,
                m_TileSize switch
                {
                    08 => 0x30,
                    16 => 0x31,
                    32 => 0x33,
                    64 => 0x36,
                    _ => throw new Exception("Invalid Tile Size")
                },
                m_TileSize switch
                {
                    08 => 0x38,
                    16 => 0x36,
                    32 => 0x32,
                    64 => 0x34,
                    _ => throw new Exception("Invalid Tile Size")
                }
            };
        if (Header != header)
            throw new FormatException(
                "Invalid Tile data: Data does not match required file format.");

        if (Value.Length <= 2 * m_SplotchLength + 4)
            throw new FormatException(
                "Invalid Tile Palette data: Value does not reach minimum length.");

        if (4 + TileSize + 16 != Value.Length)
            throw new RankException(
                "Invalid Tile Palette data: Value does not match required length.");

        return true;
    }

    public void ImportBoardFile(string path = "..\\MyPalette.gbtp")
    {
        m_Value = File.ReadAllBytes(path);
        Verify();
    }

    public void ExportBoardFile(string path = "..\\MyPalette.gbtp")
    {
        Verify();
        File.WriteAllBytes(path, m_Value);
    }
    #endregion
}
