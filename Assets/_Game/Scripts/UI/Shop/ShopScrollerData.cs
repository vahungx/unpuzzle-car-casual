using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedScrollerDemos.SuperSimpleDemo
{
    public class ShopScrollerData
    {
        public string someText;
        public ShopPackageType type;
        public int id;
        public string name;
        public float cash;
        public List<Reward> rewards;
    }
}

public enum ShopPackageType
{
    RewardPackage,
    BigPackage,
    SmallPackage,
}
[Serializable]
public class Reward
{
    public int id;
    public string name;
    public int amount;
    public Sprite sprite;
}

public enum PackageRewardID
{
    Diamond,
    Bomb,
    Hammer,
}