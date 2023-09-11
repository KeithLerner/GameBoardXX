using System;
using System.IO;

public class GameBoard
{
    #region Data
    public byte[] Value { get; private set; }
    public byte[] MagicNumber
    {
        get
        {
            byte[] magicNumber = new byte[4];
            Array.Copy(Value, 0, magicNumber, 0, 4);
            return magicNumber;
        }
    }
    public byte[] Header
    {
        get
        {
            byte[] header = new byte[8];
            Array.Copy(Value, 0, header, 0, 8);
            return header;
        }
    }

    /// <summary>
    /// The number of bits required to store a tile of this board.
    /// </summary>
    public byte TileSize
    {
        get
        {
            byte[] gameBoardXX = new byte[2];
            Array.Copy(MagicNumber, 2, gameBoardXX, 0, 2);
            return BitConverter.ToInt16(gameBoardXX, 0) switch
            {
                0x3830 => 8,
                0x3631 => 16,
                0x3233 => 32,
                0x3436 => 64,
                _ => throw new Exception("Invalid Tile Size."),
            };
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
    /// The number of files the GameBoard contains.
    /// </summary>
    public byte Width
    {
        get
        {
            return Header[4];
        }
    }
    /// <summary>
    /// The number of ranks the GameBoard contains.
    /// </summary>
    public byte Length
    {
        get
        {
            return Header[5];
        }
    }

    public ushort Area => (ushort)(Width * Length);

    public byte[] Footer
    {
        get
        {
            byte[] footer = new byte[128];
            Array.Copy(Value, GameBoardData.HeaderLength + Area, footer, 0, 128);
            return footer;
        }
    }
    #endregion

    #region Constructors
    public GameBoard(byte tileSize, byte BoardWidth, byte BoardLength, byte modifiers = 0b0,
        byte special = 0b0)
    {
        switch (tileSize)
        {
            default: throw new Exception("Invalid Tile Size.");
            case 8: break; case 16: break; case 32: break; case 64: break;
        }

        this.Value = new byte[(BoardWidth * BoardLength * tileSize) + 
            GameBoardData.HeaderLength + GameBoardData.FooterLength];
        
        byte[] header = GameBoardData.Header(tileSize, BoardWidth, BoardLength, 
            modifiers, special);
        Array.Copy(header, 0, this.Value, 0, GameBoardData.HeaderLength);
        
        byte[] footer = GameBoardData.Footer(
            0,
            new byte[2],
            new byte[5],
            new byte[60],
            new byte[60] );
        Array.Copy(footer, 0, this.Value, GameBoardData.HeaderLength + 
            Area, GameBoardData.FooterLength);

        Verify();
    }

    public GameBoard(GameBoardData.GameBoardXXVariant tileSize, byte BoardWidth, byte BoardLength, byte modifiers = 0b0,
        byte special = 0b0)
    {
        byte tSize = tileSize switch
        {
            GameBoardData.GameBoardXXVariant._08 => 8,
            GameBoardData.GameBoardXXVariant._16 => 16,
            GameBoardData.GameBoardXXVariant._32 => 32,
            GameBoardData.GameBoardXXVariant._64 => 64,
            _ => 0
        };

        this.Value = new byte[(BoardWidth * BoardLength * tSize) +
            GameBoardData.HeaderLength + GameBoardData.FooterLength];

        byte[] header = GameBoardData.Header(tileSize, BoardWidth, BoardLength,
            modifiers, special);
        Array.Copy(header, 0, this.Value, 0, GameBoardData.HeaderLength);

        byte[] footer = GameBoardData.Footer(
            0,
            new byte[2],
            new byte[5],
            new byte[60],
            new byte[60]);
        Array.Copy(footer, 0, this.Value, GameBoardData.HeaderLength +
            Area, GameBoardData.FooterLength);

        Verify();
    }

    public GameBoard(byte[] BoardData)
    {
        Value = BoardData;
        Verify();
    }

    public GameBoard(GameBoard target)
    {
        Value = target.Value;
        Verify();
    }
    #endregion

    #region GameBoard Contents
    /// <summary>
    /// Verifies GameBoard data is valid.
    /// </summary>
    /// <returns>True when the GameBoard contains valid data, 
    /// throws exceptions otherwise.</returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="RankException"></exception>
    public bool Verify()
    {
        if (BitConverter.ToInt16(MagicNumber, 0) != 0x4247)
            throw new FormatException(
                "Invalid GameBoard data: Value does not match GameBoard file format.");

        if (Value.Length <= Header.Length)
            throw new FormatException(
                "Invalid GameBoard data: Value does not reach minimum length.");

        if (Header.Length + (Area * TileSize) + Footer.Length != Value.Length)
            throw new RankException(
                "Invalid GameBoard data: Value does not match expected length.");

        return true;
    }

    /// <summary>
    /// Set the data of a tile in the GameBoard. Indexing starts at 0.
    /// </summary>
    /// <param name="tile">The data of the tile being placed.</param>
    /// <param name="dataPosition">The position in the array to place the data. 
    /// Positions are equal to 'Header Size + 
    /// (X-Coordinate + Y-Coordinate * GameBoard Width) * Tile Size'.
    /// </param>
    public void PlaceTile(byte[] tile, int dataPosition)
    {
        if (tile.Length != TileSize)
            throw new FormatException(
                "Invalid GameBoard data: Tile does not match expected size.");
        Array.Copy(tile, 0, Value, dataPosition, TileSize);
    }

    /// <summary>
    /// Set the data of a tile in the GameBoard. Indexing starts at 0.
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
                "Invalid GameBoard data: Tile does not match expected size.");
        int arrayCoordinate = Header.Length + 
            (xCoordinate + yCoordinate * Width) * TileSize;
        Array.Copy(tile, 0, Value, arrayCoordinate, TileSize);
    }

    /// <summary>
    /// Get the data of a tile in the GameBoard. Indexing starts at 0.
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

    public bool CoordinateIsOnBoard(byte xCoordinate, byte yCoordinate)
    {
        if (xCoordinate == 0 || yCoordinate == 0) return false;
        if (xCoordinate > Width || yCoordinate > Length) return false;

        return true;
    }

    //The following needs testing but i don't feel like doing it right now!
    /*
    public (byte, byte) CoordinateToRawIndex(int coordinate)
    {
        
    }
    */
    #endregion

    #region Importing and Exporting
    /// <summary>
    /// Outputs the GameBoard as a string.
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
    /// Outputs the GameBoard as a multi-line string.
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
    /// Outputs the GameBoard as a 3D array of all bytes.
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
    /// Opens a Game++ GameBoard file, reads the contents of the file into a byte 
    /// array, and then closes the file.
    /// </summary>
    /// <param name="path">The path to import the file from.</param>
    public void ImportBoardFile(string path = "..\\MyBoard.cbxx")
    {
        Value = File.ReadAllBytes(path);
        Verify();
    }

    /// <summary>
    /// Creates a new Game++ GameBoard file, writes the specified byte array to the 
    /// file, and then closes the file. If the target file already exists, it 
    /// is overwritten.
    /// </summary>
    /// <param name="path">The path to export the file to.</param>
    public void ExportBoardFile(string path = "..\\MyBoard.cbxx")
    {
        Verify();
        File.WriteAllBytes(path, Value);
    }
    #endregion
}
