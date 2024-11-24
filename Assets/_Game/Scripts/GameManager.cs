using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private bool dontDestroyOnLoad = false;
    [HideInInspector] public Level currentLevel;

    [HideInInspector] public State GameState { get; set; }
    [HideInInspector] public bool onPlayMode = false;
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (dontDestroyOnLoad) { DontDestroyOnLoad(gameObject); }
    }
    #endregion
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Constant.frameRate;
        DataMananger.instance.LoadData();
        GameState = State.Play;
    }
    private void Update()
    {
    }

    #region Gameplay Follow
    #endregion

    public enum GameMode
    {
        Default,
        DailyChalenge,
    }
}
