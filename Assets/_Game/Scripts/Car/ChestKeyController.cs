using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestKeyController : MonoBehaviour
{
    [SerializeField] private BaseBox baseBox;
    [SerializeField] private GameObject key;
    private Box box;
    public static event Action OnDrop;
    private void Start()
    {
        SetUp();
    }
    private void SetUp()
    {
        box = baseBox.box;
        if ((DataMananger.instance.gameSave.currentLevel + 1) % 5 != 0) return;
        if (LevelManager.instance.boxChestKey.position == box.position)
            key.SetActive(true);
    }

    public void Drop()
    {
        //Anim
        if (key.activeSelf)
        {
            key.SetActive(false);
            Vector3 keyChestPos = transform.position;
            GameplayCanvasController.instance.keyChestPos = Camera.main.WorldToScreenPoint(keyChestPos);
            OnDrop();
        }
    }
    public bool isOn()
    {
        return key.activeSelf;
    }
}
