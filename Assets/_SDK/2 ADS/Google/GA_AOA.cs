using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;


public class GA_AOA
{

    #region HELPER METHODS


    #endregion

    public static Action<string> OnAdDisplayFailedEvent;
    public static Action OnAdHiddenEvent;
    public static Action OnAdLoadedEvent;
    public static Action<string> OnAdLoadFailedEvent;
    public static Action<long, string> OnAdRevenuePaidEvent;
    static readonly TimeSpan TIMEOUT = TimeSpan.FromHours(4);
    static DateTime _expireTime = DateTime.MaxValue;
    static string _adUnitId = "";
    static AppOpenAd _appOpenAd;
    public static void Initialize(string id)
    {
        _adUnitId = id;
    }
    public static void LoadAd()
    {

        if (_appOpenAd != null) DestroyAd();
        var adRequest = new AdRequest();

#if UNITY_EDITOR
        AppOpenAd.Load(_adUnitId, ScreenOrientation.LandscapeLeft, adRequest, OnAppOpenAdLoad);
#else
        AppOpenAd.Load(_adUnitId, adRequest, OnAppOpenAdLoad);
#endif
    }

    static void OnAppOpenAdLoad(AppOpenAd ad, LoadAdError error)
    {
       
        if (ad == null || error != null)
        {
            OnAdLoadFailedEvent?.Invoke(error.GetCode().ToString());
            return;
        }
        _appOpenAd = ad;
        _expireTime = DateTime.Now + TIMEOUT;
        RegisterEventHandlers(_appOpenAd);
    }
    static void DestroyAd()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

    }

    static bool isShowingAppOpenAd = false;
    public static void ShowAd()
    {
        //#if !UNITY_EDITOR
        if (!IsAppOpenAdAvailable) return;
        RegisterEventHandlers(_appOpenAd);
        isShowingAppOpenAd = true;
        _appOpenAd.Show();
        //#endif

    }
    public static bool IsAppOpenAdAvailable => !isShowingAppOpenAd && _appOpenAd != null && DateTime.Now < _expireTime;




    static void RegisterEventHandlers(AppOpenAd ad)
    {
        ad.OnAdPaid += (args) => { OnAdRevenuePaidEvent?.Invoke(args.Value, args.CurrencyCode); };
        ad.OnAdFullScreenContentOpened += () => { };
        ad.OnAdFullScreenContentClosed += () => { isShowingAppOpenAd = false; OnAdHiddenEvent?.Invoke(); };
        ad.OnAdFullScreenContentFailed += (args) =>
         {
             isShowingAppOpenAd = false; OnAdDisplayFailedEvent?.Invoke(args.GetCode().ToString());
             _appOpenAd = null;
         };
    }
}
