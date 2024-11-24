using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
public class FirebaseParam
{
    [SerializeField] string key;
    [SerializeField] string value;
    public FirebaseParam() { }
    public FirebaseParam(string key, object value)
    {
        this.key = key;
        this.value = value.ToString();
    }
    public FirebaseParam(string key, string value)
    {
        this.key = key;
        this.value = value;
    }

    public string Value => value;

    public void SetValue(string value)
    {
        this.value = value;
    }


    public string Key => key;

    public void SetKey(string value)
    {
        key = value;
    }


}

public class FirebaseRemoteData
{
    static int inter_ad_capping_time = 12;
    static int open_ad_capping_time = 35;
    static string open_ad_id = AdsManager.Instance.AOA_ID;
    static string native_ad_id = AdsManager.Instance.NATIVE_ID;
    static string mrec_ad_id = AdsManager.Instance.MREC_ID;
    static bool open_ad_on_off = false;
    static bool banner_ad_on_off = true;
    static bool native_ad_on_off = true;
    static bool mrec_ad_on_off = true;
    static bool inter_ad_on_off = true;
    static int level_show_rate = 1;
    static bool show_rate_off_country = false;


    public static int INTER_AD_CAPPING_TIME { get => inter_ad_capping_time; set => inter_ad_capping_time = value; }
    public static int OPEN_AD_CAPPING_TIME { get => open_ad_capping_time; set => open_ad_capping_time = value; }
    public static string OPEN_AD_ID { get => open_ad_id; set => open_ad_id = value; }
    public static string NATIVE_AD_ID { get => native_ad_id; set => native_ad_id = value; }
    public static string MREC_AD_ID { get => mrec_ad_id; set => mrec_ad_id = value; }
    public static bool OPEN_AD_ON_OFF { get => open_ad_on_off; set => open_ad_on_off = value; }
    public static bool BANNER_AD_ON_OFF { get => banner_ad_on_off; set => banner_ad_on_off = value; }
    public static bool NATIVE_AD_ON_OFF { get => native_ad_on_off; set => native_ad_on_off = value; }
    public static bool MREC_AD_ON_OFF { get => mrec_ad_on_off; set => mrec_ad_on_off = value; }
    public static bool INTER_AD_ON_OFF { get => inter_ad_on_off; set => inter_ad_on_off = value; }

    public static int LEVEL_SHOW_RATE { get => level_show_rate; set => level_show_rate = value; }
    public static bool SHOW_RATE_OFF_COUNTRY { get => show_rate_off_country; set => show_rate_off_country = value; }
}


public class HandleFireBase : unity_base.Singleton<HandleFireBase>
{
    public bool InitDone = false;
    [System.Serializable]
    public enum Location
    {
        Popup_LevelResult = 0,
        Popup_Settings = 1,
        Popup_Shop = 2,
        Popup_LuckySpin = 3,
        Popup_ResourceAd = 4,
        Popup_DailyReward = 5,
        Popup_LevelSelection = 6,
        Popup_FreeSkin = 7,
        Popup_Revive = 8,

        Scene_Menu = 100,
        Scene_Game = 101,

        Popup_Shop_Decor,
        Popup_Shop_Card,
        Popup_Show_Coin
    }
    [System.Serializable]
    public enum ButtonName
    {
        Default,
        Coin,
        Live,
        TrySkin,
        UnlockSkin,
        Play,
        Replay,
        Home,
        SkipLevel,
        Magnet,
    }


    public const string ADS_REWARD_OFFER = "ads_reward_offer";

    public const string ADS_REWARD_CLICK = "ads_reward_click";
    public const string ADS_REWARD_SHOW = "ads_reward_show";
    public const string ADS_REWARD_SHOW_FAIL = "ads_reward_show_fail";
    public const string ADS_REWARD_LOAD_FAIL = "ads_reward_load_fail";

    public const string ADS_REWARD_TIME_LIMIT = "ads_reward_time_limit";


    public const string ADS_REWARD_COMPLETE = "ads_reward_complete";
    public const string AD_INTER_LOAD_FAIL = "ad_inter_load_fail";
    public const string AD_INTER_SHOW_FAIL = "ad_inter_show_fail";

