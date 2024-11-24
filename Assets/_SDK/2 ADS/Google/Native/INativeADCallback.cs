using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INativeADViewCallback
{
    void ShowNativeAD(HGNativeADInfor nativeAd, Action<GameObject, GameObject, GameObject, GameObject> RegisterCallback);
    void ResetView();
}
