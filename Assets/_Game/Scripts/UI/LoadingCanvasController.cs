using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingCanvasController : MonoBehaviour
{
    [SerializeField] Image fillbar;
    [SerializeField] CanvasGroup canvasGroup;

    #region Singleton
    public static LoadingCanvasController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion
    private void Start()
    {
        DataMananger.instance.LoadData();
        StartCoroutine(IEStart());
    }

    IEnumerator IEStart()
    {
        int currentLevel = DataMananger.instance.gameSave.currentLevel;
        bool isFirstTime = DataMananger.instance.gameSave.isFirstTime;
        //canvasGroup.alpha = 0;
        //DOVirtual.Float(0, 1, 0.2f, (val) => { canvasGroup.alpha = val; });
        bool passAge = DataMananger.instance.gameSave.isSetAge;
        Transform temp = fillbar.transform.parent;
        temp.localScale = Vector3.zero;
        temp.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.25f);
        fillbar.fillAmount = 0;
        fillbar.DOFillAmount(0.1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
#if GleyIAPGooglePlay
        HandleIAP.Instance.Initialize();
#endif
        yield return new WaitForSeconds(0.5f);
        AdsManager.Instance.Init();
        float fillTime1 = 3;
        float fillTime2 = 4;
        CalculateData();
        fillbar.DOFillAmount(0.7f, fillTime1);
        yield return new WaitForSeconds(fillTime1);
        AsyncOperation nextScene;
        if (currentLevel == 0)
        {
            if (isFirstTime)
            {
                //D1 
                DataMananger.instance.gameSave.isFirstTime = false;
                DataMananger.instance.SaveGame();
                nextScene = SceneManager.LoadSceneAsync((int)SceneID.Gameplay, LoadSceneMode.Additive);
            }
            else
            {
                nextScene = SceneManager.LoadSceneAsync((int)SceneID.Gameplay, LoadSceneMode.Additive);
                //No Tut
            }
            if (!passAge)
            {
                SceneManager.LoadSceneAsync((int)SceneID.Age, LoadSceneMode.Additive);
                DataMananger.instance.gameSave.isSetAge = true;
                DataMananger.instance.SaveGame();
            }
        }
        else
        {
            //D2
            //SceneManager.LoadSceneAsync((int)SceneID.)
            nextScene = SceneManager.LoadSceneAsync((int)SceneID.Home, LoadSceneMode.Additive);
        }
        fillbar.DOFillAmount(1f, fillTime2);
        yield return new WaitForSeconds(fillTime2);
        yield return new WaitForSeconds(0.5f);
        while (!nextScene.isDone) yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f);
        //if (!AdsManager.Instance.IsRemoveAds)
        //{
        //    if (PlayerPrefs.GetInt("IsRemoveAds") == 1)
        //    {
        //        AdsManager.Instance.SetIsRemoveAds(true);
        //        AdsManager.Instance.HideBanner();
        //    }
        //    //else if (HandleIAP.Instance.HaveBought(ShopProductNames.RemoveADS))
        //    //{
        //    //    AdsManager.Instance.SetIsRemoveAds(true);
        //    //    AdsManager.Instance.HideBanner();
        //    //}
        //}
        if (passAge) AdsManager.Instance.ShowAOA();
        yield return new WaitForSeconds(0.2f);

        SceneManager.UnloadSceneAsync((int)SceneID.Idle);
        SceneManager.UnloadSceneAsync((int)SceneID.Loading);
        AdsManager.Instance.ShowBanner();
        //TransitionUI.Instance.Hide(() => { });
        //TransitionUI.Instance.Show(() =>
        //{
        //    SceneManager.UnloadSceneAsync(0);
        //    SceneManager.UnloadSceneAsync(1);
        //    TransitionUI.Instance.Hide(() => { });
        //});
    }
    void CalculateData() { }

}
