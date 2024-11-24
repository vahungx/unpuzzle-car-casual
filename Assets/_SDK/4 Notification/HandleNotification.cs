#if PLATFORM_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
#if EnableNotificationsAndroid
using Unity.Notifications.Android;
#endif
using UnityEngine;

public class HandleNotification : MonoBehaviour
{
#if EnableNotificationsAndroid
    [SerializeField] NotificationConfigSO dataSO;
    private void Start()
    {
        Invoke("Initialize", 2);
    }
    public void Initialize()
    {
        GleyNotifications.Initialize(true);
        SendNotification();
    }
    public bool IsPermissionGranted()
    {
        return GleyNotifications.IsPermissionGranted();
    }
    public void RequestPermision(Action onComplete)
    {
        //GleyNotifications.RequestPermision((value) => { if (value == PermissionStatus.Allowed) onComplete.Invoke(); });
    }
    public void SendNotification()
    {
        var items = dataSO.pushInfors;
        Debug.Log("------------------");
        for (int i = 0; i < items.Count; i++)
        {
            var inf = items[i];
            var now = DateTime.Now;
            now = now.AddDays(inf.Day);
            now = now.AddHours(inf.Hour);
            var pushTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            TimeSpan timeSpan = pushTime - DateTime.Now;
            GleyNotifications.SendNotification(inf.Title, inf.Description, timeSpan);
        }
        Debug.Log("------------------");

    }
#endif
}
#endif