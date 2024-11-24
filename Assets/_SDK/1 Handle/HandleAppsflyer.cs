using AppsFlyerSDK;
using System.Collections.Generic;
using UnityEngine;
public class AppsflyerParams
{
    private string key;
    private string value;
    public AppsflyerParams() { }
    public AppsflyerParams(string key, string value)
    {
        this.key = key;
        this.value = value;
    }
    public string Key => key;
    public string Value => value;

}
public class HandleAppsflyer : unity_base.Singleton<HandleAppsflyer>
{

    public const string AF_INTERS_CALL_SHOW = "af_inters_call_show";
    public const string AF_INTERS_PASSED_CAPPING_TIME = "af_inters_passed_capping_time";
    public const string AF_INTERS_AVAILABLE = "af_inters_available";
    public const string AF_INTERS_DISPLAYED = "af_inters_displayed";
    public const string AF_REWARDED_CALL_SHOW = "af_rewarded_call_show";
    public const string AF_REWARDED_AVAILABLE = "af_rewarded_available";
    public const string AF_REWARDED_DISPLAYED = "af_rewarded_ad_displayed";
    public const string AF_REWARDED_AD_COMPLETED = "af_rewarded_ad_completed";
    public const string AF_INTER_LOAD_FAIL = "af_inter_load_fail";

    

    public const string AF_LEVEL_ACHIEVED = "af_level_achieved";
    public const string AF_LEVEL_ACHIEVED_MINI = "af_level_achieved_mini";

    public const string AF_TUTORIAL_COMPLETION = "af_tutorial_completion";
    public const string AF_ACHIEVEMENT_UNLOCKED = "af_achievement_unlocked";
    public const string AF_COMPLETE_REGISTRATION = "af_complete_registration";
    public const string AF_PURCHASE = "af_purchase";

    public void LogEventWithParam(string eventName, AppsflyerParams arg1)
    {


        System.Collections.Generic.Dictionary<string, string> af_event = new System.Collections.Generic.Dictionary<string, string>();
        af_event.Add(arg1.Key, arg1.Value);
        AppsFlyer.sendEvent(eventName, af_event);
    }
    public void LogEventTutorial(string af_success, string af_tutorial_id, string gameType = "")
    {
        Debug.Log($"Appsflyer EVENT LogEventTutorial: af_success{af_success} / af_tutorial_id:{af_tutorial_id} / gameType:{gameType}");
        System.Collections.Generic.Dictionary<string, string> af_event = new System.Collections.Generic.Dictionary<string, string>();
        af_event.Add("af_success", af_success);
        af_event.Add("af_tutorial_id", af_tutorial_id);
        if (gameType.Length != 0) af_event.Add("game_type", gameType);
        AppsFlyer.sendEvent(AF_TUTORIAL_COMPLETION, af_event);
    }
    public void LogEventLevelAchieved(string af_game_type, string af_level, string af_score)
    {
        Debug.Log($"Appsflyer EVENT LogEventLevelAchieved: af_game_type{af_game_type} / af_level:{af_level} / af_score:{af_score}");
        System.Collections.Generic.Dictionary<string, string> af_event = new System.Collections.Generic.Dictionary<string, string>();
        af_event.Add("af_level", af_level);
        af_event.Add("af_score", af_score);
        af_event.Add("af_game_type", af_game_type);
        AppsFlyer.sendEvent(AF_LEVEL_ACHIEVED_MINI, af_event);
    }
    public void LogEventLevelAchieved(string af_level, string af_score)
    {
        Debug.Log($"Appsflyer EVENT LogEventLevelAchieved: af_level:{af_level} / af_score:{af_score}");
        System.Collections.Generic.Dictionary<string, string> af_event = new System.Collections.Generic.Dictionary<string, string>();
        af_event.Add("af_level", af_level);
        af_event.Add("af_score", af_score);
        AppsFlyer.sendEvent(AF_LEVEL_ACHIEVED, af_event);
    }
    public void LogEventAchievementUnlock(string content_id, string af_level)
    {
        Debug.Log($"Appsflyer EVENT LogEventAchievementUnlock: content_id:{content_id} / af_level:{af_level}");
    }

    public void LogEventPurchase(string af_revenue, string af_currency, string af_quantity, string af_content_id)
    {
        System.Collections.Generic.Dictionary<string, string> af_event = new System.Collections.Generic.Dictionary<string, string>();
        af_event.Add("af_revenue", af_revenue);
        af_event.Add("af_currency", af_currency);
        af_event.Add("af_quantity", af_quantity);
        af_event.Add("af_content_id", af_content_id);
        AppsFlyer.sendEvent(AF_PURCHASE, af_event);
    }
    public void LogEventWithName(string eventName)
    {
        Debug.Log("Appsflyer EVENT " + eventName);
        System.Collections.Generic.Dictionary<string, string> af_event = new System.Collections.Generic.Dictionary<string, string>();
        AppsFlyer.sendEvent(eventName, af_event);
    }

    public void LogAdRevenue(string monetizationNetwork, double eventRevenue, string revenueCurrency, Dictionary<string, string> dic)
    {
        AppsFlyerAdRevenue.logAdRevenue(monetizationNetwork, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, eventRevenue, revenueCurrency, dic);
    }
   
}
