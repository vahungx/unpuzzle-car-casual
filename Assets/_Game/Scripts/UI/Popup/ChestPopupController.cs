using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChestPopupController : BasePopup
{
    [SerializeField] private GameObject winPopupPrefab, handObj;
    [SerializeField] private ChestCollection chestCollection;
    [SerializeField] private List<ChestCellController> chestCells;

    [SerializeField] private List<Image> keys;
    [SerializeField] private Sprite darkKey;
    [SerializeField] private TextMeshProUGUI currentDiamonds;
    private int numClick = 3;
    private List<Chest> chests;
    private bool isLoad = false;
    private void Start()
    {
        chests = new List<Chest>();
        foreach (Chest chest in chestCollection.chests)
        {
            chests.Add(new Chest()
            {
                id = chest.id,
                type = chest.type,
                total = chest.total,
                icon = chest.icon,
                amount = chest.amount,
                rating = chest.rating,
            });
        }
        SetUp();
    }

    private void SetUp()
    {
        DataMananger.instance.LoadData();
        currentDiamonds.text = DataMananger.instance.gameSave.currentDiamonds.ToString();
        if (OnTutorial())
        {
            handObj.SetActive(true);
        }
        isLoad = true;
    }
    private Chest RandomChest()
    {
        List<Chest> chests = this.chests;

        double totalRating = 0;

        double randomValue = new System.Random().NextDouble() * totalRating;

        double cumulativeRating = 0;
        foreach (Chest chest in chests)
        {
            cumulativeRating += (double)chest.rating / 100;
            if (randomValue <= cumulativeRating && chest.total > 0)
            {
                chest.total--;
                return chest;
            }
        }
        return null;

    }

    public void OnClickChest(int id)
    {
        if (!isLoad) return;
        handObj.SetActive(false);
        numClick--;
        keys[numClick].DOColor(new Color(1, 1, 1, 0), 0.3f).OnComplete(() =>
        {
            keys[numClick].color = Color.white;
            keys[numClick].sprite = darkKey;
        });
        Chest chest = RandomChest();
        for (int i = 0; i < chestCells.Count; i++)
        {
            if (id == i)
            {
                chestCells[i].SetUp(chest);
                StartCoroutine(GetRewards(chest.amount));
                DataMananger.instance.gameSave.currentDiamonds += chest.amount;
                DataMananger.instance.SaveGame();
            }
        }
        if (numClick == 0)
        {
            for (int i = 0; i < chestCells.Count; i++)
            {
                chestCells[i].GetComponent<ButtonExtension>().interactable = false;
            }
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator GetRewards(int amount)
    {
        int curDiamonds = DataMananger.instance.gameSave.currentDiamonds;
        int targetDiamonds = curDiamonds + amount;

        yield return new WaitForSeconds(1);
        while (curDiamonds < targetDiamonds)
        {
            curDiamonds++;
            currentDiamonds.text = curDiamonds.ToString();
            yield return null;
        }
    }

    private IEnumerator PlayAnimation()
    {
        //Anim
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        GameManager.instance.GameState = State.Play;
        DataMananger.instance.gameSave.currentLevel++;
        DataMananger.instance.SaveGame();
        Instantiate(winPopupPrefab, GameplayCanvasController.instance.transform);
    }

    private bool OnTutorial()
    {
        return DataMananger.instance.gameSave.currentLevel == 14;
    }
}
