#if EDITORMODE
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Unity.VisualScripting;

#endif
#if EDITORMODE
public class MapEditor : MonoBehaviour
{

    #region Singleton
    public static MapEditor Instance;

    [ExecuteAlways]
    private void Awake()
    {
        Debug.Log("Awake");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public List<List<Box>> currentBoxes = new List<List<Box>>();
    #region MAP_PARAMS
    private const float TileSize = 20;

    private const int mapSizeMin = 2;

    private const int mapSizeMax = 20;
    private Vector2Int currentMapSize = new Vector2Int(-1, -1);
    private bool firstEdit = true;

    private Box[,] editedBoxes;

    private BlockType currentBlockType = BlockType.Empty;

    private static readonly Color[] NumberColors = new Color[13]
    {
            new Color32(236, 212, 212, 200),		// Empty Block
			new Color32(150, 146, 146, 200),		// Random Block
			new Color32(248, 172, 97, 200),		// 
			new Color32(251, 250, 153, 200),		// Up Block
			new Color32(0, 255, 0, 200),		// Down Block
			new Color32(255, 0, 0, 200),		// Right Block
			new Color32(0, 0, 200, 200),		// Left Block
			new Color32(0, 255, 255, 200),		// ItSelf Locked Block
			new Color32(255, 215, 0, 200),		// 
			new Color32(192, 255, 62, 200),		// 
			new Color32(255, 20, 147, 200),		// 
			new Color32(28, 134, 238, 200),		// 
			new Color32(160, 82, 45, 200),		// 
    };
    private bool onCreateNewMapTab = true;
    #endregion

    #region Create New Map
    [FoldoutGroup("Map Editor/Create New Map/Trigger Box to Bind")]
    [SerializeField] private GameObject rightTrigger, leftTrigger, upTrigger, downTrigger;
    [FoldoutGroup("Map Editor/Create New Map/Prefabs to Instanitiate")]
    [SerializeField] private GameObject boxPrefab;
    [TabGroup("Map Editor", "Create New Map")]
    [Header("Level")]
    [SerializeField] private int levelNumber;
    [TabGroup("Map Editor", "Create New Map")]
    [SerializeField] private int newNumOfMoves;
    [SerializeField] public GameObject levelContainer = null;
    [SerializeField] public bool isLockedByKey;
    [SerializeField] private bool isLockedItSelf;
    [SerializeField] private bool hasKey;
    [SerializeField] private bool hasBomb;
    [SerializeField] private int lockedCountDown;
    [SerializeField] private int bombCountDown;
    //=====================MAP CUSTOM=======================
    [Header("Map Custom")]
    [TabGroup("Map Editor", "Create New Map")]
    [CustomValueDrawer("CustomMapSizeRange")]
    public Vector2Int CustomMapSize;

    [HorizontalGroup("Map Editor/Create New Map/Pickable")]
    [VerticalGroup("Map Editor/Create New Map/Pickable/Left")]
    [ShowInInspector]
    [ReadOnly]
    [Space(10)]
    private static Color CurrentBlock = NumberColors[0];
    //[VerticalGroup("Map Editor/Create New Map/Pickable/Right")]
    //[Button]
    //[PropertySpace(10)]

    //public void Restart()
    //{
    //    for (int x = 0; x < currentMapSize.x; x++)
    //    {
    //        for (int y = 0; y < currentMapSize.y; y++)
    //        {
    //            editedBoxes[x, y] = new Box()
    //            {
    //                type = BlockType.Empty,
    //            };
    //        }
    //    }
    //}

    [ButtonGroup("Map Editor/Create New Map/FunctionButton")]
    public void LoadLevel()
    {
        if (boxPrefab == null)
        {
            Debug.LogError("Lack of prefabs.");
            return;
        }

        if (editedBoxes == null)
        {
            Debug.LogError("Fill in all the fields in order to generate a map.");
            return;
        }

        //TODO: Check xem đầy đủ vị trí bắt đầu / kết thúc của map chưa => Nếu chưa thì báo lỗi
        if (levelContainer != null)
        {
            DestroyImmediate(levelContainer);
        }
        levelContainer = new GameObject("Level " + levelNumber.ToString());

        for (int x = 0; x < currentMapSize.x; x++)
        {
            for (int y = 0; y < currentMapSize.y; y++)
            {
                currentBoxes[x][y] = new Box()
                {
                    type = editedBoxes[x, y].type,
                    isLockedByKey = editedBoxes[x, y].isLockedByKey,
                    isLockedItSelf = editedBoxes[x, y].isLockedItSelf,
                    hasBomb = editedBoxes[x, y].hasBomb,
                    hasKey = editedBoxes[x, y].hasKey,
                    bombCountDown = editedBoxes[x, y].bombCountDown,
                    lockedCountDown = editedBoxes[x, y].lockedCountDown,
                };
                currentBoxes[x][y].position = new Vector2(x, -y);
                Debug.Log("MapEditor: Box(" + x + "," + y + ") = type: " + currentBoxes[x][y].type + " - position: " + currentBoxes[x][y].position + " at " + Time.time);
                if (editedBoxes[x, y].type == BlockType.Empty)
                {
                    continue;
                }
                else
                {
                    Debug.Log("MapEditor: - editedBoxes[ " + x + ", " + -y + "] = " + editedBoxes[x, y].type
                        + "\n " + editedBoxes[x, y].isLockedByKey
                        + "\n " + editedBoxes[x, y].isLockedItSelf
                        + "\n " + editedBoxes[x, y].hasBomb
                        + "\n " + editedBoxes[x, y].hasKey
                        + "\n " + editedBoxes[x, y].bombCountDown
                        + "\n " + editedBoxes[x, y].lockedCountDown
                        );
                    LoadBlock(editedBoxes[x, y], new Vector3(x, -y, 0), levelContainer.transform);
                }

            }
        }
        SetUpCam();
    }

    //private void LoadBlocksOfRotBlock(BlockType type, GameObject box, int posX, int posY)
    //{
    //    switch (type)
    //    {
    //        case BlockType.Empty:
    //        case BlockType.Rotation:
    //        case BlockType.Up:
    //        case BlockType.Right:
    //        case BlockType.Down:
    //        case BlockType.Left:
    //        case BlockType.Random:
    //            box.SetActive(false);
    //            break;
    //        case BlockType.RotUp:
    //            box.GetComponent<BaseBox>().obstacle = Obstacle.Default;
    //            box.GetComponent<CarController>().direction = Direction.Up;
    //            box.transform.position = new Vector3(posX, -posY, 0);
    //            box.GetComponent<BaseBox>().SetBoxInfo(new Box()
    //            {
    //                type = type,
    //                position = new Vector2(posX, -posY)
    //            });
    //            box.SetActive(true);
    //            break;
    //        case BlockType.RotDown:
    //            box.GetComponent<BaseBox>().obstacle = Obstacle.Default;
    //            box.GetComponent<CarController>().direction = Direction.Down;
    //            box.transform.position = new Vector3(posX, -posY, 0);
    //            box.GetComponent<BaseBox>().SetBoxInfo(new Box()
    //            {
    //                type = type,
    //                position = new Vector2(posX, -posY)
    //            });
    //            box.SetActive(true);
    //            break;
    //        case BlockType.RotLeft:
    //            box.GetComponent<BaseBox>().obstacle = Obstacle.Default;
    //            box.GetComponent<CarController>().direction = Direction.Left;
    //            box.transform.position = new Vector3(posX, -posY, 0);
    //            box.GetComponent<BaseBox>().SetBoxInfo(new Box()
    //            {
    //                type = type,
    //                position = new Vector2(posX, -posY)
    //            });
    //            box.SetActive(true);
    //            break;
    //        case BlockType.RotRight:
    //            box.GetComponent<BaseBox>().obstacle = Obstacle.Default;
    //            box.GetComponent<CarController>().direction = Direction.Right;
    //            box.transform.position = new Vector3(posX, -posY, 0);
    //            box.GetComponent<BaseBox>().SetBoxInfo(new Box()
    //            {
    //                type = type,
    //                position = new Vector2(posX, -posY)
    //            });
    //            box.SetActive(true);
    //            break;
    //    }

    //}



    [ButtonGroup("Map Editor/Create New Map/FunctionButton")]
    [Button]
    public void ResetLevel()
    {
        if (levelContainer != null)
        {
            DestroyImmediate(levelContainer);
        }
        InitMap(currentMapSize.x, currentMapSize.y);
        currentBlockType = BlockType.Empty;
        CurrentBlock = NumberColors[0];
        Debug.Log("MapEditor: Reset Level " + Time.time);
    }


    [ButtonGroup("BlockColor")]
    [GUIColor(0.925f, 0.83f, 0.83f)]
    public void Empty()
    {
        _CurrentBlock = NumberColors[0];
        CurrentBlock = NumberColors[0];
        currentBlockType = BlockType.Empty;
        Debug.Log("MapEditor: Chosing Empty " + Time.time);
    }

    //[ButtonGroup("BlockColor")]
    //[GUIColor(0.59f, 0.57f, 0.57f)]
    //public void Box()
    //{
    //    CurrentBlock = NumberColors[1];
    //    currentBlockType = BlockType.Random;
    //    Debug.Log("MapEditor: Chosing Random " + Time.time);

    //}

    //[ButtonGroup("BlockColor")]
    //[GUIColor(0, 1, 1)]
    //public void Rotation()
    //{
    //    CurrentBlock = NumberColors[7];
    //    currentBlockType = BlockType.Rotation;
    //    Debug.Log("MapEditor: Chosing Rotation " + Time.time);
    //}


    [ButtonGroup("BlockColor")]
    [GUIColor(0.95f, 0.95f, 0.6f)]
    public void Up()
    {
        _CurrentBlock = NumberColors[3];
        CurrentBlock = NumberColors[3];
        currentBlockType = BlockType.Up;
        Debug.Log("MapEditor: Chosing Up " + Time.time);
    }

    [ButtonGroup("BlockColor")]
    [GUIColor(0, 1, 0)]
    public void Down()
    {
        _CurrentBlock = NumberColors[4];
        CurrentBlock = NumberColors[4];
        currentBlockType = BlockType.Down;
        Debug.Log("MapEditor: Chosing Down " + Time.time);
    }
    [ButtonGroup("BlockColor")]
    [GUIColor(1, 0, 0)]
    public void Right()
    {
        _CurrentBlock = NumberColors[5];
        CurrentBlock = NumberColors[5];
        currentBlockType = BlockType.Right;
        Debug.Log("MapEditor: Chosing Right " + Time.time);
    }
    [ButtonGroup("BlockColor")]
    [GUIColor(0, 0, 0.85f)]
    public void Left()
    {
        _CurrentBlock = NumberColors[6];
        CurrentBlock = NumberColors[6];
        currentBlockType = BlockType.Left;
        Debug.Log("MapEditor: Chosing Left " + Time.time);
    }

    private void InitMap(int sizeX, int sizeY)
    {
        onCreateNewMapTab = true;
        editedBoxes = new Box[sizeX, sizeY];
        currentBoxes = new List<List<Box>>();
        for (int x = 0; x < sizeX; x++)
        {
            List<Box> row = new List<Box>();
            for (int y = 0; y < sizeY; y++)
            {
                editedBoxes[x, y] = new Box()
                {
                    type = BlockType.Empty,
                };
                row.Add(new Box());
            }
            currentBoxes.Add(row);
        }
        Debug.Log("MapEditor: InitMap -- currentBoxes Size (x,y): " + currentBoxes.Count + ", " + currentBoxes[1].Count + " at " + Time.time);
    }

    private void ShowMapEditor()
    {
        Rect rect = EditorGUILayout.GetControlRect(true, TileSize * mapSizeMax);
        rect = rect.AlignCenter(TileSize * currentMapSize.x);

        rect = rect.AlignBottom(TileSize * currentMapSize.y);
        SirenixEditorGUI.DrawSolidRect(rect, NumberColors[0]);

        for (int x = 0; x < currentMapSize.x; x++)
        {
            for (int y = 0; y < currentMapSize.y; y++)
            {

                int i = y * currentMapSize.x + x;
                Rect tileRect = rect.SplitGrid(TileSize, TileSize, i);
                SirenixEditorGUI.DrawBorders(tileRect.SetWidth(tileRect.width + 1).SetHeight(tileRect.height + 1), 1);

                BlockType edited = BlockType.Empty;
                if (editedBoxes != null) edited = editedBoxes[x, y].type;

                //Coloring all edited block
                if (edited == BlockType.Empty)
                {
                    SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), NumberColors[0]);
                }
                if (edited == BlockType.Up)
                {
                    SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), NumberColors[3]);
                }
                if (edited == BlockType.Down)
                {
                    SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), NumberColors[4]);
                }
                if (edited == BlockType.Right)
                {
                    SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), NumberColors[5]);
                }
                if (edited == BlockType.Left)
                {
                    SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), NumberColors[6]);
                }
                //Color unedited block 
                if (tileRect.Contains(Event.current.mousePosition))
                {
                    SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), GetColorBlock(x, y));

                    if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) && Event.current.button == 0)
                    {
                        if (editedBoxes[x, y].type != currentBlockType ||
                            editedBoxes[x, y].isLockedByKey != isLockedByKey ||
                            editedBoxes[x, y].isLockedItSelf != isLockedItSelf ||
                            editedBoxes[x, y].hasKey != hasKey ||
                            editedBoxes[x, y].hasBomb != hasBomb ||
                            editedBoxes[x, y].lockedCountDown != lockedCountDown ||
                            editedBoxes[x, y].bombCountDown != bombCountDown)
                        {
                            editedBoxes[x, y].type = currentBlockType;
                            editedBoxes[x, y].isLockedByKey = isLockedByKey;
                            editedBoxes[x, y].isLockedItSelf = isLockedItSelf;
                            editedBoxes[x, y].hasKey = hasKey;
                            editedBoxes[x, y].hasBomb = hasBomb;
                            editedBoxes[x, y].lockedCountDown = lockedCountDown;
                            editedBoxes[x, y].bombCountDown = bombCountDown;
                            //Load or Reload
                            if (onCreateNewMapTab)
                                LoadLevel();
                            else
                                ReloadLevel();
                        }
                        Event.current.Use();
                    }
                }
            }
        }

        GUIHelper.RequestRepaint();
    }



    private Vector2Int CustomMapSizeRange(Vector2Int size, GUIContent label)
    {
        //Vector2 newSize = EditorGUILayout.Vector2IntField(label, new Vector2Int((int)size.x, (int)size.y));
        var newSizeX = EditorGUILayout.IntSlider(label, (int)size.x, mapSizeMin, mapSizeMax);
        var newSizeY = EditorGUILayout.IntSlider(label, (int)size.y, mapSizeMin, mapSizeMax);

        if (firstEdit)
        {
            firstEdit = false;
            currentMapSize.x = mapSizeMin;
            currentMapSize.y = mapSizeMin;
            InitMap(currentMapSize.x, currentMapSize.y);
            ShowMapEditor();

        }
        else
        {
            if (currentMapSize.x != newSizeX || currentMapSize.y != newSizeY)
            {
                currentMapSize.x = (int)newSizeX;
                currentMapSize.y = (int)newSizeY;
                InitMap(currentMapSize.x, currentMapSize.y);
                ShowMapEditor();
            }
            else
            {
                ShowMapEditor();
            }
        }
        return new Vector2Int(newSizeX, newSizeY);
    }
    #endregion


    #region Edit Map 
    private bool isEditingMap = false;
    [Header("Level")]
    [TabGroup("Map Editor", "Edit An Existing Map")]
    [ShowInInspector]
    [AssetsOnly]
    [InlineEditor(InlineEditorModes.GUIOnly)]
    [InlineButton("StartEditMap")]
    private Level levelToEdit;
    [TabGroup("Map Editor", "Edit An Existing Map")]
    [SerializeField] private int currentNumOfMoves;

    [Header("Map Custom")]
    [ShowIf("isEditingMap")]
    [TabGroup("Map Editor", "Edit An Existing Map")]
    [CustomValueDrawer("CustomEditMapSizeRange")]
    public Vector2Int EditMapSize;

    [HorizontalGroup("Map Editor/Edit An Existing Map/Pickable")]
    [VerticalGroup("Map Editor/Edit An Existing Map/Pickable/Left")]
    [ShowInInspector]
    [ReadOnly]
    [Space(10)]
    private static Color _CurrentBlock = NumberColors[0];
    //[VerticalGroup("Map Editor/Edit An Existing Map/Pickable/Right")]
    //[Button]
    //[PropertySpace(10)]

    //public void _Restart()
    //{
    //    for (int x = 0; x < currentMapSize.x; x++)
    //    {
    //        for (int y = 0; y < currentMapSize.y; y++)
    //        {
    //            editedBoxes[x, y] = new Box()
    //            {
    //                type = BlockType.Empty,
    //            };
    //        }
    //    }
    //}

    [TabGroup("Map Editor", "Edit An Existing Map")]
    [Button]
    public void ReloadLevel()
    {
        if (levelContainer != null)
        {
            DestroyImmediate(levelContainer);
        }

        if (levelToEdit == null)
        {
            Debug.LogError("Don't have any Levels in asset. Let's create a Level!");
        }

        levelContainer = new GameObject(levelToEdit.name);

        for (int x = 0; x < currentMapSize.x; x++)
        {
            for (int y = 0; y < currentMapSize.y; y++)
            {
                currentBoxes[x][y] = new Box()
                {
                    type = editedBoxes[x, y].type,
                    isLockedByKey = editedBoxes[x, y].isLockedByKey,
                    isLockedItSelf = editedBoxes[x, y].isLockedItSelf,
                    hasBomb = editedBoxes[x, y].hasBomb,
                    hasKey = editedBoxes[x, y].hasKey,
                    bombCountDown = editedBoxes[x, y].bombCountDown,
                    lockedCountDown = editedBoxes[x, y].lockedCountDown,
                };
                currentBoxes[x][y].position = new Vector2(x, -y);
                Debug.Log("MapEditor: Box(" + x + "," + y + ") = type: " + currentBoxes[x][y].type + " - position: " + currentBoxes[x][y].position + " at " + Time.time);
                if (editedBoxes[x, y].type == BlockType.Empty)
                {
                    continue;
                }
                else
                {
                    LoadBlock(editedBoxes[x, y], new Vector3(x, -y, 0), levelContainer.transform);
                }
            }
        }
        SetUpCam();
    }

    private void StartEditMap()
    {
        if (levelToEdit == null)
        {
            isEditingMap = false;
            Debug.LogError("Choose a level to edit !!!");
            return;
        }
        else
        {
            EditMapSize = levelToEdit.MapSize;
            currentMapSize = EditMapSize;
            currentNumOfMoves = levelToEdit.NumberOfMoves;
            editedBoxes = ConvertArray2DBlockToEnumArray(levelToEdit.MapEnumBlock);
            isEditingMap = true;
            currentBoxes = new List<List<Box>>();
            for (int x = 0; x < currentMapSize.x; x++)
            {
                List<Box> row = new List<Box>();
                for (int y = 0; y < currentMapSize.y; y++)
                {
                    row.Add(new Box());
                }
                currentBoxes.Add(row);
            }
        }
        onCreateNewMapTab = false;
    }

    private Vector2Int CustomEditMapSizeRange(Vector2Int size, GUIContent label)
    {
        var newSizeX = EditorGUILayout.IntSlider(label, (int)size.x, mapSizeMin, mapSizeMax);
        var newSizeY = EditorGUILayout.IntSlider(label, (int)size.y, mapSizeMin, mapSizeMax);

        if (isEditingMap)
        {
            if (currentMapSize.x != newSizeX || currentMapSize.y != newSizeY)
            {
                InitMap(newSizeX, newSizeY);
                currentMapSize.x = (int)newSizeX;
                currentMapSize.y = (int)newSizeY;
                ShowMapEditor();
            }
            else
            {
                ShowMapEditor();
            }
        }

        return new Vector2Int(newSizeX, newSizeY);
    }

    #endregion
    [ButtonGroup("Map Editor/Create New Map/Save&Del")]
    public void SaveNewLevel()
    {
        if (levelContainer == null)
        {
            Debug.LogError("A map should be created first.");
            return;
        }
        string localPath = "Assets/Resources/Levels/" + levelContainer.name + ".asset";
        Level level = ScriptableObject.CreateInstance<Level>();
        level.MapSize = currentMapSize;
        level.NumberOfMoves = newNumOfMoves;
        Array2DBlock ar2B = ConvertEnumToArray2DBlock(editedBoxes);
        level.MapEnumBlock = ar2B;

        AssetDatabase.CreateAsset(level, localPath);
        AssetDatabase.SaveAssets();
        levelNumber++;

    }

    [ButtonGroup("Map Editor/Edit An Existing Map/Save&Del")]
    public void SaveCurrentLevel()
    {

        if (levelContainer == null)
        {
            Debug.LogError("A map should be created first.");
            return;
        }

        string localPath = "Assets/Resources/Levels/" + levelContainer.name + ".asset";

        AssetDatabase.DeleteAsset(localPath);

        Level level = ScriptableObject.CreateInstance<Level>();
        level.MapSize = currentMapSize;
        level.NumberOfMoves = currentNumOfMoves;

        Array2DBlock ar2B = ConvertEnumToArray2DBlock(editedBoxes);
        level.MapEnumBlock = ar2B;

        AssetDatabase.CreateAsset(level, localPath);
        AssetDatabase.SaveAssets();

    }

    [ButtonGroup("SameBtnGroup")]
    public void DeleteLevel()
    {
        if (levelContainer == null)
        {
            Debug.LogError("A map should be created first.");
            return;
        }

        string localPath = "Assets/Resources/Levels/" + levelContainer.name + ".asset";
        AssetDatabase.DeleteAsset(localPath);
    }
    private Color GetColorBlock(int x, int y)
    {
        BlockType clickBlock = BlockType.Empty;
        Color curblockColor = NumberColors[0];

        if (editedBoxes != null) clickBlock = editedBoxes[x, y].type;

        switch (clickBlock)
        {
            case BlockType.Empty:
                curblockColor = NumberColors[0];
                break;
            case BlockType.Up:
                curblockColor = NumberColors[3];
                break;
            case BlockType.Down:
                curblockColor = NumberColors[4];
                break;
            case BlockType.Right:
                curblockColor = NumberColors[5];
                break;
            case BlockType.Left:
                curblockColor = NumberColors[6];
                break;
        }

        curblockColor.a = 255;
        return curblockColor;
    }


    private void ResetBoxElements()
    {
        Debug.Log("MapEditor: - ResetBoxElements");
        isLockedByKey = false;
        isLockedItSelf = false;
        hasKey = false;
        hasBomb = false;
        bombCountDown = 0;
        lockedCountDown = 0;
    }
    private Array2DBlock ConvertEnumToArray2DBlock(Box[,] enumToConvert)
    {
        Array2DBlock array2DBlock = new Array2DBlock(Mathf.Max(enumToConvert.GetLength(0), enumToConvert.GetLength(1)));

        Vector2Int sizeMap = new Vector2Int(enumToConvert.GetLength(0), enumToConvert.GetLength(1));
        array2DBlock.GridSize = sizeMap;

        for (int i = 0; i < enumToConvert.GetLength(0); i++)
        {
            for (int j = 0; j < enumToConvert.GetLength(1); j++)
            {
                array2DBlock.SetCell(i, j, enumToConvert[i, j]);
            }
        }

        return array2DBlock;
    }

    private void LoadBlock(Box currentBox, Vector3 position, Transform parent)
    {
        GameObject spawnObj = boxPrefab;

        switch (currentBox.type)
        {
            case BlockType.Up:
                spawnObj.GetComponent<BaseBox>().obstacle = Obstacle.Default;
                spawnObj.GetComponent<CarController>().direction = Direction.Up;
                break;
            case BlockType.Down:
                spawnObj.GetComponent<BaseBox>().obstacle = Obstacle.Default;
                spawnObj.GetComponent<CarController>().direction = Direction.Down;
                break;
            case BlockType.Left:
                spawnObj.GetComponent<BaseBox>().obstacle = Obstacle.Default;
                spawnObj.GetComponent<CarController>().direction = Direction.Left;
                break;
            case BlockType.Right:
                spawnObj.GetComponent<BaseBox>().obstacle = Obstacle.Default;
                spawnObj.GetComponent<CarController>().direction = Direction.Right;
                break;
        }

        BaseBox baseBoxComponent = spawnObj.GetComponent<BaseBox>();
        Box boxInfo = new Box()
        {
            type = currentBox.type,
            isLockedItSelf = currentBox.isLockedItSelf,
            isLockedByKey = currentBox.isLockedByKey,
            hasBomb = currentBox.hasBomb,
            hasKey = currentBox.hasKey,
            bombCountDown = currentBox.bombCountDown,
            lockedCountDown = currentBox.lockedCountDown,
            position = new Vector2(position.x, position.y),
        };
        baseBoxComponent.SetBoxInfo(boxInfo);

        GameObject obj = Instantiate(spawnObj, position, Quaternion.identity, parent);
        obj.name = position.x + ", " + position.y;
    }

    private Box[,] ConvertArray2DBlockToEnumArray(Array2DBlock arrayToConvert)
    {
        var cells = arrayToConvert.GetCells();
        int cellSizeX = cells.GetLength(1);
        int cellSizeY = cells.GetLength(0);

        Box[,] enumArray = new Box[cellSizeX, cellSizeY];

        for (int i = 0; i < enumArray.GetLength(0); i++)
        {
            for (int j = 0; j < enumArray.GetLength(1); j++)
            {
                enumArray[i, j] = cells[j, i];
            }
        }

        return enumArray;
    }

    void SetUpCam()
    {
        Camera.main.transform.position = new Vector3((float)(currentMapSize.x - 1) / 2, (float)(currentMapSize.y - 1) / (-4), -10f);
        float sizeCamera = (currentMapSize.x > Constant.SIZE_CAMERA || currentMapSize.y > (Constant.SIZE_CAMERA + 1f)) ? Mathf.Max(currentMapSize.x, currentMapSize.y) : Constant.SIZE_CAMERA;
        Camera.main.orthographicSize = sizeCamera;

        //this script to set up main camera and destroy trigger box
        rightTrigger.transform.localPosition = new Vector3(sizeCamera * Constant.HORIZONTAL_POSITION_TRIGGER_BOX / Constant.SIZE_CAMERA, 0, 0);
        rightTrigger.GetComponent<BoxCollider2D>().size = new Vector2(Constant.WIDTH_SIZE_TRIGGER_BOX, sizeCamera / Constant.SIZE_CAMERA * Constant.HEIGHT_SIZE_TRIGGER_BOX);
        leftTrigger.transform.localPosition = new Vector3(sizeCamera * (-Constant.HORIZONTAL_POSITION_TRIGGER_BOX) / Constant.SIZE_CAMERA, 0, 0);
        leftTrigger.GetComponent<BoxCollider2D>().size = new Vector2(Constant.WIDTH_SIZE_TRIGGER_BOX, sizeCamera / Constant.SIZE_CAMERA * Constant.HEIGHT_SIZE_TRIGGER_BOX);
        upTrigger.transform.localPosition = new Vector3(0, sizeCamera * Constant.VERTICAL_POSITION_TRIGGER_BOX / Constant.SIZE_CAMERA, 0);
        upTrigger.GetComponent<BoxCollider2D>().size = new Vector2(sizeCamera / Constant.SIZE_CAMERA * Constant.HEIGHT_SIZE_TRIGGER_BOX, Constant.WIDTH_SIZE_TRIGGER_BOX);
        downTrigger.transform.localPosition = new Vector3(0, sizeCamera * (-Constant.VERTICAL_POSITION_TRIGGER_BOX) / Constant.SIZE_CAMERA, 0);
        downTrigger.GetComponent<BoxCollider2D>().size = new Vector2(sizeCamera / Constant.SIZE_CAMERA * Constant.HEIGHT_SIZE_TRIGGER_BOX, Constant.WIDTH_SIZE_TRIGGER_BOX);
    }

    public void DirectionRandom(CarController boxController)
    {
        boxController.direction = (Direction)Random.Range(0, 4);
    }
}
#endif