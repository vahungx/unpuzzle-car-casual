using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonExtension : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.Stop();
        AudioManager.instance.Play((int)SoundID.TappingButton);
        Vibrator.Vibrate();

        base.OnPointerClick(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        gameObject.transform.DOPunchScale(transform.localScale * 0.1f, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
        });
        base.OnPointerDown(eventData);
    }
}
