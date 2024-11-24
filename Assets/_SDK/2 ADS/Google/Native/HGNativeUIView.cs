using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HGNativeUIView : MonoBehaviour, INativeADViewCallback
{
    [SerializeField] Camera cam;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject container;
    [SerializeField] RawImage imgIcon;
    [SerializeField] RawImage imgADChoices;
    [SerializeField] Text txtHeadline;
    [SerializeField] Text txtCallToAction;
    bool showing = false;
    private void Awake()
    {
        canvas.worldCamera = cam;
    }
    private void Start()
    {
        container.SetActive(false);
    }
    public void ResetView()
    {
        showing = false;
    }
    private void OnDestroy()
    {
        ResetView();
    }
    private void LateUpdate()
    {
        if (!FirebaseRemoteData.NATIVE_AD_ON_OFF) return;
        if (showing) return;
        if (AdsManager.Instance.IsNativeADReady())
        {
            container.SetActive(true);
            AdsManager.Instance.ShowNativeAD(this);
        }
    }
    public void ShowNativeAD(HGNativeADInfor nativeAd, Action<GameObject, GameObject, GameObject, GameObject> registerCallback)
    {
        if (showing) return;
        showing = true;
        imgIcon.texture = nativeAd.Icon;
        imgADChoices.texture = nativeAd.ADIcon;
        txtHeadline.text = nativeAd.Title;
        txtCallToAction.text = nativeAd.Buttontext;
        registerCallback.Invoke(imgIcon.gameObject, txtHeadline.gameObject, imgADChoices.gameObject, txtCallToAction.gameObject);
    }


}
