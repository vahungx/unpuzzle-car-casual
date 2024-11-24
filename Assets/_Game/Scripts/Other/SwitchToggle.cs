using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform uiHandleRectTransform;
    [SerializeField] private Color off, on;
    [SerializeField] private Image backgroundToggle;
    [SerializeField] private Toggle toggle;
    Vector2 handlePosition;

    private void Start()
    {
        handlePosition = uiHandleRectTransform.anchoredPosition;
        SetUpToggle(toggle.isOn);
    }

    public Toggle GetToggle() { return toggle; }

    public void OnSwitch(bool on)
    {
        uiHandleRectTransform.DOAnchorPos(on ? new Vector2(handlePosition.x * -1, handlePosition.y) : handlePosition, 0.5f).SetEase(Ease.InOutBack);
        backgroundToggle.DOColor(on ? this.on : this.off, 0.5f);
    }

    private void SetUpToggle(bool on)
    {
        uiHandleRectTransform.anchoredPosition = on ? new Vector2(handlePosition.x * -1, handlePosition.y) : handlePosition;
        backgroundToggle.color = on ? this.on : this.off;
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}