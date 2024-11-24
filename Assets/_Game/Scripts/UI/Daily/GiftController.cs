using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftController : MonoBehaviour
{
    [SerializeField] private GameObject darkBgObj, colectObj, tickObj, claimBtnObj;

    [Space]
    [SerializeField] private Sprite yellowBg, blueBg;

    [Space]
    [SerializeField] private Image bgImage;

    [Space]
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI nameText;

    [Space]
    [SerializeField] private DailyCollection dailyCollection;

    [Space]
    [SerializeField] private GameObject getRewardPrefab;
    public Daily daily;
    [HideInInspector] public int id;

    public void SetUp()
    {
        DataMananger.instance.LoadData();

        if (id == 6)
        {
            colectObj.SetActive(!DataMananger.instance.gameSave.claimedDailys[6]);
        }
        switch (daily.state)
        {
            case DailyState.Claimed:
                nameText.color = Color.white;
                bgImage.sprite = blueBg;
                darkBgObj.SetActive(true);
                tickObj.SetActive(true);
                claimBtnObj.SetActive(false);
                countDownText.gameObject.SetActive(false);
                break;

            case DailyState.CanClaming:
                nameText.color = new Color(0.6313726f, 0.3686275f, 0.1019608f, 1f);
                bgImage.sprite = yellowBg;
                claimBtnObj.SetActive(true);
                countDownText.gameObject.SetActive(false);
                darkBgObj.SetActive(false);
                tickObj.SetActive(false);
                break;

            case DailyState.WaitingToClaim:
                nameText.color = Color.white;
                bgImage.sprite = blueBg;
                claimBtnObj.SetActive(false);
                countDownText.gameObject.SetActive(true);
                darkBgObj.SetActive(false);
                tickObj.SetActive(false);
                //Set CountDown
                DateTime installTime = DateTime.ParseExact(DataMananger.instance.gameSave.installTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime startTime = installTime.AddSeconds(DataMananger.instance.gameSave.dayPlayed * Constant.giftTimeCountDown);
                if ((DateTime.Now - installTime).TotalSeconds % Constant.giftTimeCountDown < Constant.giftTimeCountDown)
                {
                    StartCoroutine(StartCountDown(startTime));
                }
                break;

            case DailyState.CantClaming:
                nameText.color = Color.white;
                bgImage.sprite = blueBg;
                darkBgObj.SetActive(false);
                tickObj.SetActive(false);
                claimBtnObj.SetActive(false);
                countDownText.gameObject.SetActive(false);
                break;
        }
    }
    public void OnClickClaim()
    {
        foreach (Daily daily in dailyCollection.dailys)
        {
            Daily rewardDaily = new Daily();
            if (id == daily.id)
            {
                rewardDaily = (id != 6) ? daily : dailyCollection.dailys[UnityEngine.Random.Range(6, 8)];
                GameObject rewardObj = Instantiate(getRewardPrefab, GameplayCanvasController.instance.transform);
                rewardObj.GetComponent<GetRewardPopupController>().SetInfo(rewardDaily);
                DataMananger.instance.gameSave.claimedDailys[id] = true;
                DataMananger.instance.SaveGame();
                this.daily.state = DailyState.Claimed;
                break;
            }
        }
        GameplayCanvasController.instance.TurnOnNotification();
        SetUp();
    }

    private IEnumerator StartCountDown(DateTime startTime)
    {
        TimeSpan timePassed = DateTime.Now - startTime;
        float timeLeft = Constant.giftTimeCountDown - (float)timePassed.TotalSeconds;
        while (timeLeft > 0)
        {
            timeLeft--;
            UpdateTimeText(timeLeft);
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateTimeText(float curTime)
    {
        int hours = (curTime > 3600f) ? Mathf.FloorToInt(curTime / 3600) : Mathf.FloorToInt(curTime / 60);
        int minutes = (curTime > 3600f) ? Mathf.FloorToInt(curTime / 60) : Mathf.FloorToInt(curTime % 60);
        countDownText.text = (curTime > 3600f) ? "open in " + string.Format("{0}h {0}m", hours, minutes) : "open in " + string.Format("{0}m {0}s", hours, minutes);
        if (curTime > 0)
        {
            daily.state = DailyState.WaitingToClaim;
        }
        else
        {
            daily.state = DailyState.CanClaming;
            SetUp();
        }
    }
}
