using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGameBoardEditor : MonoBehaviour
{
    #region Data
    [SerializeField]
    private byte m_MaxBoardSize = 32; // Game specific value

    GameBoardDesigner m_Designer;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        byte[] t64 = GameBoardData.Tile64(0b00001000, 0b0, 0b0, 0b0, 0b0, 0b0, 0b00000011, 0b0);
        byte[] up64 = GameBoardData.Tile64(0b00001000, 0b0, 0b0, 0b00010001, 0b00100100, 0b0, 0b00000011, 0b0);
        byte[] left64 = GameBoardData.Tile64(0b00001000, 0b0, 0b0, 0b00100010, 0b00011000, 0b0, 0b00000011, 0b0);
        byte[] down64 = GameBoardData.Tile64(0b00001000, 0b0, 0b0, 0b00010100, 0b00100001, 0b0, 0b00000011, 0b0);
        byte[] right64 = GameBoardData.Tile64(0b00001000, 0b0, 0b0, 0b00101000, 0b00010010, 0b0, 0b00000011, 0b0);

        byte[][] basic4PlayerTileLayoutT64 = {
        GameBoardData.Header(64, 8, 8, 0b10000000, 0),
        t64, up64, up64, up64, up64, up64, up64, t64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        left64, t64, t64, t64, t64, t64, t64, right64,
        t64, down64, down64, down64, down64, down64, down64, t64
        };
        //m_Board = new GameBoard("D:\\Unity\\Projects\\GamePlusPlus\\Assets\\Binaries\\Boards\\MyBoard.GameBoard");
        //m_Board.ImportBoardFile();

        //Debug.Log(m_Board.OutputBoardString + ", " + m_Board.Dimensions + "\n" + m_Board.OutputBoardString2D);
        //Debug.Log("d4 tile is " + m_Board.GetTile(GamePlusPlusCoordinateConverter.ToCoordinates["d4"]));


        string s = "";

        Debug.Log(s);

        m_Designer = new GameBoardDesigner(new GameBoard(64, 2, 2));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
