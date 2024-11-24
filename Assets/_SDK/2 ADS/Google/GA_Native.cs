using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GA_Native
{
    private static NativeAd nativeAd;
    public static NativeAd NativeAd => nativeAd;
    static bool _isInitialized = false;
    static string _adUnitId = "";
    public static Action OnNativeAdLoadedEvent;
    public static Action<string> OnNativeAdFailedToLoadEvent;
    public static Action<long, string> OnAdRevenuePaidEvent;

    public static void ShowNativeAD(INativeADViewCallback nativeADCallback)
    {
        if (!_isInitialized) return;
        if (nativeAd == null) return;
        HGNativeADInfor infor = new HGNativeADInfor();
        infor.SetIcon(nativeAd.GetIconTexture());
        infor.SetADIcon(nativeAd.GetAdChoicesLogoTexture());
        infor.SetTitle(nativeAd.GetHeadlineText());
        infor.SetButtontext(nativeAd.GetCallToActionText());
        nativeADCallback.ShowNativeAD(infor, RegisterCallback);
    }
    public static void Initialize(string id)
    {
        _isInitialized = true;
        _adUnitId = id;
    }

    public static void LoadNativeAD()
    {
        nativeAd = null;
        AdLoader adLoader = new AdLoader.Builder(_adUnitId).ForNativeAd().Build();
        adLoader.OnNativeAdLoaded += HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
        adLoader.LoadAd(new AdRequest());
    }
    static void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    {
        nativeAd = args.nativeAd;
        OnNativeAdLoadedEvent();
        nativeAd.OnPaidEvent += (a, args) => { OnAdRevenuePaidEvent?.Invoke(args.AdValue.Value, args.AdValue.CurrencyCode); };
    }
    static void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        OnNativeAdFailedToLoadEvent.Invoke(e.LoadAdError.GetMessage());
    }

    static void RegisterCallback(GameObject icon, GameObject title, GameObject adchoise, GameObject textInstall)
    {
        nativeAd.RegisterIconImageGameObject(icon);
        nativeAd.RegisterHeadlineTextGameObject(title);
        nativeAd.RegisterAdChoicesLogoGameObject(adchoise);
        nativeAd.RegisterBodyTextGameObject(textInstall);
    }
}
