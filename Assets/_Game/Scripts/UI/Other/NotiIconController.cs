using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotiIconController : MonoBehaviour
{
    void Start()
    {

    }
    private void OnEnable()
    {
        Blink();
    }

    private void Blink()
    {
        if (gameObject.activeSelf)
        {
            transform.DOPunchScale(Vector3.one * 0.2f, 0.5f).SetEase(Ease.Linear).SetLoops(-1);
        }
        else
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
        }
    }
}
