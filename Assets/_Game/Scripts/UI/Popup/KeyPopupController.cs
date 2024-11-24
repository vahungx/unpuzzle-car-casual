using AssetKits.ParticleImage;
using DG.Tweening;
using NSubstitute.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPopupController : MonoBehaviour
{
    [SerializeField] private Image key1, key2, key3;
    [SerializeField] private Sprite lightKey;

    [SerializeField] ParticleImage keyParticleImage;
    [SerializeField] private GameObject keyChest,container;
    private void Start()
    {
        DataMananger.instance.LoadData();
        keyParticleImage.gameObject.SetActive(false);
        SetUp();
    }

    private void SetUp()
    {
        StartCoroutine(ActiveKeyContinuous());
        AudioManager.instance.Play((int)SoundID.Claiming);
        StartCoroutine(DestroyPopup());
    }

    private IEnumerator DestroyPopup()
    {
        yield return new WaitForSeconds(2);
        container.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 2);
        key1.DOColor(new Color(1, 1, 1, 0), 2);
        key2.DOColor(new Color(1, 1, 1, 0), 2);
        key3.DOColor(new Color(1, 1, 1, 0), 2).OnComplete(() => Destroy(gameObject));
    }

    private IEnumerator ActiveKeyContinuous()
    {
        keyParticleImage.gameObject.SetActive(true);
        keyParticleImage.gameObject.transform.position = GameplayCanvasController.instance.keyChestPos;
        keyChest.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f).SetEase(Ease.Linear).OnComplete(() => keyChest.SetActive(false));
        if (DataMananger.instance.gameSave.chestKey == 0)
        {
            keyParticleImage.attractorTarget = key1.transform;
            yield return new WaitForSeconds(1);
            key1.sprite = lightKey;
            key1.color = new Color(1, 1, 1, 0.3f);
            key1.DOColor(Color.white, 0.5f);
        }
        else if (DataMananger.instance.gameSave.chestKey == 1)
        {
            key1.sprite = lightKey;
            keyParticleImage.attractorTarget = key2.transform;
            yield return new WaitForSeconds(1);
            key2.sprite = lightKey;
            key2.color = new Color(1, 1, 1, 0.3f);
            key2.DOColor(Color.white, 0.5f);
        }
        else if (DataMananger.instance.gameSave.chestKey == 2)
        {
            key1.sprite = lightKey;
            key2.sprite = lightKey;
            keyParticleImage.attractorTarget = key3.transform;
            yield return new WaitForSeconds(1);
            key3.sprite = lightKey;
            key3.color = new Color(1, 1, 1, 0.3f);
            key3.DOColor(Color.white, 0.5f);
        }
    }

}
