using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutMovesPopupController : BasePopup
{
    public override void Close()
    {
        base.Close();
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }

    public void OnAdsToSkip()
    {
        //Check ads
        //if true
        DataMananger.instance.gameSave.currentLevel++;
        DataMananger.instance.SaveGame();
        SceneManager.LoadScene((int)SceneID.Gameplay);
    }
}
