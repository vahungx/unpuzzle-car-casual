using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HomeCanvasContorller : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject[] panels;

    [Header("Prefabs")]
    [SerializeField] private GameObject settingsPopupPrefab;
    [SerializeField] private GameObject dailyPopupPrefab;

    [Header("Other")]
    [SerializeField] private GameObject noAds;
    [SerializeField] private TextMeshProUGUI currentDiamonds;
    [SerializeField] private TextMeshProUGUI level;


    private bool isLoaded = false;
    #region Singleton
    public static HomeCanvasContorller instance;
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
    }
    #endregion
    private void Start()
    {
        DataMananger.instance.LoadData();
        GameManager.instance.GameState = State.Play;
        LevelLoad();
        NoAds();
    }
    private void Update()
    {
        if (isLoaded)
            DiamondsLoad();
    }

    #region LoadData
    private void LevelLoad()
    {
        int currentLevelID = DataMananger.instance.gameSave.currentLevel;
        Level currentLevel = Resources.Load<Level>(Constant.LEVEL_PATH + currentLevelID);
        if (currentLevel == null)
        {
            Debug.Log("GameplayCanvasController: Level " + currentLevelID + " is  null");
            isLoaded = false;
            return;
        }
        level.text = "Level " + (DataMananger.instance.gameSave.currentLevel + 1).ToString();
        DiamondsLoad();
        isLoaded = true;
    }
    private void NoAds()
    {
        if (DataMananger.instance.gameSave.isNoAds)
        {
            noAds.SetActive(false);
        }
    }

    private void DiamondsLoad()
    {
        currentDiamonds.text = DataMananger.instance.gameSave.currentDiamonds.ToString();
    }
    #endregion
    #region Button
    public void OnClickNoAdsBtn()
    {
        //check thanh toán 
        //if true
        DataMananger.instance.gameSave.isNoAds = true;
        DataMananger.instance.SaveGame();
        noAds.SetActive(false);
    }

    public void OnClickPanelBtn(int id)
    {
        GameManager.instance.GameState = State.Pause;
        Debug.Log("GameplayCanvasController - OnClickPanelBtn[" + id + "]");
        for (int i = 0; i < panels.Length; i++)
        {
            if (i != id)
            {
                panels[i].SetActive(false);
            }
            else panels[i].SetActive(true);
        }
    }
    public void OnClickSettings()
    {
        GameManager.instance.GameState = State.Pause;
        Instantiate(settingsPopupPrefab, transform);
    }
    public void OnClickBackBtn()
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
            }
        }
        GameManager.instance.GameState = State.Play;
    }
    public void OnClickPlay()
    {
        //StartCoroutine(LoadGamePlayScene());
        GameManager.instance.onPlayMode = true;
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }
    public void OnClickDailyChalenge()
    {

    }
    #endregion

    //IEnumerator LoadGamePlayScene()
    //{
    //    var gameplay = SceneManager.LoadSceneAsync((int)SceneID.Gameplay, LoadSceneMode.Additive);
    //    while (!gameplay.isDone) yield return new WaitForEndOfFrame();
    //    yield return new WaitForSeconds(0.5f);
    //    {
    //        DataMananger.instance.gameSave.gameMode = 0;
    //        DataMananger.instance.SaveGame();
    //        SceneManager.UnloadSceneAsync((int)SceneID.Home);
    //    }
    //}
}
