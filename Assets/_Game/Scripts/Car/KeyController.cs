using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeyController : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.speed = Random.Range(0.5f, 0.8f);
    }

    public void Drop()
    {
        gameObject.SetActive(false);
    }
}
