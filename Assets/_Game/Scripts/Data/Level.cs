using System;
using UnityEngine;
using Array2DEditor;

[Serializable]
public class Level : ScriptableObject
{
    [SerializeField] private int numberOfMoves;
    [SerializeField] private Vector2Int mapSize;
    [SerializeField] private Array2DBlock mapEnumBlock;
    public Array2DBlock MapEnumBlock { get => mapEnumBlock; set => mapEnumBlock = value; }

    public Vector2Int MapSize { get => mapSize; set => mapSize = value; }
    public int NumberOfMoves { get => numberOfMoves; set => numberOfMoves = value; }
}
