using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chest", menuName = "Collection Menu/Chest", order = 0)]
public class ChestCollection : ScriptableObject
{
    public List<Chest> chests;
}
