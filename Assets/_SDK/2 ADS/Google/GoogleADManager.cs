using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleADManager
{
    [SerializeField] static bool _isInitialized;
    public bool IsInitialized => _isInitialized;
    public static void Initialize(string aoaID, string nativeID, string interImageID, params Action[] onLoads)
    {
        MobileAds.Initialize((value) =>
        {
            _isInitialized = true;
            GA_AOA.Initialize(aoaID);
            GA_Native.Initialize(nativeID);
            GA_Inter.Initialize(interImageID);
            for (int i = 0; i < onLoads.Length; i++) onLoads[i].Invoke();
        });
        MobileAds.SetiOSAppPauseOnBackground(true);
    }
}
