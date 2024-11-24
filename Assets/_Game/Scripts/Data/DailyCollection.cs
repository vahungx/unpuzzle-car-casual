using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Daily", menuName = "Collection Menu/Daily", order = 0)]
public class DailyCollection : ScriptableObject
{
    public List<Daily> dailys;
}

[Serializable]
public class Daily
{
    public int id;
    public string name;
    public RewardType type;
    public int amount;
    public Sprite icon;
    public bool claimed;
    public DailyState state;
}

public enum DailyState
{
    Claimed,
    CanClaming,
    CantClaming,
    WaitingToClaim,
}
