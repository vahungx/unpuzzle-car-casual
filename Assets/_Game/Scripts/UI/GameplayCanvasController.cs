using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayCanvasController : MonoBehaviour
{
    [Header("Header")]

    [SerializeField] private TextMeshProUGUI numberOfMoves;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI diamonds;

    [Space]
    [Header("Popup")]

    [SerializeField] private GameObject settingsPopupPrefab;
    [SerializeField] private GameObject losePopupPrefab;
    [SerializeField] private GameObject winPopupPrefab;
    [SerializeField] private GameObject getBoosterPopupPrefab;
    [SerializeField] private GameObject hammerPopupPrefab;
    [SerializeField] private GameObject chestPopupPrefab;
    [SerializeField] private GameObject keyPopupPrefab;
    [SerializeField] private GameObject loadingPopupPrefab;
    [SerializeField] private GameObject noInternetPopupPrefab;

    [Space]
    [Header("Panels")]

    [SerializeField] private GameObject[] panels;
    [SerializeField] public GameObject gameplayPanel;

    [Space]
    [Header("Button")]

    [SerializeField] private ButtonExtension spinBtn;
    [SerializeField] private ButtonExtension dailyBtn, shopBtn;

    [Space]
    [Header("Booster")]

    [SerializeField] private GameObject booster;
    [SerializeField] private GameObject hammer, bomb, hammerBtn, bombBtn, hammerLight, bombLight;
    [SerializeField] private TextMeshProUGUI bombAmount, hammerAmount;

    [Space]
    [Header("Other")]

    [SerializeField] private GameObject noAds;
    [SerializeField] private GameObject newLevelComingSoon;
    [SerializeField] private GameObject[] notiIcons;

    [HideInInspector] public int currentNumOfMoves = 0;
    [HideInInspector] public bool onBombBooster, onHammerBooster;
    [HideInInspector] public Vector3 keyChestPos;
    private bool isLoaded = false;
    private bool isOnUnder6Moves = false;

    public static bool isLoad = false;
    #region Singleton
    public static GameplayCanvasController instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }
    #endregion

    private void OnEnable()
    {
        CarController.OnWin += OnWinPopupOpen;
        CarController.OnLose += OnLosePopupOpen;
        CarController.OnOutOfMoves += OnHammerPopupOpen;
        BoosterController.OnWin += OnWinPopupOpen;
        BoosterController.OnHammer += OnHammerPopupOpen;
        BaseBox.OnLose += OnLosePopupOpen;
        ChestKeyController.OnDrop += OnKeyPopupOpen;
        CarController.OnUnder6Moves += OnUnder6Moves;
        CarController.OnPlay += TurnOffBtns;
    }
    private void OnDisable()
    {
        CarController.OnLose -= OnLosePopupOpen;
        CarController.OnWin -= OnWinPopupOpen;
        CarController.OnOutOfMoves -= OnHammerPopupOpen;
        BoosterController.OnWin -= OnWinPopupOpen;
        BoosterController.OnHammer -= OnHammerPopupOpen;
        BaseBox.OnLose -= OnLosePopupOpen;
        ChestKeyController.OnDrop -= OnKeyPopupOpen;
        CarController.OnUnder6Moves -= OnUnder6Moves;
        CarController.OnPlay -= TurnOffBtns;

    }
    private void Start()
    {
        DataMananger.instance.LoadData();
        GameManager.instance.GameState = State.Play;
        LevelLoad();
        NoAds();
        gameplayPanel.SetActive(true);
        onBombBooster = false; onHammerBooster = false;
        OnLoaddingPopup();
        TurnOnNotification();
    }

    private void Update()
    {
        if (isLoaded)
        {
            UpdateCurrentMoves();
            SetUpBooster();
            DiamondsLoad();
        }
        OnUnder6Moves();
    }

    #region Open Popup

    private void OnLoaddingPopup()
    {
        if (isLoad) return;
        GameManager.instance.GameState = State.Pause;
        Instantiate(loadingPopupPrefab, transform);
        isLoad = true;
    }

    private void OnNoInternetPopupOpen()
    {
        GameManager.instance.GameState = State.Pause;
        Instantiate(noInternetPopupPrefab, transform);
    }

    private void OnWinPopupOpen()
    {
        if (GameManager.instance.GameState == State.Win)
        {
            if ((DataMananger.instance.gameSave.currentLevel + 1) % 5 == 0)
            {
                DataMananger.instance.gameSave.chestKey++;
                DataMananger.instance.SaveGame();
                if (DataMananger.instance.gameSave.chestKey == 3)
                {
                    DataMananger.instance.gameSave.chestKey = 0;
                    DataMananger.instance.SaveGame();
                    OnChestPopupOpen();
                    return;
                }
            }
            DataMananger.instance.gameSave.currentLevel++;
            DataMananger.instance.SaveGame();
            gameplayPanel.SetActive(false);
            Instantiate(winPopupPrefab, transform);
        }
    }
    private void OnLosePopupOpen()
    {
        if (GameManager.instance.GameState == State.Lose)
        {
            gameplayPanel.SetActive(false);
            Instantiate(losePopupPrefab, transform);
        }
    }
    private void OnChestPopupOpen()
    {

        Instantiate(chestPopupPrefab, transform);
    }
    private void OnKeyPopupOpen()
    {
        if (GameManager.instance.GameState == State.Lose) return;
        Instantiate(keyPopupPrefab, transform);
    }
    public void OnHammerPopupOpen()
    {
        gameplayPanel.SetActive(false);
        Instantiate(hammerPopupPrefab, transform);
    }
    #endregion
    public void ResetUI()
    {
        gameplayPanel.SetActive(true);
        bombBtn.SetActive(true);
        hammerBtn.SetActive(true);
        booster.SetActive(false);
        bombLight.SetActive(false);
        hammerLight.SetActive(false);
        bombBtn.GetComponent<Button>().interactable = true;
        hammerBtn.GetComponent<Button>().interactable = true;
        GameManager.instance.GameState = State.Play;
        onBombBooster = false; onHammerBooster = false;
    }

    private void TurnOnNotifications(int id, bool state)
    {
        notiIcons[id].SetActive(state);
    }
    public void TurnOnNotification()
    {
        DataMananger.instance.LoadData();
        int dayPlayed = DataMananger.instance.gameSave.dayPlayed % 7;

        for (int i = 0; i < dayPlayed + 1; i++)
        {
            if (!DataMananger.instance.gameSave.claimedDailys[i])
            {
                TurnOnNotifications((int)NotiID.Daily, true);
                return;
            }
        }
        TurnOnNotifications((int)NotiID.Daily, false);
    }

    private void TurnOffBtns()
    {
        if (spinBtn.gameObject.activeSelf) spinBtn.transform.DOMoveX(-50f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            spinBtn.gameObject.SetActive(false);
        });
        if (dailyBtn.gameObject.activeSelf) dailyBtn.transform.DOMoveX(-50f, 0.3f).SetEase(Ease.Linear).SetEase(Ease.Linear).OnComplete(() =>
        {
            dailyBtn.gameObject.SetActive(false);
        });
        if (shopBtn.gameObject.activeSelf) shopBtn.transform.DOMoveX(1100f, 0.3f).SetEase(Ease.Linear).SetEase(Ease.Linear).OnComplete(() =>
        {
            shopBtn.gameObject.SetActive(false);
        });

    }
    #region Load Data
    private void LevelLoad()
    {
        int currentLevelID = DataMananger.instance.gameSave.currentLevel;
        Level currentLevel = Resources.Load<Level>(Constant.LEVEL_PATH + currentLevelID);
        if (currentLevel == null)
        {
            Debug.Log("GameplayCanvasController: Level " + currentLevelID + " is  null");
            isLoaded = false;
            newLevelComingSoon.SetActive(true);
            return;
        }
        currentNumOfMoves = currentLevel.NumberOfMoves;
        numberOfMoves.text = currentNumOfMoves.ToString() + " moves";
        level.text = "Level " + (DataMananger.instance.gameSave.currentLevel + 1).ToString();
        newLevelComingSoon.SetActive(false);
        isLoaded = true;
    }

    public void SetUpBooster()
    {
        int bombBooster = DataMananger.instance.gameSave.bombBooster;
        int hammerBooster = DataMananger.instance.gameSave.hammerBooster;
        bombAmount.text = (bombBooster == 0) ? "+" : bombBooster.ToString();
        hammerAmount.text = (hammerBooster == 0) ? "+" : hammerBooster.ToString();
    }
    private void NoAds()
    {
        if (DataMananger.instance.gameSave.isNoAds)
        {
            noAds.SetActive(false);
        }
    }

    private void UpdateCurrentMoves()
    {
        numberOfMoves.text = currentNumOfMoves.ToString() + " moves";
    }

    private void OnUnder6Moves()
    {
        if (currentNumOfMoves < 6)
        {
            if (!isOnUnder6Moves)
            {
                isOnUnder6Moves = true;
                numberOfMoves.gameObject.transform.DOKill();
                numberOfMoves.color = Color.red;
                numberOfMoves.gameObject.transform.DOScale(Vector3.one * 1.2f, 0.5f).OnComplete(() =>
                {
                    numberOfMoves.gameObject.transform.DOScale(Vector3.one, 0.5f);
                }).SetLoops(-1);
            }
        }
        else
        {
            isOnUnder6Moves = false;
            numberOfMoves.gameObject.transform.DOKill();
            numberOfMoves.color = new Color(0.1803922f, 0.1921569f, 0.1921569f, 1);
        }
    }
    public void DiamondsLoad()
    {
        diamonds.text = DataMananger.instance.gameSave.currentDiamonds.ToString();
    }
    #endregion

    #region Button

    public void OnClickBombBooster()
    {
        GameManager.instance.GameState = State.Pause;
        int bombBooster = DataMananger.instance.gameSave.bombBooster;
        onBombBooster = true;
        if (bombBooster == 0)
        {
            Instantiate(getBoosterPopupPrefab, transform);
        }
        else
        {
            gameplayPanel.SetActive(false);
            hammerBtn.SetActive(false);
            booster.SetActive(true);
            bomb.SetActive(true);
            hammer.SetActive(false);
            bombLight.SetActive(true);
            bombBtn.GetComponent<Button>().interactable = false;
        }
    }

    public void OnClickHammerBooster()
    {
        GameManager.instance.GameState = State.Pause;
        int hammerBooster = DataMananger.instance.gameSave.hammerBooster;
        onHammerBooster = true;
        if (hammerBooster == 0)
        {
            GameManager.instance.GameState = State.Pause;
            Instantiate(getBoosterPopupPrefab, transform);
        }
        else
        {
            gameplayPanel.SetActive(false);
            bombBtn.SetActive(false);
            booster.SetActive(true);
            hammer.SetActive(true);
            bomb.SetActive(false);
            hammerLight.SetActive(true);
            hammerBtn.GetComponent<Button>().interactable = false;
        }
    }
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
    public void OnClickRestart()
    {
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }

    public void OnClickSettings()
    {
        GameManager.instance.GameState = State.Pause;
        Instantiate(settingsPopupPrefab, transform);
    }

    public void OnBackLevel()
    {
        DataMananger.instance.LoadData();
        DataMananger.instance.gameSave.currentLevel--;
        DataMananger.instance.SaveGame();
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }
    public void OnUpLevel()
    {
        DataMananger.instance.LoadData();
        DataMananger.instance.gameSave.currentLevel++;
        DataMananger.instance.SaveGame();
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }
    #endregion
}
