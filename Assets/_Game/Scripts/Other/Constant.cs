using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{
    public static string CURRENT_LEVEL_ID = "Current Level ID";
    public static string LEVEL_PATH = "Levels/Level ";
    public static string DAILY_CHALENGE_LEVEL = "DailyChalengeLevels/Level ";
    public static int frameRate = 60;


    //----------- Camera -----------------
    public static float SIZE_CAMERA = 6f;

    //----------- Destroy Trigger --------
    public static float WIDTH_SIZE_TRIGGER_BOX = 2f;
    public static float HEIGHT_SIZE_TRIGGER_BOX = 14f;
    public static float VERTICAL_POSITION_TRIGGER_BOX = 4f;
    public static float HORIZONTAL_POSITION_TRIGGER_BOX = 2.9f;

    //----------- Path -------------------

    public static string PATH_SETTINGS_POPUP = "Assers/_Game/Prefabs/Popups/SettingsPopup.asset";
    public static string PATH_WIN_POPUP = "Assers/_Game/Prefabs/Popups/WinPopup.asset";
    public static string PATH_LOSE_POPUP = "Assers/_Game/Prefabs/Popups/LosePopup.asset";
    public static string PATH_NEWSKIN_POPUP = "Assers/_Game/Prefabs/Popups/NewSkinPopup.asset";

    //----------- Popup ------------------

    //public static GameObject SETTINGS_POPUP = AssetDatabase.LoadAssetAtPath<GameObject>(PATH_SETTINGS_POPUP);
    //public static GameObject WIN_POPUP = AssetDatabase.LoadAssetAtPath<GameObject>(PATH_WIN_POPUP);
    //public static GameObject LOSE_POPUP = AssetDatabase.LoadAssetAtPath<GameObject>(PATH_LOSE_POPUP);
    //public static GameObject NEWSKIN_POPUP = AssetDatabase.LoadAssetAtPath<GameObject>(PATH_NEWSKIN_POPUP);

    //----------- CarController
    public static bool IsCarHandlingMouseUp = false;
    public static float speed = 0.5f;

    //----------- Screen ------------------
    public static float screenWidth = 1080f;
    public static float screenHeight = 1920f;
    //----------- Skin --------------------
    public static int SKIN_DEFAULT = 0;
    public static int SKIN_AMBULANCE = 2;
    public static int SKIN_UFO = 7;
    public static int SKIN_ROCKET = 6;
    //----------- Other -------------------
    public static float luckyTimeCountDown = 1800f;
    public static float giftTimeCountDown = 86400f;

    //----------- Action ------------------

    //----------- Function ----------------

    public static bool CantWin()
    {
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
        Vector2 currentMapSize = new Vector2(currentBoxes.Count, currentBoxes[0].Count);

        for (int x = 0; x < currentBoxes.Count; x++)
        {
            for (int y = 0; y < currentBoxes[x].Count; y++)
            {
                switch (currentBoxes[x][y].type)
                {
                    case BlockType.Up:
                        if (!currentBoxes[x][y].isLockedByKey && !currentBoxes[x][y].isLockedItSelf)
                            if (y - 1 < 0 || currentBoxes[x][y - 1].type == BlockType.Empty || currentBoxes[x][y - 1].isMoving)
                            {
                                return false;
                            }
                        break;
                    case BlockType.Down:
                        if (!currentBoxes[x][y].isLockedByKey && !currentBoxes[x][y].isLockedItSelf)
                            if (y + 1 >= currentMapSize.y || currentBoxes[x][y + 1].type == BlockType.Empty || currentBoxes[x][y + 1].isMoving)
                            {
                                return false;
                            }
                        break;
                    case BlockType.Left:
                        if (!currentBoxes[x][y].isLockedByKey && !currentBoxes[x][y].isLockedItSelf)
                            if (x - 1 < 0 || currentBoxes[x - 1][y].type == BlockType.Empty || currentBoxes[x - 1][y].isMoving)
                            {
                                return false;
                            }
                        break;
                    case BlockType.Right:
                        if (!currentBoxes[x][y].isLockedByKey && !currentBoxes[x][y].isLockedItSelf)
                            if (x + 1 >= currentMapSize.x || currentBoxes[x + 1][y].type == BlockType.Empty || currentBoxes[x + 1][y].isMoving)
                            {
                                return false;
                            }
                        break;
                }
            }
        }
        return true;
    }

    public static bool Win()
    {
#if EDITORMODE
        List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
#endif
        for (int x = 0; x < currentBoxes.Count; x++)
        {
            for (int y = 0; y < currentBoxes[x].Count; y++)
            {
                Debug.Log(" x CarController: box(" + x + ",-" + y + ") : " + currentBoxes[x][y].type + " - isMoving = " + currentBoxes[x][y].isMoving + Time.time);
                if (currentBoxes[x][y].isMoving) return false;
                if (currentBoxes[x][y].type != BlockType.Empty)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool Lose()
    {
#if EDITORMODE
        List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
        return false;
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
        if (GameplayCanvasController.instance.currentNumOfMoves <= 0)
        {
            if (HasAtLeastACarDoesntOut(currentBoxes))
                return true;
        }
        return false;
#endif
    }

    public static bool HasAtLeastACarDoesntOut(List<List<Box>> currentBoxes)
    {
        int count = 0;
        for (int x = 0; x < currentBoxes.Count; x++)
        {
            for (int y = 0; y < currentBoxes[x].Count; y++)
            {
                Debug.Log("CarController: - box( " + x + ", -" + y + ").type = " + currentBoxes[x][y].type + " - isMoving = " + currentBoxes[x][y].isMoving + " - " + Time.time);
                if (!currentBoxes[x][y].isMoving && currentBoxes[x][y].type != BlockType.Empty)
                    count++;
            }
        }
        return count >= 1;
    }


    public static float RedBlinkListCar(float delayTime)
    {
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
        Vector2 currentMapSize = new Vector2(currentBoxes.Count, currentBoxes[0].Count);

        List<Box> listBoxes = new List<Box>();
        for (int x = 0; x < currentMapSize.x; x++)
        {
            for (int y = 0; y < currentMapSize.y; y++)
            {
                if (currentBoxes[x][y].type != BlockType.Empty) listBoxes.Add(currentBoxes[x][y]);
            }
        }
        delayTime = 0;
        for (int i = 0; i < listBoxes.Count; i++)
        {
            SpriteRenderer carSR = listBoxes[i].GetTransform().GetChild(0).GetComponent<SpriteRenderer>();
            carSR.DOColor(Color.red, 0.2f).SetDelay(delayTime).OnComplete(() =>
            {
                carSR.DOColor(Color.white, 0.2f).OnComplete(() =>
                {
                    carSR.DOKill();
                });
            });
            delayTime += 0.05f;
        }
        return delayTime;
    }
    public static void Vibrate()
    {
        if (SystemInfo.supportsVibration)
        {
            Handheld.Vibrate();
        }
    }
}

[System.Serializable]
public enum Direction
{
    Up = 0,
    Down, Left, Right
}

[System.Serializable]
public enum Obstacle
{
    Default = 0,
    Bomb,
}

public enum State
{
    Play = 0,
    Pause,
    Lose,
    Win,
}

public enum SceneID
{
    Idle,
    Loading,
    Age,
    Home,
    Gameplay,
    Daily,
}

public enum BlockType
{
    Empty,
    Left,
    Right,
    Down,
    Up,
}

public enum SoundID
{
    DrivingAwayDefault = 0,
    DrivingAwayRocket,
    DrivingAwayAmbulance,
    DrivingAwayUFO,
    ClickingHiddenBox,
    ClickingHiddenIronBox,
    UnlockingHiddenBox,
    UnlocikingHiddenIronBox,
    BoosterBomb,
    BoosterHammer,
    Collision,
    Claiming,
    Win,
    Lose,
    TappingButton,
    Spinning,
}

public enum NotiID
{
    Daily,
    Spin,
    Shop,
}
