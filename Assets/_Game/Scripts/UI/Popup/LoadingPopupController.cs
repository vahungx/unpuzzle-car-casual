using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPopupController : BasePopup
{
    [SerializeField] private Image fillSlider;
    [SerializeField] private RectTransform car;

    [SerializeField] private GameObject agePopup;

    private void Start()
    {
        Loading();
    }

    private void Loading()
    {
        fillSlider.fillAmount = 0;
        StartCoroutine(LoadingSlider());
    }

    private IEnumerator LoadingSlider()
    {
        bool passAge = DataMananger.instance.gameSave.isSetAge;
        float widthFill = fillSlider.gameObject.GetComponent<RectTransform>().rect.width;
        float targetPosX = 0;
        float deltaDuration = 0;
        do
        {
            float deltaTime = Time.deltaTime / 2.5f;
            deltaDuration += deltaTime;
            fillSlider.fillAmount += deltaTime;
            targetPosX += deltaTime * widthFill;
            targetPosX = Mathf.Min(targetPosX, widthFill);
            Debug.Log(targetPosX);
            car.anchoredPosition = new Vector3(targetPosX, 0, 0);
            yield return new WaitForSeconds(deltaTime);
            if (fillSlider.fillAmount >= 1)
            {
                Destroy(gameObject);
                GameManager.instance.GameState = State.Play;
            }
        }
        while (fillSlider.fillAmount < 1);
    }
}
