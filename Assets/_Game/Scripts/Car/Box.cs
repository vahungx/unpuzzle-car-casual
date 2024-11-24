using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Box
{
    public BlockType type;
    public Vector2 position;
    public bool isLockedByKey;
    public bool isLockedItSelf;
    public bool hasKey;
    public bool hasBomb;
    public int lockedCountDown;
    public int bombCountDown;

    public bool isMoving = false;

    private Transform transformObject;
    public void SetTransform(Transform transform)
    {
        transformObject = transform;
    }

    public Transform GetTransform()
    {
        return transformObject;
    }
}
