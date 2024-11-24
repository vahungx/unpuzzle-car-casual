using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Collection Menu/Shop", order = 1)]
public class ShopCollection : ScriptableObject
{
    public List<ShopPackage> shopPackages;
}
[System.Serializable]
public class ShopPackage
{
    public string name;
    public int id;
    public ShopPackageType type;
    public float cash;
    public List<Reward> rewards;
}

