using System;
using UnityEngine;

public class BoardDesigner : MonoBehaviour
{
    #region Data
    [SerializeField]
    private byte m_MaxBoardSize = 32; // Game specific value

    private Board m_Board;
    public enum AppendStyle { Column, Row, Both }
    public enum ShiftStyle { Up, Down, Left, Right }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        byte[] t64 = BoardData.Tile64(0b00001000, 0b0, 0b0, 0b0, 0b0, 0b0, 0b00000011, 0b0);
        byte[] up64 = BoardData.Tile64(0b00001000, 0b0, 0b0, 0b00010001, 0b00100100, 0b0, 0b00000011, 0b0);
        byte[] left64 = BoardData.Tile64(0b00001000, 0b0, 0b0, 0b00100010, 0b00011000, 0b0, 0b00000011, 0b0);
        byte[] down64 = BoardData.Tile64(0b00001000, 0b0, 0b0, 0b00010100, 0b00100001, 0b0, 0b00000011, 0b0);
        byte[] right64 = BoardData.Tile64(0b00001000, 0b0, 0b0, 0b00101000, 0b00010010, 0b0, 0b00000011, 0b0);

        byte[][] basic4PlayerTileLayoutT64 = {
        BoardData.Header(64, 8, 8, 0b10000000, 0),
        t64, up64, up64, up64, up64, up64, up64, t64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        t64, down64, down64, down64, down64, down64, down64, t64
    };
        //m_Board = new Board("D:\\Unity\\Projects\\GamePlusPlus\\Assets\\Binaries\\Boards\\MyBoard.GameBoard");
        //m_Board.ImportBoardFile();

        //Debug.Log(m_Board.OutputBoardString + ", " + m_Board.Dimensions + "\n" + m_Board.OutputBoardString2D);
        //Debug.Log("d4 tile is " + m_Board.GetTile(GamePlusPlusCoordinateConverter.ToCoordinates["d4"]));


        string s = "";

        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Board Manipulation
    /// <summary>
    /// Add rows and columns to Board, adjust any existing data to new positions.
    /// </summary>
    /// <param name="appendStyle">The method in which the tiles will be 
    /// appended.</param>
    /// <param name="quantity">The number of times to repeat the 
    /// opperation.</param>
    /// <param name="defaultValue"></param>
    public void AppendToBoard(AppendStyle appendStyle, short quantity = 1)
    {
        m_Board.Verify();

        // Copy 1d array to 2d of new dimensions
        // Populate with data
        // Perform shift if requested
        byte w = m_Board.Width;
        byte l = m_Board.Length;
        byte[,,] array3d = m_Board.OutputByteArray3D();

        m_Board = new Board(m_Board.TileSize, (byte)(w + (appendStyle == AppendStyle.Row ? 0 : 1)),
            (byte)(l + (appendStyle == AppendStyle.Column ? 0 : 1)));

        for (byte x = 0; x < w; x++)
        {
            for (byte y = 0; y < l; y++)
            {
                byte[] tile = new byte[m_Board.TileSize];
                for (int i = 0; i < m_Board.TileSize / 8; i++)
                {
                    tile[i] = array3d[x, y, i];
                }
                m_Board.PlaceTile(tile, x, y);
            }
        }

        quantity--;
        if (quantity > 0)
        {
            AppendToBoard(appendStyle, quantity);
        }
    }

    /*
    public void PopFromBoard() 
    {
        throw new NotImplementedException("NOT READY YET!!");
        // Remove and return rows and columns to the Board,
        // adjust any existing data to new positions
    }
    */

