using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DataMananger : MonoBehaviour
{
    public static DataMananger instance;

    public GameSave gameSave;
    private GameSave gameSaveBackUp;

    private bool isLoad = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnApplicationPause(bool pause)
    {
        SaveGame();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void LoadData()
    {
        if (!isLoad)
        {
            if (PlayerPrefs.HasKey("GameSave")) gameSave = JsonUtility.FromJson<GameSave>(PlayerPrefs.GetString("GameSave"));
            if (gameSave.isNew)
            {
                gameSave = new GameSave();
                gameSave.isNew = false;
                gameSave.installTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //
            }
            else
            {

            }

            isLoad = true;
            SaveGame();
        }
    }

    public void SaveGame()
    {
        if (!isLoad) { return; }
        if (gameSave == null)
        {
            if (gameSaveBackUp != null)
            {
                gameSave = gameSaveBackUp;
                Debug.LogError("gameSave is null. Back up successful!");
            }
            else
            {
                gameSave = new GameSave();
                Debug.LogError("gameSave is null. Back up is not successful. Reset data");
            }
        }
        gameSaveBackUp = gameSave;
        PlayerPrefs.SetString("GameSave", JsonUtility.ToJson(gameSave));
        PlayerPrefs.Save();
    }

    public void UpdateSaveGame()
    {

    }
}
