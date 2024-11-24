
using AppsFlyerSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAX_Manager : AdsManager
{
    [Header("ADS Key - Applovin")]
    [SerializeField] string maxSdkKey = "";
    [SerializeField] string bannerID = "";
    [SerializeField] string interID = "";
    [SerializeField] string rewardedID = "";

    #region Init
    public override void Init()
    {

        base.Init();
        MaxSdkCallbacks.OnSdkInitializedEvent += (res) => { OnSdkInitializedEvent(); };
        MaxSdk.SetSdkKey(maxSdkKey);
        MaxSdk.InitializeSdk();
        GoogleADManager.Initialize(aoaID, nativeID, imageInterID, InitAOACallback, InitNativeADCallback);
    }

    #endregion
    #region Banner
    public override void InitBannerCallback()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += (str, inf) => { OnBannerLoadedEvent(); };
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += (str, err) => { OnBannerLoadFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (placament, infor) => { OnAdRevenuePaidEvent(new RevalueInfor("AppLovin", infor.NetworkName, infor.AdUnitIdentifier, infor.Placement, infor.Revenue, "USD")); };
        LoadBanner();
    }
    public override void LoadBanner()
    {
        if (bannerID == "") return;
        if (IsRemoveAds) return;
        if (!FirebaseRemoteData.BANNER_AD_ON_OFF) return;
        MaxSdk.CreateBanner(bannerID, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(bannerID, "adaptive_banner", IsPortrait ? "false" : "true");
        MaxSdk.SetBannerBackgroundColor(bannerID, IsPortrait ? Color.black : Color.clear);
    }
    public override void ShowBanner()
    {
        if (bannerID == "") return;
        if (IsRemoveAds) return;
        if (!FirebaseRemoteData.BANNER_AD_ON_OFF) return;
        MaxSdk.ShowBanner(bannerID);
    }
    public override void HideBanner()
    {
        MaxSdk.HideBanner(bannerID);
    }
    #endregion
    #region Interstitial
    public override void InitInterstitialCallback()
    {

        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (placament, infor) => { OnInterstitialLoadedEvent(); };
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (placament, err) => { OnInterstitialLoadFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += (placament, infor) => { OnInterstitialDisplayedEvent(); };
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (placament, err, infor) => { OnInterstitialDisplayFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += (placament, infor) => { OnInterstitialClickedEvent(); };
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (placament, infor) => { OnInterstitialHiddenEvent(); };
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += (placament, infor) => { OnAdRevenuePaidEvent(new RevalueInfor("AppLovin", infor.NetworkName, infor.AdUnitIdentifier, infor.Placement, infor.Revenue, "USD")); };

        Invoke("LoadInterstitial", 0.5f);
    }
    public override void LoadInterstitial()
    {
        if (IsRemoveAds) return;
        MaxSdk.LoadInterstitial(interID);
    }

    public override void ShowInterstitial(bool ignoreCapping = false)
    {
        if (IsRemoveAds) return;
        if (!FirebaseRemoteData.INTER_AD_ON_OFF) return;
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_INTERS_CALL_SHOW);
        DateTime now = DateTime.Now;
        if ((now - timeCloseIntersAds).TotalSeconds < FirebaseRemoteData.INTER_AD_CAPPING_TIME) return;
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_INTERS_PASSED_CAPPING_TIME);
        if (!MaxSdk.IsInterstitialReady(interID)) return;
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_INTERS_AVAILABLE);
        interShowing = true;
        MaxSdk.ShowInterstitial(interID);
    }
    #endregion
    #region Rewarded
    public override void InitRewardedCallback()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += (placament, infor) => { OnRewardedLoadedEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (placament, err) => { OnRewardedLoadFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (placament, err, infor) => { OnRewardedDisplayFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += (placament, infor) => { OnRewardedDisplayedEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += (placament, infor) => { OnRewardedClickedEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (placament, infor) => { OnRewardedHiddenEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (placament, reward, adInfo) => { OnAdReceivedRewardEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (placament, infor) => { OnAdRevenuePaidEvent(new RevalueInfor("AppLovin", infor.NetworkName, infor.AdUnitIdentifier, infor.Placement, infor.Revenue, "USD")); };
        Invoke("LoadRewarded", 1.5f);
    }
    public override void LoadRewarded()
    {
        if (rewardedID == "") return;
        MaxSdk.LoadRewardedAd(rewardedID);
    }

    public override void ShowRewarded(Action callback = null, string placement = "", Action onNoADS = null)
    {
        if (rewardedID == "") return;
        if (isTestReward)
        {
            callback.Invoke();
            return;
        }
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_REWARDED_CALL_SHOW);
        if (!MaxSdk.IsRewardedAdReady(rewardedID))
        {
            onNoADS?.Invoke();
            return;
        }
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_REWARDED_AVAILABLE);
        callbackReward = callback;
        this.placement = placement;
        rewardShowing = true;
        MaxSdk.ShowRewardedAd(rewardedID);
    }
    #endregion
    #region AOA

    public override void InitAOACallback()
    {
        GA_AOA.OnAdDisplayFailedEvent = (err) => { OnAOAFailedEvent(err); };
        GA_AOA.OnAdHiddenEvent = OnAOAHiddenEvent;
        GA_AOA.OnAdLoadedEvent = OnAOALoadedEvent;
        GA_AOA.OnAdLoadFailedEvent = (err) => { OnAOAFailedEvent(err); };
        GA_AOA.OnAdRevenuePaidEvent = (value, CurrencyCode) => { OnAdRevenuePaidEvent(new RevalueInfor("Admob", "Admob", aoaID, "AOA", value, CurrencyCode)); };
        LoadAOA();
    }
    public override void LoadAOA()
    {
        if (IsRemoveAds) return;
        GA_AOA.LoadAd();
    }
    public override void ShowAOA(Action callback = null, bool irgoneCampingTime = false)
    {
        Debug.Log("AOA 1");
        if (isRemoveAds) return;
        Debug.Log("AOA 2");

        if (interShowing) return;
        Debug.Log("AOA 3");

        if (rewardShowing) return;
        Debug.Log("AOA 4");

        //if (!FirebaseRemoteData.OPEN_AD_ON_OFF) return;
        //Debug.Log("AOA 5");

        if (aoaShowing) return;
        Debug.Log("AOA 6");

        if (!irgoneCampingTime && (DateTime.Now - timeOutGame).TotalSeconds < FirebaseRemoteData.OPEN_AD_CAPPING_TIME) return;
        Debug.Log("AOA 7");

        //if (OpenGameCount == 0 && onPauseCount == 0) return;
        //Debug.Log("AOA 8");

        if (!GA_AOA.IsAppOpenAdAvailable) return;
        Debug.Log("AOA 9");

        aoaShowing = true;
        GA_AOA.ShowAd();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            onPauseCount++;
            timeOutGame = DateTime.Now;
        }
        else
        {
            ShowAOA();
        }
    }
    #endregion
    #region MREC
    public override void InitMRECCallback()
    {
        MaxSdkCallbacks.MRec.OnAdLoadedEvent += (id, infor) => { OnMRECLoadedEvent(); };
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += (id, infor) => { OnMRECLoadFailedEvent((int)infor.Code); };
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += (placament, infor) => { OnAdRevenuePaidEvent(new RevalueInfor("AppLovin", infor.NetworkName, infor.AdUnitIdentifier, infor.Placement, infor.Revenue, "USD")); };
        CreateMREC();
    }
    void CreateMREC()
    {
        if (mrecID == "") return;
        if (IsRemoveAds) return;
        MaxSdk.CreateMRec(mrecID, IsPortrait ? MaxSdkBase.AdViewPosition.TopCenter : MaxSdkBase.AdViewPosition.CenterLeft);
    }
    public override void LoadMREC()
    {
        if (mrecID == "") return;
        if (!FirebaseRemoteData.MREC_AD_ON_OFF) return;
        if (IsRemoveAds) return;
        MaxSdk.LoadMRec(mrecID);
    }

    public override void ShowMREC()
    {
        if (mrecID == "") return;
        if (IsRemoveAds) return;
        if (!FirebaseRemoteData.MREC_AD_ON_OFF) return;
        MaxSdk.ShowMRec(mrecID);
    }

    public override void HideMREC()
    {
        if (mrecID == "") return;
        MaxSdk.HideMRec(mrecID);
    }

    public override void InitNativeADCallback()
    {
        GA_Native.OnNativeAdFailedToLoadEvent = OnNativeAdFailedToLoadEvent;
        GA_Native.OnNativeAdLoadedEvent = OnNativeAdLoadedEvent;
        GA_Native.OnAdRevenuePaidEvent = (value, CurrencyCode) => { OnAdRevenuePaidEvent(new RevalueInfor("Admob", "Admob", nativeID, "Native", value, CurrencyCode)); };

        LoadNativeAD();
    }

    public override void ShowNativeAD(INativeADViewCallback nativeADViewCallback)
    {
        if (IsRemoveAds) return;
        if (nativeID == "") return;
        if (!FirebaseRemoteData.NATIVE_AD_ON_OFF) return;
        GA_Native.ShowNativeAD(nativeADViewCallback);
    }

    public override void LoadNativeAD()
    {
        if (nativeID == "") return;
        GA_Native.LoadNativeAD();
    }

    public override bool IsNativeADReady()
    {
        return GA_Native.NativeAd != null;
    }

    public override void UpdateMRecPosition(EMRecPosition mrecPosition)
    {
        MaxSdkBase.AdViewPosition pos = (MaxSdkBase.AdViewPosition)(int)mrecPosition;
        MaxSdk.UpdateMRecPosition(MREC_ID, pos);
    }
    #endregion
}
