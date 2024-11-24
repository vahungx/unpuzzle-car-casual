using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELangguage
{
    EN, JA
}
[System.Serializable]
public class LanguageAndContent
{
    public ELangguage code;

}
[System.Serializable]
public class PushInfor
{
    [TableColumnWidth(40, Resizable = false)] public int Day;
    [TableColumnWidth(40, Resizable = false)] public int Hour;
    [TableColumnWidth(60, Resizable = false)] public int Minutes;
    public string Title = string.Empty;
    public string Description = string.Empty;
}
[CreateAssetMenu(fileName = "NotificationConfigSO", menuName = "Notification Config/Notification Config SO", order = 1)]

public class NotificationConfigSO : ScriptableObject
{
    [TableList(DrawScrollView = true)] public List<PushInfor> pushInfors;
}
