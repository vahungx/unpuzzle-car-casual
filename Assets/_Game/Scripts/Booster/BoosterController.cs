using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    [SerializeField] private GameObject hammer;
    [SerializeField] private ParticleSystem explosionParticleSystem;
    private Box box;
    public static event Action OnWin;
    public static event Action OnHammer;
    private void Start()
    {
        explosionParticleSystem.gameObject.SetActive(false);
        hammer.SetActive(false);
    }
    public void DestroyNearCar()
    {
        AudioManager.instance.Stop();
        AudioManager.instance.Play((int)SoundID.BoosterBomb);
#if EDITORMODE
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
#endif
        box = GetComponent<BaseBox>().box;

        Vector2 currentMapSize = new Vector2(currentBoxes.Count, currentBoxes[0].Count);
        int x = (int)box.position.x;
        int y = (int)(-box.position.y);

        //Anim

        if (y - 1 >= 0)
        {
            DestroyBoxBooster(currentBoxes[x][y - 1]);

            if (x - 1 >= 0)
            {
                DestroyBoxBooster(currentBoxes[x - 1][y - 1]);
            }
        }
        if (y + 1 < currentMapSize.y)
        {
            DestroyBoxBooster(currentBoxes[x][y + 1]);

            if (x + 1 < currentMapSize.x)
            {
                DestroyBoxBooster(currentBoxes[x + 1][y + 1]);
            }
        }
        if (x - 1 >= 0)
        {
            DestroyBoxBooster(currentBoxes[x - 1][y]);
            if (y + 1 < currentMapSize.y)
            {
                DestroyBoxBooster(currentBoxes[x - 1][y + 1]);
            }
        }
        if (x + 1 < currentMapSize.x)
        {
            DestroyBoxBooster(currentBoxes[x + 1][y]);
            if (y - 1 >= 0)
            {
                DestroyBoxBooster(currentBoxes[x + 1][y - 1]);
            }
        }
        StartCoroutine(PlayParticleSystem(explosionParticleSystem, currentBoxes[x][y]));
    }

    private void DestroyBoxBooster(Box box)
    {
        if (box.type != BlockType.Empty && !box.isMoving)
        {
            ChestKeyController chestKeyCtr = box.GetTransform().GetComponent<BaseBox>().GetChestKeyController();
            if (chestKeyCtr.isOn())
            {
                chestKeyCtr.Drop();
            }
            if (box.isLockedItSelf)
            {
                box.isLockedItSelf = false;
                box.GetTransform().GetComponent<BaseBox>().UnlockHiddenCountDownByBooster();
                box.GetTransform().GetComponent<CarController>().ChooseSkin();

            }
            else if (box.isLockedByKey)
            {
                box.isLockedByKey = false;
                box.GetTransform().GetComponent<BaseBox>().UnlockHiddenKeyByBooster();
                box.GetTransform().GetComponent<CarController>().ChooseSkin();
            }
            else if (box.hasBomb)
            {
                box.hasBomb = false;
                box.GetTransform().GetComponent<CarController>().DropBomb();
                box.type = BlockType.Empty;
                DestroyImmediate(box.GetTransform().gameObject);
            }
            else if (box.hasKey)
            {
                box.hasKey = false;
                box.GetTransform().GetComponent<CarController>().DropKey();
                box.type = BlockType.Empty;
                DestroyImmediate(box.GetTransform().gameObject);
            }
            else
            {
                box.type = BlockType.Empty;
                DestroyImmediate(box.GetTransform().gameObject);
            }
        }
        if (box.type != BlockType.Empty)
        {
            box.GetTransform().GetComponent<BaseBox>().SetBoxInfo(box);
        }
    }

    public void DestroyItSelf()
    {
#if EDITORMODE
#else
        List<List<Box>> currentBoxes = LevelManager.instance.currentBoxes;
#endif
        box = GetComponent<BaseBox>().box;

        Vector2 currentMapSize = new Vector2(currentBoxes.Count, currentBoxes[0].Count);
        int x = (int)box.position.x;
        int y = (int)(-box.position.y);
        //Anim 
        StartCoroutine(PlayAnimator(hammer, currentBoxes[x][y]));
    }

    private IEnumerator PlayParticleSystem(ParticleSystem particleSystem, Box box)
    {
        particleSystem.gameObject.SetActive(true);
        yield return new WaitForSeconds(particleSystem.main.duration);
        DataMananger.instance.gameSave.bombBooster--;
        DataMananger.instance.SaveGame();
        GameplayCanvasController.instance.ResetUI();
        DestroyBoxBooster(box);
        if (Constant.Win())
        {
            GameManager.instance.GameState = State.Win;
            OnWin();
        }
        else if (Constant.CantWin())
        {
            GameManager.instance.GameState = State.Pause;
            Constant.RedBlinkListCar(0);
            OnHammer();
        }
    }
    private IEnumerator PlayAnimator(GameObject anim, Box box)
    {
        anim.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.Stop();
        AudioManager.instance.Play((int)SoundID.BoosterHammer);
        DataMananger.instance.gameSave.hammerBooster--;
        DataMananger.instance.SaveGame();
        GameplayCanvasController.instance.ResetUI();
        DestroyBoxBooster(box);
        if (Constant.Win())
        {
            GameManager.instance.GameState = State.Win;
            OnWin();
        }
        else if (Constant.CantWin())
        {
            GameManager.instance.GameState = State.Pause;
            Constant.RedBlinkListCar(0);
            OnHammer();
        }
    }
}
