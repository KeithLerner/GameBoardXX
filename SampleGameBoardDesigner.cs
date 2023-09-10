using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SampleGameBoardDesigner : EditorWindow
{
    #region Data
    GameBoardDesigner m_Designer;

    private bool m_HasBoard = false;
    private string m_Path = "";
    private string m_FileName = "";
    GameBoardData.GameBoardXXVariant m_TileSize;
    private byte m_MaxBoardSize = 255;
    private byte m_Width, m_Length;
    #endregion
    // Start is called before the first frame update
    void Test()
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

    [MenuItem("Window/GBXX Editor")]
    public static void ShowWindow()
    {
        GetWindow<SampleGameBoardDesigner>("Sample GBXX Editor");
    }

    private void OnEnable()
    {
        m_Designer = new GameBoardDesigner();
        m_TileSize = GameBoardData.GameBoardXXVariant._08;
        m_Width = 1;
        m_Length = 1;
    }

    public void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.Label("File", EditorStyles.boldLabel);

        // Text input field
        GUILayout.Label("Working Directory");
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Path:");
        m_Path = EditorGUILayout.TextField(m_Path);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("File Name:");
        m_FileName = EditorGUILayout.TextField(m_FileName);
        GUILayout.EndHorizontal();

        GUILayout.Space(2);

        if (GUILayout.Button("Save Game Board"))
        {
            m_Designer.Board.ExportBoardFile(m_Path + "\\" + m_FileName + ".cbxx");
        }

        GUILayout.Space(2);

        if (GUILayout.Button("Load Game Board"))
        {
            m_Designer.Board.ImportBoardFile(m_Path + "\\" + m_FileName + ".cbxx");
        }

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tile Size (bits):");
        m_TileSize = (GameBoardData.GameBoardXXVariant)
            EditorGUILayout.EnumPopup(m_TileSize);
        GUILayout.EndHorizontal();

        GUILayout.Space(2);

        EditorGUILayout.LabelField("Board Dimensions:");
        GUILayout.BeginHorizontal();
        m_Width = (byte)EditorGUILayout.IntField("Width:", 
            Mathf.Clamp(m_Width, 0, m_MaxBoardSize));
        m_Length = (byte)EditorGUILayout.IntField("Length:", 
            Mathf.Clamp(m_Length, 0, m_MaxBoardSize));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("New Game Board"))
        {
            m_Designer.NewBoard(m_TileSize, m_Width, m_Length);
            m_HasBoard = true;
        }

        if (m_HasBoard)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Undo"))
            {
                m_Designer.Undo();
            }
            GUILayout.Space(2);
            if (GUILayout.Button("Redo"))
            {
                m_Designer.Redo();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Edit", EditorStyles.boldLabel);

            if (GUILayout.Button("Clear Edit History"))
            {
                m_Designer.ClearEditHistory();
            }

            GUILayout.Space(10);

            // tile palette
            for (int y = 0; y < 2; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < m_Designer.Palette.TileCount / 2; x++)
                {
                    if (GUILayout.Button("+"))
                    {
                        // set active brush tile
                        Debug.Log((y * m_Designer.Palette.TileCount / 2) + x + " Clicked!");
                    }
                }
                GUILayout.EndHorizontal();
            }

            // tool selection
                // find a way to do custom icons
                    // use kenny icons?

            // edit grid
            GUILayout.Space(10);
            GUIStyle editAreaStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.BeginArea(new Rect(10, 10, 100, 100), editAreaStyle);
            GUIStyle tileButtonStyle = new GUIStyle
            {
                fixedWidth = 64,
                fixedHeight = 64
            };
            for (int y = m_Length; y > 0; y--)
            {
                GUILayout.BeginHorizontal();
                for (int x = 1; x <= m_Width; x++)
                {
                    if (GUILayout.Button("+", tileButtonStyle))
                    {
                        Debug.Log((x, y) + " Clicked!");
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}
