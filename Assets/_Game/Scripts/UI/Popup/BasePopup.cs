using DG.Tweening;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    [SerializeField] Transform popupTransform;
    [SerializeField] GameObject[] buttonScales;
    [SerializeField] Image[] buttonFades;
    protected virtual void Awake()
    {
        if (popupTransform != null)
            popupTransform.DOPunchScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.InOutBack);
    }

    protected virtual void OnEnable()
    {
        float delayTime = 0.25f;
        for (int i = 0; i < buttonScales.Length; i++)
        {
            if (!buttonScales[i].activeSelf) continue;
            buttonScales[i].transform.localScale = Vector3.zero;
            buttonScales[i].transform.DOScale(Vector3.one, 0.2f).SetDelay(delayTime).OnComplete(() =>
            {
                buttonScales[i].transform.DOPunchScale(Vector3.one * 1.1f, 0.05f).SetEase(Ease.Linear);
            });
            delayTime += 0.25f;
        }
        for (int i = 0; i < buttonFades.Length; i++)
        {
            if (!buttonFades[i].gameObject.activeSelf) continue;
            buttonFades[i].color = new Color(1, 1, 1, 0);
            buttonFades[i].DOColor(Color.white, 0.2f).SetDelay(delayTime).OnComplete(() =>
            {
            });
            delayTime += 0.25f;
        }
    }

    public virtual void Close()
    {
        Destroy(gameObject);
        GameManager.instance.GameState = State.Play;
    }

    public virtual void OnClickMoreOffers()
    {
        Destroy(gameObject);
        GameplayCanvasController.instance.OnClickPanelBtn(1);
    }


}
