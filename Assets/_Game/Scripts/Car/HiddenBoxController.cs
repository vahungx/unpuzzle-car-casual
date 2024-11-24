using DG.Tweening;
using TMPro;
using UnityEngine;

public class HiddenBoxController : MonoBehaviour
{
    [SerializeField] private TextMeshPro countDownTXT;
    private void OnEnable()
    {
        CarController.OnDisableHidden += UnlockHidden;
    }
    private void OnDisable()
    {
        CarController.OnDisableHidden -= UnlockHidden;
    }

    public void UnlockHidden()
    {
        if (gameObject.activeSelf) gameObject.SetActive(false);
    }

    public void Shaking()
    {
        gameObject.transform.DOPunchPosition(Vector3.one, 0.3f);
    }
}
