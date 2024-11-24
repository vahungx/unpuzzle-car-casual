using DG.Tweening;
using EnhancedScrollerDemos.GridSimulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewSkinPopupController : BasePopup
{
    [SerializeField] private SkinCollection skinCollection;
    [SerializeField] private Image skinImg;
    [SerializeField] private Transform lightTransfor;

    private Skin newSkin = null;
    private void Start()
    {
        DataMananger.instance.LoadData();
        newSkin = RandomNewSkin();
        skinImg.sprite = (newSkin == null) ? null : newSkin.elements[Constant.SKIN_DEFAULT].right;
        lightTransfor.DORotate(Vector3.forward * 360f, 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        AudioManager.instance.Stop();
        AudioManager.instance.Play((int)SoundID.Claiming);
    }
    public override void Close()
    {
        base.Close();
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }

    public void OnClickGetSkin()
    {
        //CheckAds
        // if(true)
        //{
        DataMananger.instance.gameSave.currentSkins.Remove(newSkin);
        newSkin.state = SkinState.Onwed;
        DataMananger.instance.gameSave.currentSkinId = newSkin.id;
        DataMananger.instance.gameSave.currentSkins.Add(newSkin);
        DataMananger.instance.SaveGame();
        Destroy(gameObject);
        SceneManager.LoadScene((int)SceneID.Gameplay);
        //}
    }

    private Skin RandomNewSkin()
    {
        Skin newSkin;
        List<Skin> list = new List<Skin>();
        List<Skin> curSkins = DataMananger.instance.gameSave.currentSkins;
        foreach (Skin skin in skinCollection.skins)
        {
            if (skin.id == 0) continue;
            if (skin.SkinContains(curSkins)) continue;
            list.Add(new Skin()
            {
                id = skin.id,
                state = skin.state,
                elements = skin.elements,
            });
        }
        if (list.Count <= 0)
        {
            return null;
        }
        newSkin = list[Random.Range(0, list.Count)];
        newSkin.state = SkinState.CanGet;
        DataMananger.instance.gameSave.currentSkins.Add(newSkin);
        DataMananger.instance.SaveGame();
        return newSkin;
    }
}
