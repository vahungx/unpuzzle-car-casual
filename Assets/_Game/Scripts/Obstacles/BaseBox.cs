using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BaseBox : MonoBehaviour
{
    public Obstacle obstacle;
    public Box box;

    [SerializeField] private TextMeshPro countDownTxt;
    [SerializeField] private GameObject hiddenCarObj, keyObj, bombObj, motionEffect, chestKeyObj, handObj;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite carBox, lockBox;
    [SerializeField] private GameObject glow;
    public static bool isActivatedBoomb = false;
    public static Action OnLose;

    private bool onClickBack = false;
    public void Awake()
    {
        DataMananger.instance.LoadData();
        SetUp();
        isActivatedBoomb = false;
    }

    private void OnEnable()
    {
        CarController.OnOneCarOut += UpdateLockedCountDown;
        CarController.OnClickCar += UpdateBombCountDown;
        CarController.OnClickCar += TurnOffFirstLevelTut;
        CarController.OnDisableHidden += UnlockHiddenByKey;
        LosePopupController.OnClickBack += DropBomb;
    }

    private void OnDisable()
    {
        CarController.OnOneCarOut -= UpdateLockedCountDown;
        CarController.OnClickCar -= UpdateBombCountDown;
        CarController.OnClickCar -= TurnOffFirstLevelTut;
        CarController.OnDisableHidden -= UnlockHiddenByKey;
        LosePopupController.OnClickBack -= DropBomb;
    }

    public void SetBoxInfo(Box box)
    {
        this.box = box;
    }

    public ChestKeyController GetChestKeyController()
    {
        return chestKeyObj.GetComponent<ChestKeyController>();
    }
    public BombController GetBombController()
    {
        return bombObj.GetComponent<BombController>();
    }

    public KeyController GetKeyController()
    {
        return keyObj.GetComponent<KeyController>();
    }

    public MotionEffect GetMotionEffect()
    {
        return motionEffect.GetComponent<MotionEffect>();
    }

    public void SetUp()
    {
        if (DataMananger.instance.gameSave.currentLevel == 0 && box.position == new Vector2(0, 0))
        {
            handObj.gameObject.SetActive(true);
        }
        if (box.isLockedByKey)
        {
            hiddenCarObj.GetComponent<SpriteRenderer>().sprite = lockBox;
            hiddenCarObj.SetActive(true);
            countDownTxt.gameObject.SetActive(false);
        }
        if (box.isLockedItSelf)
        {
            hiddenCarObj.GetComponent<SpriteRenderer>().sprite = carBox;
            hiddenCarObj.SetActive(true);
            countDownTxt.gameObject.SetActive(true);
            countDownTxt.text = box.lockedCountDown.ToString();
        }
        if (box.hasKey)
        {
            keyObj.gameObject.SetActive(true);
        }
        if (box.hasBomb)
        {
            if (box.bombCountDown < 6) Blink();
            if (box.isLockedItSelf || box.isLockedByKey)
                bombObj.gameObject.SetActive(false);
            else bombObj.gameObject.SetActive(true);
            GetBombController().SetInfo(box.bombCountDown);
        }
#if EDITORMODE
        //ColorSetUp(box.type);
#endif
    }

    private void UpdateLockedCountDown()
    {
        if (box.isLockedItSelf)
        {
            if (box.lockedCountDown > 0) box.lockedCountDown--;
        }

        if (countDownTxt.gameObject.activeSelf) countDownTxt.text = box.lockedCountDown.ToString();
        UnlockHiddenByCountDown();
    }

    private void UpdateBombCountDown()
    {
        if (box.hasBomb && !box.isLockedByKey && !box.isLockedItSelf)
        {
            box.bombCountDown--;
            if (box.bombCountDown >= 0)
            {
                if (box.bombCountDown < 6) Blink();
                if (bombObj.activeSelf) GetBombController().SetInfo(box.bombCountDown);
            }
            if (box.bombCountDown <= 0 && !box.isMoving)
            {
                GetBombController().Explode();
                if (isActivatedBoomb) return;
                GameManager.instance.GameState = State.Lose;
                isActivatedBoomb = true;
                OnLose();
            }
        }
    }

    private void Blink()
    {
        if (!onClickBack)
        {
            glow.SetActive(true);
        }
        else
        {
            glow.SetActive(false);
        }
    }

    public void HiddenBoxShaking()
    {
        if (hiddenCarObj.activeSelf) hiddenCarObj.transform.DOPunchPosition(Vector3.one * 0.1f, 0.3f);
    }
    private void UnlockHiddenByKey()
    {
#if EDITORMODE
                List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
#endif
        if (box.isLockedByKey)
        {
            if (box.hasBomb) bombObj.gameObject.SetActive(true);
            if (hiddenCarObj.activeSelf)
            {
                box.isLockedByKey = false;
                hiddenCarObj.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.7f).OnComplete(() => hiddenCarObj.SetActive(false));
                AudioManager.instance.Stop();
                AudioManager.instance.Play((int)SoundID.UnlocikingHiddenIronBox);
            }
        }
    }

    private void UnlockHiddenByCountDown()
    {
#if EDITORMODE
                List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
#endif
        if ((box.isLockedItSelf && box.lockedCountDown == 0))
        {
            if (box.hasBomb) bombObj.gameObject.SetActive(true);
            if (hiddenCarObj.activeSelf)
            {
                currentBoxes[(int)box.position.x][-(int)box.position.y].isLockedItSelf = false;
                box.isLockedItSelf = false;
                hiddenCarObj.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0f), 0.7f).OnComplete(() => hiddenCarObj.SetActive(false));
                AudioManager.instance.Stop();
                AudioManager.instance.Play((int)SoundID.UnlockingHiddenBox);
            }
        }
    }

    public void UnlockHiddenCountDownByBooster()
    {
#if EDITORMODE
                List<List<Box>> currentBoxes = MapEditor.Instance.currentBoxes;
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
#endif
        if (box.hasBomb) bombObj.gameObject.SetActive(true);
        if (hiddenCarObj.activeSelf)
        {
            box.isLockedItSelf = false;
            hiddenCarObj.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0f), 0.7f).OnComplete(() => hiddenCarObj.SetActive(false));
            AudioManager.instance.Stop();
            AudioManager.instance.Play((int)SoundID.UnlockingHiddenBox);
        }
    }
    public void UnlockHiddenKeyByBooster()
    {
        if (box.hasBomb) bombObj.gameObject.SetActive(true);
        if (hiddenCarObj.activeSelf)
        {
            box.isLockedItSelf = false;
            hiddenCarObj.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0f), 0.7f).OnComplete(() => hiddenCarObj.SetActive(false));
            AudioManager.instance.Stop();
            AudioManager.instance.Play((int)SoundID.UnlockingHiddenBox);
        }
    }
    private void DropBomb()
    {
        if (box.hasBomb)
        {
            if (box.bombCountDown <= 0)
            {
                onClickBack = true;
                box.hasBomb = false;
                box.bombCountDown = 0;
                gameObject.GetComponent<CarController>().ChooseSkin();
                GetMotionEffect().SetTrailColor();
                Blink();
            }
        }
    }

    private void TurnOffFirstLevelTut()
    {
        handObj.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.5f).OnComplete(() => handObj.SetActive(false));
    }
}