    /// <summary>
    /// Shift all the data in the Board, adjust any existing data to new positions.
    /// </summary>
    /// <param name="shiftStyle">Which direction to shift the data in.</param>
    /// <param name="wrap">Whether the data will wrap around the edges of the board.</param>
    /// <param name="quantity">The number of times to repeat the shift of data.</param>
    /// <exception cref="ArgumentException">Invalid quantity.</exception>
    public void ShiftBoard(ShiftStyle shiftStyle, bool wrap = false,
        byte quantity = 1)
    {
        m_Board.Verify();
        if (quantity < 0 || quantity > m_MaxBoardSize)
        {
            throw new Exception("Shift Quantity Invalid: " + quantity);
        }
        byte tSize = m_Board.TileSize;
        byte xMaxTileCoord = (byte)(m_Board.Width - 1);
        byte yMaxTileCoord = (byte)(m_Board.Length - 1);
        byte[] shiftedData = new byte[m_Board.Value.Length];
        shiftedData[4] = (byte)(xMaxTileCoord + 1);
        shiftedData[5] = (byte)(yMaxTileCoord + 1);
        byte[] tempRow = new byte[xMaxTileCoord * tSize];
        byte[] tempCol = new byte[yMaxTileCoord * tSize];

        switch (shiftStyle)
        {
            case ShiftStyle.Up:
                if (wrap)
                {
                    // store rowMax
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        Array.Copy(m_Board.GetTile(x, yMaxTileCoord), 0,
                            tempRow, x * tSize, tSize);
                    }

                }
                // shift data up for all rows except row0
                for (byte x = 0; x < xMaxTileCoord; x++)
                {
                    for (byte y = yMaxTileCoord; y > 0; y--)
                    {
                        Array.Copy(m_Board.GetTile(x, (byte)(y - 1)), 0,
                            shiftedData, m_Board.ConvertCoordinate(x, y), tSize);
                    }
                }
                if (wrap)
                {
                    // replace row0 with old rowMax
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        Array.Copy(tempRow, x * tSize, shiftedData,
                            m_Board.ConvertCoordinate(x, 0), tSize);
                    }
                }
                else
                {
                    // replace row0 with empty tiles
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        shiftedData[m_Board.ConvertCoordinate(x, 0)] = byte.MinValue;
                    }
                }
                break;

            case ShiftStyle.Down:
                if (wrap)
                {
                    // store row0
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        Array.Copy(m_Board.GetTile(x, 0), 0,
                            tempRow, x * tSize, tSize);
                    }
                }
                // shift data down for all rows except rowMax
                for (byte x = 0; x < xMaxTileCoord; x++)
                {
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(m_Board.GetTile(x, (byte)(y + 1)), 0,
                            shiftedData, m_Board.ConvertCoordinate(x, y), tSize);
                    }
                }
                if (wrap)
                {
                    // replace rowMax with old row0
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        Array.Copy(tempRow, x * tSize, shiftedData,
                            m_Board.ConvertCoordinate(x, yMaxTileCoord), tSize);
                    }
                }
                else
                {
                    // replace rowMax with empty tiles
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        shiftedData[m_Board.ConvertCoordinate(x, yMaxTileCoord)] = byte.MinValue;
                    }
                }
                break;

            case ShiftStyle.Left:
                if (wrap)
                {
                    // store col 0
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(m_Board.GetTile(0, y), 0,
                            tempCol, y * tSize, tSize);
                    }
                }
                // shift data left for all cols except colMax
                for (byte x = 0; x < xMaxTileCoord; x++)
                {
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(m_Board.GetTile((byte)(x + 1), y), 0,
                            shiftedData, m_Board.ConvertCoordinate(x, y), tSize);
                    }
                }
                if (wrap)
                {
                    // replace colMax with old col0
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(tempRow, y * tSize, shiftedData,
                            m_Board.ConvertCoordinate(xMaxTileCoord, y), tSize);
                    }
                }
                else
                {
                    // replace colMax with empty tiles
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        shiftedData[m_Board.ConvertCoordinate(xMaxTileCoord, y)] = byte.MinValue;
                    }
                }
                break;

            case ShiftStyle.Right:
                if (wrap)
                {
                    // store colMax
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(m_Board.GetTile(xMaxTileCoord, y), 0,
                            tempCol, y * tSize, tSize);
                    }
                }
                // shift data right for all cols except col0
                for (byte x = xMaxTileCoord; x > 0; x--)
                {
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(m_Board.GetTile((byte)(x - 1), y), 0,
                            shiftedData, m_Board.ConvertCoordinate(x, y), tSize);
                    }
                }
                if (wrap)
                {
                    //replace left row with old right row
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(tempRow, y * tSize, shiftedData,
                            m_Board.ConvertCoordinate(0, y), tSize);
                    }
                }
                else
                {
                    // replace col0 with empty tiles
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        shiftedData[m_Board.ConvertCoordinate(0, y)] = byte.MinValue;
                    }
                }
                break;
        }

        m_Board = new Board(shiftedData);

        quantity--;
        if (quantity > 0)
        {
            ShiftBoard(shiftStyle, wrap, quantity);
        }
    }

    // TO DO 
    //==================================================
    //Fill tiles
    //takes array of (int,int) coordinates to fill
    //and a byte to fill the designated coordinates with

    //used for brushes/tools for eventual Board creator
    #endregion

}