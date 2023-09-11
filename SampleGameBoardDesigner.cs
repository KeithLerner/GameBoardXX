using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class SampleGameBoardDesigner : EditorWindow
{
    #region Data
    private const ushort m_WindowSizeMax = 900;
    private const ushort m_WindowSizeMin = 512;

    GameBoardDesigner m_Designer;

    public static string DefaultPath => Application.dataPath;
    private string m_Path = "";
    private string m_FileName = "";
    private Vector2Int m_Dimensions;

    GameBoardData.GameBoardXXVariant m_TileSize;
    public byte TileSize
    {
        get
        {
            return (byte)(m_Designer != null ? m_Designer.Tile.TileSize : 8);
        }
    }
    
    private byte m_Width, m_Length;
    private byte m_MaxBoardSize = 255;
    
    private bool m_HasBoard = false;
    #endregion


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
        EditorWindow window = GetWindow<SampleGameBoardDesigner>("Sample GBXX Editor");
        window.titleContent = new GUIContent("GBXX Sample Editor");

        // Limit size of the window
        window.minSize = Vector2.one * m_WindowSizeMin;
        window.maxSize = Vector2.one * m_WindowSizeMax;
    }

    private void CreateGUI()
    {
        m_TileSize = GameBoardData.GameBoardXXVariant._08;
        m_Dimensions = Vector2Int.one;

        // Get a list of all sprites in the project
        var allObjectGuids = AssetDatabase.FindAssets("t:Sprite");
        var allObjects = new List<Sprite>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        // Create a two-pane view with the left pane being fixed with
        var mainView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // Add the view to the visual tree by adding it as a child to the root element
        rootVisualElement.Add(mainView);

        // A TwoPaneSplitView always needs exactly two child elements
        var boardPane = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
        // palette area + quick action area
        var boardBarArea = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
        // palette and quick action
        var boardMainBar = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        // palette
        var boardPalette = new ListView();
        // quick action
        var quickActions = new ListView();
        // set up
        boardPalette.makeItem = () => new Label();
        boardPalette.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        boardPalette.itemsSource = allObjects;
        quickActions.makeItem = () => new Label();
        quickActions.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
        quickActions.itemsSource = allObjects;
        boardBarArea.Add(boardPalette);
        boardBarArea.Add(quickActions);
        // toolbar
        
        
        
        // grid edit area
        var boardEditArea = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
        boardPane.Add(boardBarArea);
        boardPane.Add(boardEditArea);
        mainView.Add(boardPane);


        var tilePane = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
        // current tile inspector area
            // color + char id
        // data edit area
        tilePane.Add();
        tilePane.Add();
        mainView.Add(tilePane);
    }

    public void OnGUI()
    {
        if (m_Designer == null) m_HasBoard = false;

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

        if (GUILayout.Button("Save Game Tile"))
        {
            m_Designer.Tile.ExportBoardFile(m_Path + "\\" + m_FileName + ".cbxx");
        }

        GUILayout.Space(2);

        if (GUILayout.Button("Load Game Tile"))
        {
            m_Designer.Tile.ImportBoardFile(m_Path + "\\" + m_FileName + ".cbxx");
        }

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tile Size (bits):");
        if (m_Designer == null)
        {
            m_TileSize = (GameBoardData.GameBoardXXVariant)
                EditorGUILayout.EnumPopup(m_TileSize);
        }
        else
        {
            EditorGUILayout.SelectableLabel(TileSize.ToString());
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(2);
        GUIStyle dimensionsStyle = new GUIStyle
        {
            fixedWidth = 50f,
            wordWrap = true
        };
        EditorGUILayout.LabelField("Tile Dimensions:");
        GUILayout.BeginHorizontal(dimensionsStyle);
        m_Dimensions = EditorGUILayout.Vector2IntField("Dimensions:", m_Dimensions);
        m_Width = (byte)Mathf.Clamp(m_Dimensions.x, 0, m_MaxBoardSize);
        m_Length = (byte)Mathf.Clamp(m_Dimensions.y, 0, m_MaxBoardSize);
        if (GUILayout.Button("New Game Tile"))
        {
            m_Designer = new GameBoardDesigner();
            m_Designer.NewBoard(m_TileSize, m_Width, m_Length);
            m_HasBoard = true;
        }
        GUILayout.EndHorizontal();

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

            };
            GUIStyle tileButtonStyle = new GUIStyle
            {
                contentOffset = new Vector2(16, -16),
                fontSize = 32,
                fixedWidth = 32,
                fixedHeight = 32
            };
            for (int y = m_Length; y > 0; y--)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < m_Width; x++)
                {
                    string s = "¤";

                    byte[] tile = m_Designer.Tile.GetTile((byte)x, (byte)y);
                    byte[] p = new byte[TileSize];
                    byte[] color = new byte[4];
                    char[] name = new char[12];
                    if (m_Designer.Palette.Value != null)
                    {
                        for (int i = 0; i < m_Designer.Palette.TileCount; i++)
                        {
                            Array.Copy(m_Designer.Palette.Value, TileSize * i, p, 0, TileSize);
                            if (tile == p)
                            {
                                Array.Copy(p, TileSize, color, 0, 4);
                                Array.Copy(p, TileSize + 4, name, 0, 12);
                                tileButtonStyle.fontStyle = FontStyle.Italic;
                                tileButtonStyle.richText = true;
                                s = "<color = " + BitConverter.ToString(color).Replace("-", string.Empty) + ">" + name.ToString() + "</color>";
                                break;
                            }
                        }
                    }
                    
                    if (GUILayout.Button(s, tileButtonStyle))
                    {
                        Debug.Log((x+1, y) + " Clicked!");
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
