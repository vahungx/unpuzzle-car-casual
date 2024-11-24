using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Skin", menuName = "Collection Menu/Skin", order = 0)]
public class SkinCollection : ScriptableObject
{
    public List<Skin> skins;
}
[Serializable]
public class Skin
{
    public int id;
    public string name;
    public List<SkinElement> elements;
    public SkinState state;

    public bool SkinContains(List<Skin> skins)
    {
        bool contains = false;
        foreach (Skin skin in skins)
        {
            if (skin.id == id)
            {
                contains = true; break;
            }
        }
        return contains;
    }
}

[Serializable]
public class SkinElement
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;
    public Gradient gradient;
}
public enum SkinState
{
    CantGet,
    CanGet,
    Onwed,
}