using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DailyPopupController : BasePopup
{
    [SerializeField] private List<GiftController> giftControllers;

    protected override void Awake()
    {
        SetTimeDaily();
        SetListGiftControllers();
    }

    private void SetTimeDaily()
    {
        DateTime installTime = DateTime.ParseExact(DataMananger.instance.gameSave.installTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        DataMananger.instance.gameSave.dayPlayed = (int)((DateTime.Now - installTime).TotalSeconds / Constant.giftTimeCountDown);
        ResetClaimedDailys();
        DataMananger.instance.SaveGame();
    }

    private void SetListGiftControllers()
    {
        int dayPlayeds = DataMananger.instance.gameSave.dayPlayed % 7;
        for (int i = 0; i < giftControllers.Count; i++)
        {
            giftControllers[i].id = i;
            if (i <= dayPlayeds)
            {
                giftControllers[i].daily.state = DataMananger.instance.gameSave.claimedDailys[i] ? DailyState.Claimed : DailyState.CanClaming;
            }
            else giftControllers[i].daily.state = (dayPlayeds + 1 == i) ? DailyState.WaitingToClaim : DailyState.CantClaming;
            giftControllers[i].SetUp();
        }
    }

    public void OnAddDayPlayed()
    {
        DataMananger.instance.gameSave.dayPlayed++;
        DataMananger.instance.SaveGame();
        ResetClaimedDailys();
        SetListGiftControllers();
    }

    private void ResetClaimedDailys()
    {
        int dayPlayeds = DataMananger.instance.gameSave.dayPlayed % 7;

        if (dayPlayeds == 0)
        {
            if (!DataMananger.instance.gameSave.isResetedDailys)
            {
                for (int i = 0; i < DataMananger.instance.gameSave.claimedDailys.Length; i++)
                {
                    DataMananger.instance.gameSave.claimedDailys[i] = false;
                }
                DataMananger.instance.gameSave.isResetedDailys = true;
            }
        }
        else
        {
            DataMananger.instance.gameSave.isResetedDailys = false;
        }
        DataMananger.instance.SaveGame();
    }

    public override void Close()
    {
        base.Close();
        StartCoroutine(IEShowADS());
    }

    IEnumerator IEShowADS()
    {
        GA_Inter.ShowAd();
        yield return new WaitForSeconds(0.5f);
        AdsManager.Instance.ShowAOA(null, true);
        yield return new WaitForEndOfFrame();
        //AdsManager.Instance.UpdateMRecPosition(EMRecPosition.BottomCenter);
        AdsManager.Instance.HideMREC();
        yield return new WaitForSeconds(0.25f);
        SceneManager.UnloadSceneAsync((int)SceneID.Daily);
    }

}
