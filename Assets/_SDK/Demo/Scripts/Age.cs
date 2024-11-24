using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Age : MonoBehaviour
{
    [SerializeField] int currentAge = 2007;
    [SerializeField] TextMeshProUGUI txtLeft, txtCenter, txtRight;
    [SerializeField] GameObject gOK, gDontShare;
    float delayShowTime = 5f, autoNextTime = 15f;
    private void Start()
    {
        Construct();
    }
    void Construct()
    {
        txtLeft.text = (currentAge - 1).ToString();
        txtCenter.text = "----";
        txtRight.text = (currentAge + 1).ToString();
        Invoke(nameof(AutoNext), autoNextTime);
    }
    public void AdjustAge(int value)
    {
        CancelInvoke(nameof(AutoNext));
        CancelInvoke(nameof(ShowButton));
        currentAge += value;
        txtLeft.text = (currentAge - 1).ToString();
        txtCenter.text = (currentAge).ToString();
        txtRight.text = (currentAge + 1).ToString();
        Invoke(nameof(ShowButton), delayShowTime);
    }
    void ShowButton()
    {
        gOK.SetActive(true);
        gDontShare.SetActive(true);
        Invoke(nameof(AutoNext), delayShowTime);
    }
    void AutoNext()
    {
        OK();
    }
    public void OK()
    {
        StartCoroutine(IEShowADS());
    }
    IEnumerator IEShowADS()
    {
        GA_Inter.ShowAd();
        yield return new WaitForSeconds(0.5f);
        AdsManager.Instance.ShowAOA(null, true);
        yield return new WaitForEndOfFrame();
        //AdsManager.Instance.UpdateMRecPosition(EMRecPosition.BottomCenter);
        AdsManager.Instance.HideMREC();
        yield return new WaitForSeconds(0.25f);
        SceneManager.UnloadSceneAsync(2);
    }
}
