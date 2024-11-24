using Array2DEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Array2DBlock : Array2D<Box>
{
    [SerializeField]
    CellRowBlockEnum[] cells = new CellRowBlockEnum[Consts.defaultGridSize];
    public Array2DBlock(int size)
    {
        cells = new CellRowBlockEnum[size];
        for (int i = 0; i < size; i++)
        {
            cells[i] = new CellRowBlockEnum(size);
        }
    }

    protected override CellRow<Box> GetCellRow(int index)
    {
        return cells[index];
    }

}
[Serializable]
public class CellRowBlockEnum : CellRow<Box>
{
    public CellRowBlockEnum(int size)
    {
        row = new Box[size];
    }
}
