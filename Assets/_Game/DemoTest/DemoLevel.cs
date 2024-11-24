using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "DemoLevel/Level", order = 1)]
public class DemoLevel : ScriptableObject
{
   public LevelData[] levels;
}

[System.Serializable]
public class LevelData
{
    public int id;
    public Vector2Int mapSize;
    public Box[] boxs;
    public Saw[] saws;
}
