using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ETransitionState
{
    GoingIn, In, GoingOut, Out
}
public class TransitionUI : unity_base.Singleton<TransitionUI>
{
    [SerializeField] Transform mask;
    [SerializeField] Canvas canvas;
    [SerializeField] float scaleTime = 0.25f;
    [SerializeField] ETransitionState transitionState = ETransitionState.Out;
    public ETransitionState TransitionState => transitionState;
    Tween tween;

    public void Show(TweenCallback action)
    {
        transitionState = ETransitionState.GoingIn;
        tween?.Kill();
        canvas.enabled = true;
        mask.localScale = new Vector3(30, 30, 30);
        tween = mask.DOScale(0, scaleTime);
        tween.OnComplete(() =>
        {
            action.Invoke();
            transitionState = ETransitionState.In;
        });
    }
    public void Hide(TweenCallback action)
    {
        transitionState = ETransitionState.GoingOut;
        tween?.Kill();
        mask.localScale = new Vector3(0, 0, 0);
        tween = mask.DOScale(30, scaleTime);
        tween.OnComplete(() =>
        {
            action?.Invoke();
            canvas.enabled = false;
            transitionState = ETransitionState.Out;

        });
    }
}
