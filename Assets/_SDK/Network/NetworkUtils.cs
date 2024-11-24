using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkUtils
{
    public static void OpenSettings()
    {
#if !UNITY_EDITOR

#if UNITY_IPHONE
            string url = MyNativeBindings.GetSettingsURL();
            Application.OpenURL(url);
#elif UNITY_ANDROID
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject currentActivityObject =
            unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var intentObject = new AndroidJavaObject(
            "android.content.Intent", "android.settings.WIFI_SETTINGS"))
        {
            currentActivityObject.Call("startActivity", intentObject);
        }
#endif
#endif

    }
}
