using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimtorController : MonoBehaviour
{
    public void EndEvent()
    {
        gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0f), 0.3f).OnComplete(() => gameObject.SetActive(false));
    }
}
