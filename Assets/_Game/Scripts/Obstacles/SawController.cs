using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SawController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite saw;
    [SerializeField] private Transform sawTransform;

    private void Start()
    {
        spriteRenderer.sprite = saw;
    }
    private void Update()
    {
        sawTransform.Rotate(Vector3.forward * Time.deltaTime * 100f);
    }
}
