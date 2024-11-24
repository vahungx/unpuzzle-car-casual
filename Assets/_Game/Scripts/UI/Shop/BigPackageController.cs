using EnhancedScrollerDemos.SuperSimpleDemo;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigPackageController : MonoBehaviour
{
    [Header("Text")]

    [SerializeField] private TextMeshProUGUI namePackageTxt;
    [SerializeField] private TextMeshProUGUI cashTxt;
    [SerializeField] private TextMeshProUGUI diamondsAmount;
    [SerializeField] private TextMeshProUGUI bombsAmonut;
    [SerializeField] private TextMeshProUGUI hammersAmount;
    [Space]
    [Header("Image")]

    [SerializeField] private Image diamonds;
    [SerializeField] private Image bombs;
    [SerializeField] private Image hammers;

    public void SetUp(ShopScrollerData data)
    {
        namePackageTxt.text = data.name;
        cashTxt.text = "Buy $" + data.cash.ToString();
        diamondsAmount.text = "x" + data.rewards[(int)PackageRewardID.Diamond].amount.ToString();
        bombsAmonut.text = data.rewards[(int)PackageRewardID.Bomb].amount.ToString();
        hammersAmount.text = data.rewards[(int)PackageRewardID.Hammer].amount.ToString();
        diamonds.sprite = data.rewards[(int)PackageRewardID.Diamond].sprite;
        bombs.sprite = data.rewards[(int)PackageRewardID.Bomb].sprite;
        hammers.sprite = data.rewards[(int)PackageRewardID.Hammer].sprite;
    }
    public void OnClickBuy()
    {

    }
}
