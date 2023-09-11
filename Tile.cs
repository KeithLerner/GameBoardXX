using System;
using System.IO;
using UnityEngine;

public class Tile
{
    public static string DefaultPath => Application.dataPath;

    private byte[] m_Value;
    public byte[] Value => m_Value;
    public byte TileSize => Header[3];
    private byte m_TileSize
    {
        set
        {
            Header[3] = value switch
            {
                8 => 8,
                16 => 16,
                32 => 32,
                64 => 64,
                _ => throw new Exception("Invalid Tile Size."),
            };
        }
    }
    // TXX
    public byte[] MagicNumber
    {
        get
        {
            ushort tSize = TileSize switch
            {
                 8 => 0x3830,
                16 => 0x3631,
                32 => 0x3233,
                64 => 0x3436,
                _ => throw new Exception("Invalid Tile Size."),
            };
            byte[] tSizeIEC = BitConverter.GetBytes(tSize);
            byte[] magicNumber = new byte[3];
            magicNumber[0] = 0x54;
            magicNumber[1] = tSizeIEC[0];
            magicNumber[2] = tSizeIEC[1];
            return magicNumber;
        }
    }
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

    private Color32 m_Color;
    public Color32 PreviewColor => m_Color;
    private string m_Name;
    public string Name => m_Name;

    public Tile(byte[] value)
    {
        this.m_Value = value;

        byte[] colorBytes = new byte[4];
        Array.Copy(Footer, 0, colorBytes, 0, 4);
        m_Color = new Color32(colorBytes[0], colorBytes[1], 
            colorBytes[2], colorBytes[3]);

        char[] nameBytes = new char[12];
        Array.Copy(Footer, 4, nameBytes, 0, 12);
        m_Name = new string(nameBytes);
    }

    public Tile(byte[] tile, Color color, string name)
    {
        m_TileSize = (byte)tile.Length;
        Array.Copy(tile, 0, m_Value, 4, tile.Length);
        m_Color = color;
        m_Name = name;
    }

    public bool Verify()
    {
        if (MagicNumber[0] != 0x54)
            throw new FormatException(
                "Invalid Tile data: Data does not match required file format.");

        if (BitConverter.ToInt16(MagicNumber, 1) != TileSize switch {
            8 => 0x3830,
            16 => 0x3631,
            32 => 0x3233,
            64 => 0x3436,
            _ => throw new Exception("Invalid Tile Size.")
        })
            throw new FormatException(
                "Invalid Tile data: Data does not match required file format.");

        if (Value.Length <= 4)
            throw new FormatException(
                "Invalid Tile data: Value does not reach minimum length.");

        if (4 + TileSize + 16 != Value.Length)
            throw new RankException(
                "Invalid Tile data: Value does not match required length.");

        return true;
    }

    public void ImportBoardFile(string path = "..\\MyTile.txx")
    {
        m_Value = File.ReadAllBytes(path);
        Verify();
    }

    public void ExportBoardFile(string path = "..\\MyBoard.txx")
    {
        Verify();
        File.WriteAllBytes(path, m_Value);
    }
}