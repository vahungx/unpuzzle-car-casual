using EasyUI.PickerWheelUI;
using EnhancedScrollerDemos.SuperSimpleDemo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetRewardPopupController : BasePopup
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private GameObject singleReward, multiReward, collectx2Obj;

    [Header("ShopReward")]
    [SerializeField] private Image[] icons;
    [SerializeField] private TextMeshProUGUI[] amounts;

    private WheelPiece _wheelPiece;
    private Daily _daily;
    private ShopScrollerData _shopScrollerData;
    private void Start()
    {
        DataMananger.instance.LoadData();
        AudioManager.instance.Stop();
        AudioManager.instance.Play((int)SoundID.Claiming);
    }
    public void SetInfo(WheelPiece wheelPiece)
    {
        singleReward.SetActive(true);
        multiReward.SetActive(false);
        collectx2Obj.SetActive(true);
        _wheelPiece = wheelPiece;
        icon.sprite = wheelPiece.Icon;
        amount.text = wheelPiece.Amount.ToString();
    }
    public void SetInfo(Daily daily)
    {
        singleReward.SetActive(true);
        multiReward.SetActive(false);
        collectx2Obj.SetActive(true);
        _daily = daily;
        icon.sprite = daily.icon;
        amount.text = daily.amount.ToString();
    }
    public void SetInfo(ShopScrollerData shopScrollerData)
    {
        multiReward.SetActive(true);
        singleReward.SetActive(false);
        collectx2Obj.SetActive(false);
        _shopScrollerData = shopScrollerData;
        for (int i = 0; i < 3; i++)
        {
            icons[i].sprite = shopScrollerData.rewards[i].sprite;
            amounts[i].text = shopScrollerData.rewards[i].amount.ToString();
        }
    }
    public override void Close()
    {
        DataMananger.instance.LoadData();
        if (_wheelPiece != null)
        {
            if (_wheelPiece.Label != "Diamonds")
            {
                DataMananger.instance.gameSave.currentSkins.Add(new Skin
                {

                });
            }
            else if (_wheelPiece.Label == "Diamonds")
            {
                DataMananger.instance.gameSave.currentDiamonds += _wheelPiece.Amount;
            }

        }
        if (_daily != null)
        {
            if (_daily.type == RewardType.Diamonds)
            {
                DataMananger.instance.gameSave.currentDiamonds += _daily.amount;
            }
            else if (_daily.type == RewardType.Booster)
            {
                if (_daily.name == "Hammer")
                    DataMananger.instance.gameSave.hammerBooster += _daily.amount;
                if (_daily.name == "Boomb")
                    DataMananger.instance.gameSave.bombBooster += _daily.amount;
            }
        }
        if (_shopScrollerData != null)
        {
            foreach (Reward reward in _shopScrollerData.rewards)
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
        }
        DataMananger.instance.SaveGame();
        base.Close();
    }

    public void OnAds2Collect()
    {
        if (_wheelPiece != null)
        {
            if (_wheelPiece.Label != "Diamonds")
            {
                DataMananger.instance.gameSave.currentSkins.Add(new Skin
                {

                });
            }
            else if (_wheelPiece.Label == "Diamonds")
            {
                DataMananger.instance.gameSave.currentDiamonds += _wheelPiece.Amount;
            }

        }
        if (_daily != null)
        {
            if (_daily.type == RewardType.Diamonds)
            {
                DataMananger.instance.gameSave.currentDiamonds += _daily.amount;
            }
            else if (_daily.type == RewardType.Booster)
            {
                if (_daily.name == "Hammer")
                    DataMananger.instance.gameSave.hammerBooster += _daily.amount;
                if (_daily.name == "Boomb")
                    DataMananger.instance.gameSave.bombBooster += _daily.amount;
            }
        }
        DataMananger.instance.SaveGame();
        Destroy(gameObject);
    }
}
