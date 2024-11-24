using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject rightTrigger, leftTrigger, upTrigger, downTrigger;
    [SerializeField] private Transform upLeft, upRight, downLeft, downRight, pathUp, pathDown;
    [SerializeField] private Camera mainCam;
    [Space]
    [SerializeField] private GameObject[] bigDecorations, smallDecrations;

    [System.Obsolete]
    void Start()
    {
        DataMananger.instance.LoadData();
        int currentLevelID = DataMananger.instance.gameSave.currentLevel;

        Level currentLevel = Resources.Load<Level>(Constant.LEVEL_PATH + currentLevelID);
        if (currentLevel == null)
        {
            Debug.Log("GameManageError: Level " + currentLevelID + " is  null");
            return;
        }

        Vector2Int currentMapSize = new Vector2Int();

        currentMapSize = currentLevel.MapSize;

        //this script to set up main camera and trigger box
        mainCam.transform.localPosition = new Vector3((float)(currentMapSize.x - 1) / 2, (float)(currentMapSize.y - 1) / (-2) + 0.5f, -10f);
        transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, 0);
        float sizeCamera = Mathf.Max(currentMapSize.x + 2, currentMapSize.y) + GetBonusSizeCammera();
        mainCam.orthographicSize = sizeCamera;

        float curMapSizeY = currentMapSize.y;
        float curMapSizeX = currentMapSize.x;

        upTrigger.transform.localPosition = new Vector3(0, 0.7f + curMapSizeY / 2, 0);
        rightTrigger.transform.localPosition = new Vector3(1.1f + curMapSizeX / 2, 0, 0);
        leftTrigger.transform.localPosition = new Vector3(-1.1f - curMapSizeX / 2, 0, 0);
        downTrigger.transform.localPosition = new Vector3(0, -1.7f - curMapSizeY / 2, 0);

        //this script to set up map elements
        upLeft.localPosition = new Vector3(-2f - curMapSizeX / 2, 3.5f + curMapSizeY / 2, 0);
        upRight.localPosition = new Vector3(2f + curMapSizeX / 2, 3.5f + curMapSizeY / 2, 0);
        downLeft.localPosition = new Vector3(-2f - curMapSizeX / 2, -4.5f - curMapSizeY / 2, 0);
        downRight.localPosition = new Vector3(2f + curMapSizeX / 2, -4.5f - curMapSizeY / 2, 0);

        pathDown.localPosition = new Vector3(0, -1.5f - (curMapSizeY + 2) / 2, 0);
        pathUp.localPosition = new Vector3(0, 0.5f + (curMapSizeY + 2) / 2, 0);

        pathDown.localScale = new Vector3(curMapSizeX / 4, curMapSizeX / 4, 0);
        pathUp.localScale = new Vector3(curMapSizeX / 4, curMapSizeX / 4, 0);

        //this function to set up decoration

        SetUpDecoration();
    }

    private float GetBonusSizeCammera()
    {
        float screenRatio = Screen.height / Screen.width;
        return screenRatio / 2;

    }
    private float SetSizeCammera(float sizeX, float sizeY)
    {
        float size = 0;

        return size;
    }

    [System.Obsolete]
    private void SetUpDecoration()
    {
        RandomDecoration(upLeft, bigDecorations, Decoration.Big, GroundDirection.Left, GroundDirection.Up);
        RandomDecoration(upLeft, smallDecrations, Decoration.Small, GroundDirection.Left, GroundDirection.Up);

        RandomDecoration(upRight, bigDecorations, Decoration.Big, GroundDirection.Right, GroundDirection.Up);
        RandomDecoration(upRight, smallDecrations, Decoration.Small, GroundDirection.Right, GroundDirection.Up);

        RandomDecoration(downLeft, bigDecorations, Decoration.Big, GroundDirection.Left, GroundDirection.Down);
        RandomDecoration(downLeft, smallDecrations, Decoration.Small, GroundDirection.Left, GroundDirection.Down);

        RandomDecoration(downRight, bigDecorations, Decoration.Big, GroundDirection.Right, GroundDirection.Down);
        RandomDecoration(downRight, smallDecrations, Decoration.Small, GroundDirection.Right, GroundDirection.Down);
    }

    [System.Obsolete]
    private void RandomDecoration(Transform parent, GameObject[] prefabs, Decoration type, GroundDirection x, GroundDirection y)
    {
        int max = 0;
        List<GameObject> decoObjs = new List<GameObject>();
        switch (type)
        {
            case Decoration.Big:
                max = Random.Range(7, 12);
                break;
            case Decoration.Small:
                max = Random.Range(3, 6);
                break;
        }
        for (int i = 0; i < max; i++)
        {
            GameObject decoPrefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject decoObj = Instantiate(decoPrefab, parent);
            decoObj.transform.localPosition = RandomDecorationPos(type, x, y);
            decoObjs.Add(decoObj);
        }
        if (type == Decoration.Small)
        {
            decoObjs = decoObjs.OrderBy(x => x.transform.localPosition.y).ToList();
            for (int i = 0; i < max; ++i)
            {
                decoObjs[i].GetComponent<SpriteRenderer>().sortingOrder = -i;
            }
        }
        else
        {
            decoObjs = decoObjs.OrderBy(x => x.transform.localPosition.y).ToList();
            decoObjs.Reverse();
            for (int i = 0; i < max; ++i)
            {
                decoObjs[i].GetComponent<SpriteRenderer>().sortingOrder = i + 1;
            }
        }
    }

    [System.Obsolete]
    private Vector3 RandomDecorationPos(Decoration type, GroundDirection xDirection, GroundDirection yDirection)
    {
        Vector3 pos = Vector3.zero;
        switch (type)
        {
            case Decoration.Big:
                pos.x = (xDirection == GroundDirection.Right) ? Random.Range(-1.3f, 1f) : Random.Range(-1f, 1.3f);
                pos.y = (yDirection == GroundDirection.Up) ? Random.Range(-3f, 2f) : Random.Range(-2f, 3f);
                break;
            case Decoration.Small:
                pos.x = (xDirection == GroundDirection.Right) ? Random.Range(-1.5f, 1f) : Random.Range(-1f, 1.5f);
                pos.y = (yDirection == GroundDirection.Up) ? Random.Range(-3.5f, 2.5f) : Random.Range(-2.5f, 3.5f);
                break;
        }
        return pos;
    }

    enum Decoration
    {
        Big,
        Small
    }

    enum GroundDirection
    {
        Up, Down, Left, Right
    }
}
