using GamePlusPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Board
{
    #region Data
    public byte[] Value { get; private set; }

    public byte[] Header
    {
        get
        {
            byte[] header = new byte[8];
            Array.Copy(Value, 0, header, 0, 8);
            return header;
        }
    }
    // This just shouldn't exist??
    public byte[] MagicNumber
    {
        get
        {
            byte[] magicNumber = new byte[4];
            Array.Copy(Value, 0, magicNumber, 0, 4);
            return magicNumber;
        }
    }
    // This does not do what it should, also maybe it just shouldn't exist?
    public byte[] GameBoardXXVariant
    {
        get
        {
            byte[] gameBoardXX = new byte[2];
            Array.Copy(MagicNumber, 0, gameBoardXX, 2, 2);
            return gameBoardXX;
        }
    }

    public byte TileSize
    {
        get
        {
            switch (BitConverter.ToInt16(GameBoardXXVariant, 0))
            {
                default: throw new Exception("Invalid Tile Size.");
                case 0x3830: return 8;
                case 0x3631: return 16;
                case 0x3233: return 32;
                case 0x3436: return 64;
            }
        }
    }

    public byte[] Body
    {
        get 
        {
            byte[] body = new byte[Area * TileSize];
            Array.Copy(Value, Header.Length, body, 0, body.Length);
            return body;
        }

    }

    /// <summary>
    /// The number of files the Board contains.
    /// </summary>
    public byte Width
    {
        get
        {
            return Header[4];
        }
    }
    /// <summary>
    /// The number of ranks the Board contains.
    /// </summary>
    public byte Length
    {
        get
        {
            return Header[5];
        }
    }

    public ushort Area => (ushort)(Width * Length);
    #endregion

    #region Constructors
    public Board(byte tileSize, byte BoardWidth, byte BoardLength, byte modifiers = 0b0,
        byte winConditions = 0b0)
    {
        switch (tileSize)
        {
            default: throw new Exception("Invalid Tile Size.");
            case 8: break; case 16: break; case 32: break; case 64: break;
        }

        BoardWidth = (byte)Mathf.Clamp(BoardWidth, 1, 31);
        BoardLength = (byte)Mathf.Clamp(BoardLength, 1, 31);

        this.Value = new byte[BoardWidth * BoardLength * tileSize + 
            BoardData.HeaderLength + BoardData.FooterLength];

        Array.Copy(BoardData.Header(tileSize, BoardWidth, BoardLength, modifiers, 
                winConditions), 0, this.Value, 0, BoardData.HeaderLength);
        Array.Copy(BoardData.Footer(), 0, this.Value, BoardData.HeaderLength + 
            Area, BoardData.FooterLength);

        Verify();
    }

    public Board(byte[] BoardData)
    {
        Value = BoardData;
        Verify();
    }

    public Board(Board target)
    {
        Value = target.Value;
        Verify();
    }
    #endregion

    #region Board Contents
    /// <summary>
    /// Verifies Board data is valid.
    /// </summary>
    /// <returns>True when the Board contains valid data, 
    /// throws exceptions otherwise.</returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="RankException"></exception>
    public bool Verify()
    {
        if (BitConverter.ToInt16(MagicNumber, 0) != 0x4D43)
            throw new FormatException(
                "Invalid Board data: Value does not match GameBoard file format.");

        if (Value.Length <= Header.Length)
            throw new FormatException(
                "Invalid Board data: Value does not reach minimum length.");

        if (Width * Length * TileSize + Header.Length != Value.Length)
            throw new RankException(
                "Invalid Board data: Value does not match expected length.");

        return true;
    }

    /// <summary>
    /// Set the data of a tile in the Board. Indexing starts at 0.
    /// </summary>
    /// <param name="tile">The data of the tile being placed.</param>
    /// <param name="xCoordinate">The X Coordinate of the grid position to 
    /// place the tile at.</param>
    /// <param name="yCoordinate">The Y Coordinate of the grid position to 
    /// place the tile at.</param>
    public void PlaceTile(byte[] tile, byte xCoordinate, byte yCoordinate)
    {
        if (tile.Length != TileSize)
            throw new FormatException(
                "Invalid Board data: Tile does not match expected size.");
        int arrayCoordinate = Header.Length + 
            (xCoordinate + yCoordinate * Width) * TileSize;
        Array.Copy(tile, 0, Value, arrayCoordinate, TileSize);
    }

    /// <summary>
    /// Set the data of a tile in the Board. Indexing starts at 0.
    /// </summary>
    /// <param name="tile">The data of the tile being placed.</param>
    /// <param name="dataPosition">The position in the array to place the data. 
    /// Positions are equal to 'Header Size + 
    /// (X-Coordinate + Y-Coordinate * Board Width) * Tile Size'.
    /// </param>
    public void PlaceTile(byte[] tile, int dataPosition)
    {
        if (tile.Length != TileSize)
            throw new FormatException(
                "Invalid Board data: Tile does not match expected size.");
        Array.Copy(tile, 0, Value, dataPosition, TileSize);
    }

    /// <summary>
    /// Get the data of a tile in the Board. Indexing starts at 0.
    /// </summary>
    /// <param name="xCoordinate">X coordinate of the targeted tile.</param>
    /// <param name="yCoordinate">Y coordinate of the targeted tile.</param>
    /// <returns>A byte representing the targeted tile.</returns>
    public byte[] GetTile(byte xCoordinate, byte yCoordinate)
    {
        byte[] tile = new byte[TileSize];
        int arrayCoordinate = Header.Length +
            (xCoordinate + yCoordinate * Width) * TileSize;
        Array.Copy(Value, arrayCoordinate, tile, 0, TileSize);
        return tile;
    }

    public byte[] GetTile(int dataPosition)
    {
        byte[] tile = new byte[TileSize];
        Array.Copy(Value, dataPosition, tile, 0, TileSize);
        return tile;
    }

    public int ConvertCoordinate(byte xCoordinate, byte yCoordinate)
    {
        return Header.Length +
            (xCoordinate + yCoordinate * Width) * TileSize;
    }

    //The following needs testing but i don't feel like doing it right now!
    /*
    public (byte, byte) ConvertCoordinate(int coordinate)
    {
        return (
            (byte)(((coordinate - Header.Length) / TileSize / Width) - (coordinate / Width)),
            (byte)(((coordinate - Header.Length) / TileSize / Width) - (coordinate % Width))
            );

        return (
            (byte)((coordinate - Header.Length) / (Width * TileSize)),
            (byte)((coordinate - Header.Length) % (Width * TileSize))
            );
    }
    */
    #endregion

    #region Importing and Exporting
    /// <summary>
    /// Outputs the Board as a string.
    /// </summary>
    /// <returns>A string of all tiles in Hexadecimal form seperated by 
    /// whitespace.</returns>
    public string OutputBoardString()
    {
        Verify();
        string s = "";
        for (uint i = (uint)Header.Length; i < Value.Length; i++)
        {
            s += Value[i].ToString("X") + " ";
        }
        return s;
    }

    /// <summary>
    /// Outputs the Board as a multi-line string.
    /// </summary>
    /// <returns>A string of all tiles in Hexadecimal form seperated by 
    /// whitespace and newlines.</returns>
    public string OutputBoardString2D()
    {
        Verify();
        string s = "";
        for (byte length = Length; length > 0; length--)
        {
            for (byte width = 0; width < Width; width++)
            {
                byte[] tile = new byte[TileSize];
                Array.Copy(GetTile(width, length), tile, TileSize);
                s += BitConverter.ToUInt64(tile, 0).ToString("X") + " ";
            }
            s += "\n";
        }
        return s;
    }

    /// <summary>
    /// Outputs the Board as a 3D array of all bytes.
    /// </summary>
    /// <returns>A 3D byte array indexed by X Coordinate, Y Coordinate, 
    /// Tile Size.</returns>
    public byte[,,] OutputByteArray3D()
    {
        Verify();
        byte[,,] byteArray3D = new byte[Width, Length, TileSize / 8];
        for (byte x = 0; x < Width; x++)
        {
            for (byte y = 0; y < Length; y++)
            {
                for (int i = 0; i < TileSize; i++)
                {
                    byteArray3D[i, x, y] = GetTile(x, y)[i];
                }
            }
        }
        return byteArray3D;
    }

    /// <summary>
    /// Opens a Game++ Board file, reads the contents of the file into a byte 
    /// array, and then closes the file.
    /// </summary>
    /// <param name="path">The path to import the file from.</param>
    public void ImportBoardFile(string path = "D:\\Unity\\Projects\\GamePlusPlus\\Assets\\Components\\Boards\\MyBoard.cbxx")
    {
        Value = File.ReadAllBytes(path);
        Verify();
    }

    /// <summary>
    /// Creates a new Game++ Board file, writes the specified byte array to the 
    /// file, and then closes the file. If the target file already exists, it 
    /// is overwritten.
    /// </summary>
    /// <param name="path">The path to export the file to.</param>
    public void ExportBoardFile(string path = "D:\\Unity\\Projects\\GamePlusPlus\\Assets\\Components\\Boards\\MyBoard.cbxx")
    {
        Verify();
        File.WriteAllBytes(path, Value);
    }
    #endregion
}
