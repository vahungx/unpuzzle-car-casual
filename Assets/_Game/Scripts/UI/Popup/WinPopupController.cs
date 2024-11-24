using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopupController : BasePopup
{
    [SerializeField] private GameObject newSkinPopupPrefab;
    [SerializeField] private Slider newSkinSlider;
    [SerializeField] private SkinCollection skinCollection;

    [SerializeField] private TextMeshProUGUI multiText;
    [SerializeField] private GameObject arrowObj;
    [SerializeField] private List<RectTransform> wayPoints;

    private List<Vector3> wps = new List<Vector3>();
    private int multiId = 0;
    private bool isLoaded = false;

    public void Start()
    {
        DataMananger.instance.LoadData();
        newSkinSlider.value = (float)((DataMananger.instance.gameSave.currentLevel % 15) / 14);
        UpdateNewSkinSlider();
        AudioManager.instance.Stop();
        AudioManager.instance.Play((int)SoundID.Win);
        SetWps();
    }

    private void Update()
    {
        UpdateTextAdsBtn();
    }
    private void UpdateNewSkinSlider()
    {
        float lastLevel = (DataMananger.instance.gameSave.currentLevel - 2) % 15;
        float currentLevel = (DataMananger.instance.gameSave.currentLevel - 1) % 15;
        newSkinSlider.value = (float)lastLevel / 14;
        newSkinSlider.DOValue((float)currentLevel / 14, 1f).SetEase(Ease.InBack);
    }
    public void OnClickGet1()
    {
        DataMananger.instance.gameSave.currentDiamonds++;
        DataMananger.instance.SaveGame();
        if (DataMananger.instance.gameSave.currentLevel % 15 == 0)
        {
            if (skinCollection.skins.Count > DataMananger.instance.gameSave.currentSkins.Count)
                Instantiate(newSkinPopupPrefab, GameplayCanvasController.instance.transform);
            Destroy(gameObject);
            return;
        }
        Destroy(gameObject);
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }

    public void OnClickGetAds()
    {
        int amount = 1;
        if (multiId == 0) amount *= 2;
        if (multiId == 1) amount *= 3;
        if (multiId == 2) amount *= 5;
        //checkAds
        DataMananger.instance.gameSave.currentDiamonds += amount;
        DataMananger.instance.SaveGame();
        if (DataMananger.instance.gameSave.currentLevel % 15 == 0)
        {
            if (skinCollection.skins.Count > DataMananger.instance.gameSave.currentSkins.Count)
                Instantiate(newSkinPopupPrefab, GameplayCanvasController.instance.transform);
            Destroy(gameObject);
            return;
        }
        Destroy(gameObject);
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }

    private void UpdateTextAdsBtn()
    {
        if (!isLoaded) return;
        int id = 0;
        Vector3 arrowPos = arrowObj.transform.position;
        float distance = Mathf.Abs(arrowPos.x - wayPoints[0].position.x);
        for (int i = 0; i < wayPoints.Count; i++)
        {
            if (Mathf.Abs(arrowPos.x - wayPoints[i].position.x) < distance)
            {
                distance = Mathf.Abs(arrowPos.x - wayPoints[i].position.x);
                id = i;
            }
        }
        if (id == 0 || id == 4)
        {
            multiText.text = "Get x2";
            multiId = 0;
        }
        if (id == 1 || id == 3)
        {
            multiText.text = "Get x3";
            multiId = 1;
        }
        else if (id == 2)
        {
            multiText.text = "Get x5";
            multiId = 2;
        }
    }

    private void SetWps()
    {
        for (int i = 0; i < wayPoints.Count; i++)
        {
            Vector3 worldPos = wayPoints[i].TransformPoint(wayPoints[i].localPosition);
            wps.Add(worldPos);
        }
        isLoaded = true;
    }

}
