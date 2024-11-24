using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab, darkBoxPrefab, leftBoxPrefab, upBoxPrefab, downBoxPrefab, righBoxPrefab;
    [SerializeField] public GameObject levelContainer;
    [SerializeField] private GameObject levelBackGround;

    [HideInInspector] public int currentLevelID;
    [HideInInspector] public Level currentLevel;
    [HideInInspector] public bool isLoaded = false;
    [HideInInspector] public List<List<Box>> currentBoxes;
    [HideInInspector] public Box boxChestKey;
    #region Singleton
    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);
    }
    #endregion 

    private void Start()
    {
        LoadGameMode();
    }
    #region LoadLevel
    public void LoadLevel()

    {
        currentLevelID = DataMananger.instance.gameSave.currentLevel;

        currentLevel = Resources.Load<Level>(Constant.LEVEL_PATH + currentLevelID);
        if (currentLevel == null)
        {
            Debug.Log("GameManageError: Level " + currentLevelID + " is  null");
            isLoaded = false;
            return;
        }
        levelContainer.name = currentLevel.name;
        Vector2Int currentMapSize = new Vector2Int();

        currentMapSize = currentLevel.MapSize;

        var cells = currentLevel.MapEnumBlock.GetCells();
        int cellSizeX = cells.GetLength(1);
        int cellSizeY = cells.GetLength(0);

        Box[,] boxArray = new Box[cellSizeX, cellSizeY];

        for (int i = 0; i < boxArray.GetLength(0); i++)
        {
            for (int j = 0; j < boxArray.GetLength(1); j++)
            {
                boxArray[i, j] = cells[j, i];
            }
        }

        currentBoxes = new List<List<Box>>();
        for (int x = 0; x < currentMapSize.x; x++)
        {
            List<Box> row = new List<Box>();
            for (int y = 0; y < currentMapSize.y; y++)
            {
                row.Add(new Box()
                {
                });
            }
            currentBoxes.Add(row);
        }

        for (int x = 0; x < currentMapSize.x; x++)
        {
            for (int y = 0; y < currentMapSize.y; y++)
            {
                LoadBlack(new Vector3(x, -y, 0), levelBackGround.transform, currentMapSize);
                currentBoxes[x][y] = new Box()
                {
                    type = boxArray[x, y].type,
                    isLockedByKey = boxArray[x, y].isLockedByKey,
                    isLockedItSelf = boxArray[x, y].isLockedItSelf,
                    hasBomb = boxArray[x, y].hasBomb,
                    hasKey = boxArray[x, y].hasKey,
                    bombCountDown = boxArray[x, y].bombCountDown,
                    lockedCountDown = boxArray[x, y].lockedCountDown,
                };
                currentBoxes[x][y].position = new Vector2(x, -y);
                Debug.Log("LevelManager: Box(" + x + "," + y + ") = type: " + currentBoxes[x][y].type + " - position: " + currentBoxes[x][y].position + " at " + Time.time);
                if (boxArray[x, y].type == BlockType.Empty)
                {
                    continue;
                }
                else
                {
                    currentBoxes[x][y].SetTransform(LoadBlock(boxArray[x, y], new Vector3(x, -y, 0), levelContainer.transform).transform);
                }
            }
        }
        boxChestKey = RandomBoxChestKey();
        Debug.Log("LevelManager: ");
        isLoaded = true;
    }

    public void LoadDailyChalengeLevel()
    {
        //LoadDailyChalengeLevel
    }
    private GameObject LoadBlock(Box currentBox, Vector3 position, Transform parent)
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
        return obj;
    }

    private void LoadBlack(Vector3 position, Transform parent, Vector2 currentMapSize)
    {
        if ((position.x % 2 != 0 && -position.y % 2 == 0) || (position.x % 2 == 0 && -position.y % 2 != 0))
        {
            Instantiate(darkBoxPrefab, position, Quaternion.identity, parent);
        }
        if (position.x == 0 && -position.y % 2 == 0)
        {
            Instantiate(leftBoxPrefab, new Vector3(-1, position.y, 0), Quaternion.identity, parent);
        }
        if (position.x % 2 == 0 && -position.y == 0)
        {
            Instantiate(upBoxPrefab, new Vector3(position.x, 1, 0), Quaternion.identity, parent);
        }
        if (currentMapSize.x % 2 != 0 && -position.y % 2 == 0 || currentMapSize.x % 2 == 0 && -position.y % 2 != 0)
        {
            if (position.x == currentMapSize.x - 1)
            {
                Instantiate(righBoxPrefab, new Vector3(currentMapSize.x, position.y, 0), Quaternion.identity, parent);
            }
        }
        if (currentMapSize.y % 2 != 0 && position.x % 2 == 0 || currentMapSize.y % 2 == 0 && position.x % 2 != 0)
        {
            if (-position.y == currentMapSize.y - 1)
            {
                Instantiate(downBoxPrefab, new Vector3(position.x, -currentMapSize.y, 0), Quaternion.identity, parent);
            }
        }

    }

    private Box RandomBoxChestKey()
    {
        List<Box> listBoxes = new List<Box>();

        for (int x = 0; x < currentBoxes.Count; x++)
        {
            for (int y = 0; y < currentBoxes[x].Count; y++)
            {
                if (currentBoxes[x][y].type != BlockType.Empty &&
                    !currentBoxes[x][y].hasKey && !currentBoxes[x][y].hasBomb)
                    listBoxes.Add(currentBoxes[x][y]);
            }
        }

        return listBoxes[Random.Range(0, listBoxes.Count)];

    }

    public void LoadGameMode()
    {
        GameMode mode = (GameMode)DataMananger.instance.gameSave.gameMode;
        switch (mode)
        {
            case GameMode.Default:
                LoadLevel();
                break;
            case GameMode.DailyChalenge:
                LoadDailyChalengeLevel();
                break;
        }
    }

    #endregion
}
