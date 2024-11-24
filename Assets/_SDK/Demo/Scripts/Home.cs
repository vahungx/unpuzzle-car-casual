using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    private void Start()
    {
        
    }
    public void ShowBanner()
    {
        AdsManager.Instance.ShowBanner();
    }
    public void HideBanner()
    {
        AdsManager.Instance.HideBanner();

    }
    public void ShowInter()
    {
        AdsManager.Instance.ShowInterstitial();
    }
    public void ShowReward()
    {
        AdsManager.Instance.ShowRewarded(OnRewardComplete);
    }
    void OnRewardComplete()
    {

    }

}
