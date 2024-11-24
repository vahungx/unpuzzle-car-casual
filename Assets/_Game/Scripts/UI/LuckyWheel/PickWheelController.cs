using EasyUI.PickerWheelUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickWheelController : BasePopup
{
    [SerializeField] private ButtonExtension freeSpinBtn;
    [SerializeField] private ButtonExtension AdsSpinBtn;
    [SerializeField] private ButtonExtension backBtn;

    [SerializeField] private PickerWheel pickerWheel;
    [SerializeField] private GameObject getRewardPopup;
    [SerializeField] private TextMeshProUGUI countDownText;

    private bool isStartCounting = false;

    protected override void OnEnable()
    {
        DataMananger.instance.LoadData();
        if (DataMananger.instance.gameSave.firstSpin) return;
        DateTime startTime = DateTime.ParseExact(DataMananger.instance.gameSave.luckySpinStartCountDown, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        if ((DateTime.Now - startTime).TotalSeconds < Constant.luckyTimeCountDown)
            StartCoroutine(StartCountDown(startTime));
        base.OnEnable();
    }

    private void Start()
    {
        OnClickFreeSpinBtn();
        OnClickAdsSpinBtn();
    }

    private void OnClickAdsSpinBtn()
    {
        AdsSpinBtn.onClick.AddListener(() =>
        {
            //Check Ads if true
            AdsSpinBtn.interactable = false;
            pickerWheel.OnSpinStart(() =>
            {
                backBtn.interactable = false;
            }
                );
            pickerWheel.Spin();
            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                backBtn.interactable = true;
                AdsSpinBtn.interactable = true;
                if (DataMananger.instance.gameSave.firstSpin) DataMananger.instance.gameSave.firstSpin = false;
                GameObject popup = Instantiate(getRewardPopup, GameplayCanvasController.instance.transform);
                popup.GetComponent<GetRewardPopupController>().SetInfo(wheelPiece);
            });
        });
    }
    private void OnClickFreeSpinBtn()
    {
        freeSpinBtn.onClick.AddListener(() =>
        {
            freeSpinBtn.interactable = false;
            pickerWheel.OnSpinStart(() =>
            {
                backBtn.interactable = false;

            });
            pickerWheel.Spin();
            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                backBtn.interactable = true;
                if (DataMananger.instance.gameSave.firstSpin) DataMananger.instance.gameSave.firstSpin = false;
                GameObject popup = Instantiate(getRewardPopup, GameplayCanvasController.instance.transform);
                popup.GetComponent<GetRewardPopupController>().SetInfo(wheelPiece);
                isStartCounting = true;
                DataMananger.instance.gameSave.luckySpinStartCountDown = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DataMananger.instance.SaveGame();
                DateTime startTime = DateTime.ParseExact(DataMananger.instance.gameSave.luckySpinStartCountDown, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                StartCoroutine(StartCountDown(startTime));
            });
        });
    }
    private IEnumerator StartCountDown(DateTime startTime)
    {
        if (!isStartCounting) yield return null;
        TimeSpan timePassed = DateTime.Now - startTime;
        float timeLeft = Constant.luckyTimeCountDown - (float)timePassed.TotalSeconds;
        while (timeLeft > 0)
        {
            timeLeft--;
            UpdateTimeText(timeLeft);
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateTimeText(float curTime)
    {
        int minutes = Mathf.FloorToInt(curTime / 60);
        int seconds = Mathf.FloorToInt(curTime % 60);
        countDownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (curTime > 0)
        {
            countDownText.gameObject.SetActive(true);
            freeSpinBtn.interactable = false;
        }
        else
        {
            freeSpinBtn.interactable = true;
            countDownText.gameObject.SetActive(false);
        }
    }
}
