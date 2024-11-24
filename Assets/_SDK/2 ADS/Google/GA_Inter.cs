using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Inter 
{
    static InterstitialAd _interstitialAd;
    static string _adUnitId = "";

    public static void Initialize(string id)
    {
        _adUnitId = id;
        LoadAd();
    }

    public static void LoadAd()
    {
        if (_interstitialAd != null)
        {
            DestroyAd();
        }
        var adRequest = new AdRequest();

        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {

            if (error != null)
            {
                return;
            }
            if (ad == null)
            {
                return;
            }
            _interstitialAd = ad;
            RegisterEventHandlers(ad);

        });
    }


    public static void ShowAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }

    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    static void DestroyAd()
    {
        if (_interstitialAd != null)
        {
            Debug.Log("Destroying interstitial ad.");
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
    }
    static void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) => { AdsManager.Instance.OnAdRevenuePaidEvent(new RevalueInfor("Admob", "Admob", _adUnitId, "AOA", adValue.Value, adValue.CurrencyCode)); };
        ad.OnAdFullScreenContentClosed += () => { };
        ad.OnAdFullScreenContentFailed += (err) => { };

        //ad.OnAdFullScreenContentClosed += () =>        {
        //};       ad.OnAdFullScreenContentFailed += (AdError error) =>
        //{
        //};
    }
}
