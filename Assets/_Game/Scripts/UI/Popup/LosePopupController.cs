using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePopupController : BasePopup
{
    [SerializeField] private GameObject _10MovesBtn, _5MovesBtn, _ReplayBtn;
    private int currentDiamonds;
    public static Action OnClickBack;
    public void Start()
    {
        if (BaseBox.isActivatedBoomb)
        {
            _10MovesBtn.SetActive(false);
            _5MovesBtn.SetActive(false);
            _ReplayBtn.SetActive(true);
        };
        DataMananger.instance.LoadData();
        currentDiamonds = DataMananger.instance.gameSave.currentDiamonds;
        AudioManager.instance.Stop();
        AudioManager.instance.Play((int)SoundID.Lose);
    }

    public override void Close()
    {
        base.Close();
        //Show inter
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }

    public void OnClick10Moves()
    {
        int cash = 0;
        GameManager.instance.GameState = State.Play;
        Destroy(gameObject);

        if (currentDiamonds < cash)
        {
            //show popup thiếu tiền
            DataMananger.instance.SaveGame();
        }
        else
        {
            currentDiamonds -= cash;
            DataMananger.instance.SaveGame();
            GameplayCanvasController.instance.currentNumOfMoves += 10;
        }
        GameplayCanvasController.instance.gameplayPanel.SetActive(true);
    }

    public void OnClick5Moves()
    {
        //Check Ads 2 
        //Nếu có Ads thì thêm 
        GameplayCanvasController.instance.currentNumOfMoves += 5;
        GameManager.instance.GameState = State.Play;
        GameplayCanvasController.instance.gameplayPanel.SetActive(true);
        Destroy(gameObject);
    }

    public void OnClickAdsToSkip()
    {
        //Check Ads
        // if true
        //GameManager.instance.GameState = State.Play;
        //BaseBox.isActivatedBoomb = false;
        //OnClickBack();

        Destroy(gameObject);
        DataMananger.instance.gameSave.currentLevel++;
        DataMananger.instance.SaveGame();
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }
}
