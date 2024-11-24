using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using System;
using DG.Tweening;

namespace EnhancedScrollerDemos.GridSimulation
{
    public class SkinRowCellView : MonoBehaviour
    {
        [SerializeField] private GameObject container, getBtn, chooseImg;
        [SerializeField] private Image skinImg, containerImg;
        [SerializeField] private Sprite activeFrame, deactiveFrame, chooseFrame;
        [SerializeField] private ButtonExtension chooseSkinBtn;

        public static event Action ChooseSkin;
        private SkinScrollerData _data;

        public void SetData(SkinScrollerData data)
        {
            _data = data;
            container.SetActive(data != null);

            if (data != null)
            {
                switch (data.state)
                {
                    case SkinState.CantGet:
                        skinImg.sprite = data.activeSprite;
                        skinImg.color = Color.gray;
                        containerImg.sprite = deactiveFrame;
                        chooseSkinBtn.interactable = false;
                        chooseImg.SetActive(false);
                        getBtn.SetActive(false);
                        break;
                    case SkinState.CanGet:
                        skinImg.sprite = data.activeSprite;
                        skinImg.color = Color.gray;
                        containerImg.sprite = deactiveFrame;
                        chooseSkinBtn.interactable = false;
                        chooseImg.SetActive(false);
                        getBtn.SetActive(true);
                        break;
                    case SkinState.Onwed:
                        getBtn.SetActive(false);
                        skinImg.sprite = data.activeSprite;
                        skinImg.color = Color.white;
                        if (data.isSelected)
                        {
                            containerImg.sprite = chooseFrame;
                            chooseSkinBtn.interactable = false;
                            chooseImg.SetActive(true);
                        }
                        else
                        {
                            containerImg.sprite = activeFrame;
                            chooseImg.SetActive(false);
                            chooseSkinBtn.interactable = true;
                        }
                        break;
                }
            }
        }

        public void OnClickChooseSkin()
        {
            Debug.Log("OnClickChooseSkin");
            if (_data.state == SkinState.Onwed)
            {
                DataMananger.instance.gameSave.currentSkinId = _data.id;
                DataMananger.instance.SaveGame();
                ChooseSkin();
            }
            SkinScrollerController.instance.LoadData();
        }

        public void OnClickGet()
        {
            //CheckAds
            //if (true){
            _data.state = SkinState.Onwed;

            DataMananger.instance.gameSave.currentSkinId = _data.id;
            ChangeSkinState(_data.id);
            DataMananger.instance.SaveGame();
            ChooseSkin();
            SkinScrollerController.instance.LoadData();
            //}
        }

        private void ChangeSkinState(int id)
        {
            foreach (Skin skin in DataMananger.instance.gameSave.currentSkins)
            {
                if (skin.id == id)
                {
                    skin.state = _data.state;
                }
            }
        }
    }
}