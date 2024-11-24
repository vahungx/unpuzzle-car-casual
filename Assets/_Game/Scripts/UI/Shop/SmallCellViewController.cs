using EnhancedScrollerDemos.SuperSimpleDemo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmallCellViewController : MonoBehaviour
{
    [Header("Text")]

    [SerializeField] private TextMeshProUGUI namePackageTxt;
    [SerializeField] private TextMeshProUGUI cashTxt;
    [SerializeField] private TextMeshProUGUI diamondsAmount;
    [Space]
    [Header("Image")]

    [SerializeField] private Image diamonds;
    private ShopScrollerData _data;
    public void SetData(ShopScrollerData data)
    {
        _data = data;
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        namePackageTxt.text = data.name;
        cashTxt.text = "Buy $" + data.cash.ToString();
        diamondsAmount.text = data.rewards[(int)PackageRewardID.Diamond].amount.ToString();
        diamonds.sprite = data.rewards[(int)PackageRewardID.Diamond].sprite;
    }

    public void OnClickGetRewards()
    {
        DataMananger.instance.LoadData();
        //if ads true || iap true
        foreach (Reward reward in _data.rewards)
        {
            switch (reward.id)
            {
                case (int)PackageRewardID.Diamond:
                    DataMananger.instance.gameSave.currentDiamonds += reward.amount;
                    break;
                case (int)PackageRewardID.Bomb:
                    DataMananger.instance.gameSave.bombBooster += reward.amount;
                    break;
                case (int)PackageRewardID.Hammer:
                    DataMananger.instance.gameSave.hammerBooster += reward.amount;
                    break;
            }
        }
        DataMananger.instance.SaveGame();
    }
}