    public const string AD_INTER_LOAD = "ad_inter_load";
    public const string AD_INTER_SHOW = "ad_inter_show";
    public const string AD_INTER_CLICK = "ad_inter_click";
    public const string AD_INTER_TIME_LIMIT = "ad_inter_time_limit";
    public const string INTER_AD_PUSH_CLICK_ON_OFF = "inter_ad_push_click_on_off";

    #region Remote Config

    public const string INTER_AD_CAPPING_TIME = "inter_ad_capping_time";
    public const string OPEN_AD_CAPPING_TIME = "open_ad_capping_time";
    public const string OPEN_AD_ID = "open_ad_id";
    public const string NATIVE_AD_ID = "native_ad_id";
    public const string MREC_AD_ID = "mrec_ad_id";
    public const string OPEN_AD_ON_OFF = "open_ad_on_off";
    public const string BANNER_AD_ON_OFF = "banner_ad_on_off";
    public const string NATIVE_AD_ON_OFF = "native_ad_on_off";
    public const string MREC_AD_ON_OFF = "mrec_ad_on_off";
    public const string INTER_AD_ON_OFF = "inter_ad_on_off";
    public const string LEVEL_SHOW_RATE = "level_show_rate";
    public const string SHOW_RATE_OFF_COUNTRY = "show_rate_off_country";




    #endregion
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    private void Start()
    {
        Invoke("Initialize", 0.5f);
        //Initialize();
    }
    public void Initialize()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            Debug.Log(dependencyStatus);
            if (dependencyStatus == DependencyStatus.Available)
            {

                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
                InitRemoteConfig();

            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void InitRemoteConfig()
    {
        Dictionary<string, object> defaults = new Dictionary<string, object>();
        defaults.Add(INTER_AD_CAPPING_TIME, 12);
        defaults.Add(OPEN_AD_CAPPING_TIME, 35);
        defaults.Add(OPEN_AD_ID, AdsManager.Instance.AOA_ID);
        defaults.Add(NATIVE_AD_ID, AdsManager.Instance.NATIVE_ID);
        defaults.Add(MREC_AD_ID, AdsManager.Instance.MREC_ID);
        defaults.Add(OPEN_AD_ON_OFF, false);
        defaults.Add(BANNER_AD_ON_OFF, true);
        defaults.Add(NATIVE_AD_ON_OFF, true);
        defaults.Add(MREC_AD_ON_OFF, true);
        defaults.Add(INTER_AD_ON_OFF, true);
        defaults.Add(LEVEL_SHOW_RATE, 1);
        defaults.Add(SHOW_RATE_OFF_COUNTRY, false);

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task => { FetchDataAsync(); });
    }

