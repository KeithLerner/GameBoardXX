using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class GameBoardDesigner
{
    #region Data
    private TilePalette m_Palette;
    public TilePalette Palette => m_Palette;
    private GameBoard m_Board;
    public GameBoard Board => m_Board;

    private Stack<byte[]> m_UndoBuffer;
    private Stack<byte[]> m_RedoBuffer;

    private HashSet<ushort> m_MultiSelect;
    private HashSet<ushort> m_BrushTempSelect;

    private PaintBrushSettings m_PaintBrushSettings;
    #endregion

    #region Constructors
    public GameBoardDesigner()
    {
        m_Board = new GameBoard(8, 8, 8);

        m_Palette = m_Board.TileSize switch
        {
            8 => TilePalette.defaultPalette08,
            16 => TilePalette.defaultPalette16,
            32 => TilePalette.defaultPalette32,
            64 => TilePalette.defaultPalette64,
            _ => new TilePalette(0)
        };

        m_UndoBuffer = new Stack<byte[]>();
        m_RedoBuffer = new Stack<byte[]>();

        m_MultiSelect = new HashSet<ushort>();

        m_PaintBrushSettings = new PaintBrushSettings(
            BrushShape.Circle, 1);
    }
    public GameBoardDesigner(GameBoard toLoad)
    {
        m_Board = toLoad;

        m_UndoBuffer = new Stack<byte[]>();
        m_RedoBuffer = new Stack<byte[]>();

        m_MultiSelect = new HashSet<ushort>();

        m_PaintBrushSettings = new PaintBrushSettings(
            BrushShape.Circle, 1);
    }
    #endregion

    #region GameBoard Edit States
    private void SaveEditState()
    {
        m_UndoBuffer.Push(m_Board.Value);
    }

    public void Undo()
    {
        m_RedoBuffer.Push(m_Board.Value);
        m_Board = new GameBoard(m_UndoBuffer.Pop());
    }

    public void Redo()
    {
        m_UndoBuffer.Push(m_Board.Value);
        m_Board = new GameBoard(m_RedoBuffer.Pop());
    }

    public void ClearEditHistory()
    {
        m_UndoBuffer.Clear();
        m_RedoBuffer.Clear();
    }
    #endregion

    #region GameBoard Manipulation
    public void NewBoard(GameBoardData.GameBoardXXVariant tileSize, byte width, byte length)
    {
        m_Board = new GameBoard(tileSize, width, length);
    }

    public enum Edge { Top, Left, Bottom, Right }
    /// <summary>
    /// Add rows and columns to GameBoard, adjust any existing data to new 
    /// positions.
    /// </summary>
    /// <param name="appendTo">The edge which the tiles will be 
    /// appended to.</param>
    /// <param name="quantity">The number of times to repeat the 
    /// opperation.</param>
    /// <param name="defaultValue"></param>
    public void AppendToBoard(Edge appendTo, short quantity = 1)
    {
        m_Board.Verify();

        // Copy 1d array to 2d of new dimensions
        // Populate with data
        // Perform shift if requested
        byte w = m_Board.Width;
        byte l = m_Board.Length;
        byte[,,] array3d = m_Board.OutputByteArray3D();

        m_Board = new GameBoard(
            m_Board.TileSize, 
            (byte)(w + (((int)appendTo % 2 == 1) ? 0 : 1)),
            (byte)(l + (((int)appendTo % 2 == 0) ? 0 : 1))
        );

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

        if (appendTo == Edge.Left)
        {
            ShiftBoard(ShiftStyle.Right);
        }
        if (appendTo == Edge.Bottom)
        {
            ShiftBoard(ShiftStyle.Up);
        }

        quantity--;
        if (quantity > 0)
        {
            AppendToBoard(appendTo, quantity);
        }

        SaveEditState();
    }

    public void SubtractFromBoard(Edge popFrom, short quantity = 1) 
    {
        m_Board.Verify();

        // Copy 1d array to 2d of new dimensions
        // Populate with data
        // Perform shift if requested
        byte w = m_Board.Width;
        byte l = m_Board.Length;
        byte[,,] array3d = m_Board.OutputByteArray3D();

        m_Board = new GameBoard(
            m_Board.TileSize,
            (byte)(w - (((int)popFrom % 2 == 1) ? 0 : 1)),
            (byte)(l - (((int)popFrom % 2 == 0) ? 0 : 1))
        );

        if (popFrom == Edge.Left)
        {
            ShiftBoard(ShiftStyle.Left);
        }
        if (popFrom == Edge.Bottom)
        {
            ShiftBoard(ShiftStyle.Down);
        }

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
            AppendToBoard(popFrom, quantity);
        }

        SaveEditState();
    }

    public enum ShiftStyle { Up, Left, Down, Right }
    /// <summary>
    /// Shift all the data in the GameBoard, adjust any existing data to new 
    /// positions.
    /// </summary>
    /// <param name="shiftStyle">Which direction to shift the data in.</param>
    /// <param name="wrap">Whether the data will wrap around the edges of the 
    /// board.</param>
    /// <param name="quantity">The number of times to repeat the shift of 
    /// data.</param>
    /// <exception cref="ArgumentException">Invalid quantity.</exception>
    public void ShiftBoard(ShiftStyle shiftStyle, bool wrap = false,
        byte quantity = 1)
    {
        m_Board.Verify();
        if (quantity < 0 || quantity >= 255)
        {
            throw new Exception("Shift Quantity Invalid: " + quantity);
        }
        GameBoardData.ConversionAssist assist = 
            new GameBoardData.ConversionAssist(
                m_Board.Width, m_Board.TileSize);
        byte xMaxTileCoord = (byte)(m_Board.Width - 1);
        byte yMaxTileCoord = (byte)(m_Board.Length - 1);
        byte[] shiftedData = new byte[m_Board.Value.Length];
        shiftedData[4] = (byte)(xMaxTileCoord + 1);
        shiftedData[5] = (byte)(yMaxTileCoord + 1);
        byte[] tempRow = new byte[xMaxTileCoord * assist.TileSize];
        byte[] tempCol = new byte[yMaxTileCoord * assist.TileSize];

        switch (shiftStyle)
        {
            case ShiftStyle.Up:
                if (wrap)
                {
                    // store rowMax
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        Array.Copy(m_Board.GetTile(x, yMaxTileCoord), 0,
                            tempRow, x * assist.TileSize, assist.TileSize);
                    }

                }
                // shift data up for all rows except row0
                for (byte x = 0; x < xMaxTileCoord; x++)
                {
                    for (byte y = yMaxTileCoord; y > 0; y--)
                    {
                        Array.Copy(m_Board.GetTile(x, (byte)(y - 1)), 0,
                            shiftedData, GameBoardData.CoordinateToRawIndex(
                                (x, y), assist.Width), assist.TileSize);
                    }
                }
                if (wrap)
                {
                    // replace row0 with old rowMax
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        Array.Copy(tempRow, x * assist.TileSize, shiftedData,
                            GameBoardData.CoordinateToRawIndex(
                                (x, 0), assist.Width), assist.TileSize);
                    }
                }
                else
                {
                    // replace row0 with empty tiles
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        shiftedData[GameBoardData.CoordinateToRawIndex(
                            (x, 0), assist.Width)] = byte.MinValue;
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
                            tempRow, x * assist.TileSize, assist.TileSize);
                    }
                }
                // shift data down for all rows except rowMax
                for (byte x = 0; x < xMaxTileCoord; x++)
                {
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(m_Board.GetTile(x, (byte)(y + 1)), 0,
                            shiftedData, GameBoardData.CoordinateToRawIndex(
                                (x, y), assist.Width), assist.TileSize);
                    }
                }
                if (wrap)
                {
                    // replace rowMax with old row0
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        Array.Copy(tempRow, x * assist.TileSize, shiftedData,
                            GameBoardData.CoordinateToRawIndex(
                                (x, yMaxTileCoord), assist.Width), 
                            assist.TileSize);
                    }
                }
                else
                {
                    // replace rowMax with empty tiles
                    for (byte x = 0; x < xMaxTileCoord; x++)
                    {
                        shiftedData[GameBoardData.CoordinateToRawIndex(
                            (x, yMaxTileCoord), assist.Width)] = byte.MinValue;
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
                            tempCol, y * assist.TileSize, assist.TileSize);
                    }
                }
                // shift data left for all cols except colMax
                for (byte x = 0; x < xMaxTileCoord; x++)
                {
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(m_Board.GetTile((byte)(x + 1), y), 0,
                            shiftedData, GameBoardData.CoordinateToRawIndex(
                                (x, y), assist.Width), assist.TileSize);
                    }
                }
                if (wrap)
                {
                    // replace colMax with old col0
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(tempRow, y * assist.TileSize, shiftedData,
                            GameBoardData.CoordinateToRawIndex(
                                (xMaxTileCoord, y), assist.Width), 
                            assist.TileSize);
                    }
                }
                else
                {
                    // replace colMax with empty tiles
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        shiftedData[GameBoardData.CoordinateToRawIndex(
                            (xMaxTileCoord, y), assist.Width)] = byte.MinValue;
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
                            tempCol, y * assist.TileSize, assist.TileSize);
                    }
                }
                // shift data right for all cols except col0
                for (byte x = xMaxTileCoord; x > 0; x--)
                {
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(m_Board.GetTile((byte)(x - 1), y), 0,
                            shiftedData, GameBoardData.CoordinateToRawIndex(
                                (x, y), assist.Width), assist.TileSize);
                    }
                }
                if (wrap)
                {
                    //replace left row with old right row
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        Array.Copy(tempRow, y * assist.TileSize, shiftedData,
                            GameBoardData.CoordinateToRawIndex(
                                (0, y), assist.Width), assist.TileSize);
                    }
                }
                else
                {
                    // replace col0 with empty tiles
                    for (byte y = 0; y < yMaxTileCoord; y++)
                    {
                        shiftedData[GameBoardData.CoordinateToRawIndex(
                            (0, y), assist.Width)] = byte.MinValue;
                    }
                }
                break;
        }

        m_Board = new GameBoard(shiftedData);

        quantity--;
        if (quantity > 0)
        {
            ShiftBoard(shiftStyle, wrap, quantity);
        }

        SaveEditState();
    }
    #endregion

    #region Basic GameBoard Editing
    public byte[] GetTile(ushort index)
    {
        return m_Board.GetTile(index);
    }

    public byte[] GetTile(byte xCoordinate, byte yCoordinate)
    {
        return m_Board.GetTile(xCoordinate, yCoordinate);
    }

    public void SetTile(byte[] setTile, ushort index)
    {
        m_Board.PlaceTile(setTile, index);

        SaveEditState();
    }

    public void SetTile(byte[] setTile, byte xCoordinate, byte yCoordinate)
    {
        m_Board.PlaceTile(setTile, xCoordinate, yCoordinate);

        SaveEditState();
    }

    public void ClearTile(ushort index)
    {
        SetTile(new byte[m_Board.TileSize], index);

        SaveEditState();
    }
    #endregion

    #region Multi-Select

    public void AddToMultiSelect((byte, byte)[] indexes)
    {
        for (int i = 0; i < indexes.Length; i++)
        {
            m_MultiSelect.Add(GameBoardData.CoordinateToTileIndex(
                indexes[i], m_Board.Width, m_Board.TileSize));
        }

        SaveEditState();
    }
    public void AddToMultiSelect(byte[] indexes)
    {
        for (int i = 0; i < indexes.Length; i++)
        {
            m_MultiSelect.Add(indexes[i]);
        }

        SaveEditState();
    }
    public void RemoveFromMultiSelect((byte, byte)[] indexes)
    {
        for (int i = 0; i < indexes.Length; i++)
        {
            m_MultiSelect.Remove(GameBoardData.CoordinateToTileIndex(
                indexes[i], m_Board.Width, m_Board.TileSize));
        }

        SaveEditState();
    }
    public void ClearMultiSelect()
    {
        m_MultiSelect.Clear();

        SaveEditState();
    }
    #endregion

    #region Paint Brushes
    public enum BrushShape { Circle, Square }
    public struct PaintBrushSettings
    {
        public BrushShape brushShape;
        public byte brushSize;

        public PaintBrushSettings(BrushShape shape, byte size)
        {
            brushShape = shape;
            brushSize = size;
        }
    }

    public void SetBrush(BrushShape shape)
    {
        m_PaintBrushSettings.brushShape = shape;
    }

    public void SetSize(byte size)
    {
        m_PaintBrushSettings.brushSize = size;
    }

    public void Paint(ushort centerIndex)
    {
        byte radius = m_PaintBrushSettings.brushSize;
        (byte, byte) coords = GameBoardData.TileIndexToCoordinate(centerIndex, m_Board.Width);
        switch (m_PaintBrushSettings.brushShape)
        {
            case BrushShape.Circle:
                for (int x = -radius; x < radius; x++)
                {
                    for (int y = -radius; y < radius; y++)
                    {
                        if (x + coords.Item1 < 0 || y + coords.Item2 < 0)
                            continue;
                        
                        ushort index =
                                GameBoardData.CoordinateToTileIndex(
                                ((byte)x, (byte)y),
                                m_Board.Width, m_Board.TileSize);
                        
                        // Honor painting inside the lines for selections
                        if (m_MultiSelect.Count > 0 &&
                            !m_MultiSelect.Contains(index)) continue;
                        
                        if (x * x + y * y <= radius * radius)
                        {
                            m_BrushTempSelect.Add(index);
                        }
                    }
                }
                break;

            case BrushShape.Square:
                for (int x = -radius; x < radius; x++)
                {
                    for (int y = -radius; y < radius; y++)
                    {
                        if (x + coords.Item1 < 0 || y + coords.Item2 < 0)
                            continue;
                        
                        ushort index =
                            GameBoardData.CoordinateToTileIndex(
                            ((byte)x, (byte)y),
                            m_Board.Width, m_Board.TileSize);
                        
                        // Honor painting inside the lines for selections
                        if (m_MultiSelect.Count > 0 &&
                            !m_MultiSelect.Contains(index)) continue;
                        
                        m_BrushTempSelect.Add(index);
                    }
                }
                break;
        }
    }

    // store brush size/radius and shape
    #endregion

    #region Fill
    public void Fill(byte[] fillTile, ushort[] indexesToFill)
    {
        for (int i = 0; i < indexesToFill.Length; i++)
        {
            m_Board.PlaceTile(fillTile, indexesToFill[i]);
        }

        SaveEditState();
    }
    public void Fill(byte[] fillTile, (byte, byte)[] coordinatesToFill)
    {
        for (int i = 0; i < coordinatesToFill.Length; i++)
        {
            m_Board.PlaceTile(fillTile, coordinatesToFill[i].Item1,
                coordinatesToFill[i].Item2);
        }

        SaveEditState();
    }
    #endregion
}