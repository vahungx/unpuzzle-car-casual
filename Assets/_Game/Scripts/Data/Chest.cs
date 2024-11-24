using UnityEngine;
using System;

[Serializable]
public class Chest
{
    public RewardType type;
    public int id;
    public int amount;
    public int total;
    public int rating;
    public Sprite icon;
}

public enum RewardType
{
    Diamonds,
    Booster,
}