    Task FetchDataAsync()
    {
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    public void DisplayData()
    {
        Debug.Log($"FBDT {INTER_AD_CAPPING_TIME} {FirebaseRemoteData.INTER_AD_CAPPING_TIME}");
        Debug.Log($"FBDT {OPEN_AD_CAPPING_TIME} {FirebaseRemoteData.OPEN_AD_CAPPING_TIME}");
        Debug.Log($"FBDT {OPEN_AD_ID} {FirebaseRemoteData.OPEN_AD_ID}");
        Debug.Log($"FBDT {NATIVE_AD_ID} {FirebaseRemoteData.NATIVE_AD_ID}");
        Debug.Log($"FBDT {MREC_AD_ID} {FirebaseRemoteData.MREC_AD_ID}");
        Debug.Log($"FBDT {OPEN_AD_ON_OFF} {FirebaseRemoteData.OPEN_AD_ON_OFF}");
        Debug.Log($" {BANNER_AD_ON_OFF} {FirebaseRemoteData.BANNER_AD_ON_OFF}");
        Debug.Log($"FBDT {NATIVE_AD_ON_OFF} {FirebaseRemoteData.NATIVE_AD_ON_OFF}");
        Debug.Log($"FBDT {INTER_AD_ON_OFF} {FirebaseRemoteData.INTER_AD_ON_OFF}");
        Debug.Log($"FBDT {MREC_AD_ON_OFF} {FirebaseRemoteData.MREC_AD_ON_OFF}");
        Debug.Log($"FBDT {LEVEL_SHOW_RATE} {FirebaseRemoteData.LEVEL_SHOW_RATE}");
        Debug.Log($"FBDT {SHOW_RATE_OFF_COUNTRY} {FirebaseRemoteData.SHOW_RATE_OFF_COUNTRY}");



    }

    void FetchComplete(Task fetchTask)
    {
        var info = FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(task =>
                {
                    //FirebaseRemoteData.INTER_AD_CAPPING_TIME = FirebaseToInt(INTER_AD_CAPPING_TIME);
                    //FirebaseRemoteData.OPEN_AD_CAPPING_TIME = FirebaseToInt(OPEN_AD_CAPPING_TIME);
                    //FirebaseRemoteData.OPEN_AD_ID = FirebaseToString(OPEN_AD_ID);
                    //FirebaseRemoteData.NATIVE_AD_ID = FirebaseToString(NATIVE_AD_ID);
                    //FirebaseRemoteData.MREC_AD_ID = FirebaseToString(MREC_AD_ID);
                    //FirebaseRemoteData.OPEN_AD_ON_OFF = FirebaseToBool(OPEN_AD_ON_OFF);
                    //FirebaseRemoteData.BANNER_AD_ON_OFF = FirebaseToBool(BANNER_AD_ON_OFF);
                    //FirebaseRemoteData.NATIVE_AD_ON_OFF = FirebaseToBool(NATIVE_AD_ON_OFF);
                    //FirebaseRemoteData.MREC_AD_ON_OFF = FirebaseToBool(MREC_AD_ON_OFF);
                    //FirebaseRemoteData.INTER_AD_ON_OFF = FirebaseToBool(INTER_AD_ON_OFF);
                    //FirebaseRemoteData.LEVEL_SHOW_RATE = FirebaseToInt(LEVEL_SHOW_RATE);
                    //FirebaseRemoteData.SHOW_RATE_OFF_COUNTRY = FirebaseToBool(SHOW_RATE_OFF_COUNTRY);
                    DisplayData();
                });

                break;
            case LastFetchStatus.Failure: break;
            case LastFetchStatus.Pending: break;
        }
        InitDone = true;
    }
    int FirebaseToInt(string key)
    {
        return int.Parse(FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue.Trim());
    }
    string FirebaseToString(string key)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue.Trim();
    }
    bool FirebaseToBool(string key)
    {
        return bool.Parse(FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue.Trim());
    }


    public void LogEventWithParameter(string eventName, params FirebaseParam[] args)
    {
        Parameter[] finalParams = new Parameter[args.Length];
        for (int i = 0; i < finalParams.Length; i++)
        {
            finalParams[i] = new Parameter(args[i].Key, args[i].Value);
        }
        FirebaseAnalytics.LogEvent(eventName, finalParams);
    }
    public void LogEventWithString(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }
    public void LogEventStart(int level)
    {
        FirebaseAnalytics.LogEvent("level_start", new Parameter[] { new Parameter("level", level.ToString()) });
    }
    public void LogEventStart(int level, int current_gold, string playType)
    {
        FirebaseAnalytics.LogEvent("level_start", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("current_gold", current_gold.ToString()), new Parameter("play_type", playType) });
    }

    public void LogEventComplete(int level, int timeplayed)
    {
        FirebaseAnalytics.LogEvent("level_complete", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("timeplayed", timeplayed.ToString()) });
    }
    public void LogEventComplete(int level, int timeplayed, string playType)
    {
        FirebaseAnalytics.LogEvent("level_complete", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("timeplayed", timeplayed.ToString()), new Parameter("play_type", playType) });
    }
    public void LogEventFail(int level, int failcount)
    {
        FirebaseAnalytics.LogEvent("level_fail", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("failcount", failcount.ToString()) });
    }
    public void LogEventFail(int level, int failcount, string playType)
    {
        FirebaseAnalytics.LogEvent("level_fail", new Parameter[] { new Parameter("level", level.ToString()), new Parameter("failcount", failcount.ToString()), new Parameter("play_type", playType) });
    }

}


