using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetBoosterPopupController : BasePopup
{
    [SerializeField] private GameObject bomb, hammer;
    [SerializeField] private TextMeshProUGUI diamondsCost;
    [SerializeField] private GameObject notEnoughGemsPopup;


    private bool isBomb = false, isHammer = false;
    private int cost = 0;
    private void Start()
    {
        GameManager.instance.GameState = State.Pause;
        Setup();
    }

    public void Setup()
    {
        if (GameplayCanvasController.instance.onBombBooster)
        {
            GameplayCanvasController.instance.onBombBooster = false;
            isBomb = true;
            cost = (DataMananger.instance.gameSave.isCheating) ? 0 : 150;
            bomb.SetActive(true);
            hammer.SetActive(false);
        }
        if (GameplayCanvasController.instance.onHammerBooster)
        {
            GameplayCanvasController.instance.onHammerBooster = false;
            isHammer = true;
            cost = (DataMananger.instance.gameSave.isCheating) ? 0 : 75;
            bomb.SetActive(false);
            hammer.SetActive(true);
        }
        diamondsCost.text = cost.ToString();
    }

    public void OnClickGetByDiamondsBtn()
    {

        if (DataMananger.instance.gameSave.currentDiamonds < cost)
        {
            Instantiate(notEnoughGemsPopup, GameplayCanvasController.instance.transform);
        }
        else
        {
            DataMananger.instance.gameSave.currentDiamonds -= cost;
            if (isBomb)
            {
                DataMananger.instance.gameSave.bombBooster = 3;
                GameplayCanvasController.instance.SetUpBooster();
                GameplayCanvasController.instance.OnClickBombBooster();
            }
            if (isHammer)
            {
                DataMananger.instance.gameSave.hammerBooster = 3;
                GameplayCanvasController.instance.SetUpBooster();
                GameplayCanvasController.instance.OnClickHammerBooster();
            }
            DataMananger.instance.SaveGame();
        }
        DataMananger.instance.LoadData();
        Destroy(gameObject);
    }

    public override void Close()
    {
        GameplayCanvasController.instance.ResetUI();
        base.Close();
    }
}
