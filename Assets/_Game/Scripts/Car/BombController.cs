using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private TextMeshPro countDownTxt;
    [SerializeField] private GameObject boom;

    public static Action OnExplosion;
    private void OnEnable()
    {

    }
    public void SetInfo(int countDown)
    {
        countDownTxt.text = countDown.ToString();
    }

    public void Drop()
    {
        //Anim
        gameObject.SetActive(false);
    }

    public void Explode()
    {
        //Anim
        if (gameObject.activeSelf) boom.SetActive(true);
        gameObject.SetActive(false);
    }
}
