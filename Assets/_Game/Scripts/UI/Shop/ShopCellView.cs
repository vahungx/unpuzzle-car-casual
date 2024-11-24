using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using EnhancedUI;
using EasyUI.PickerWheelUI;

namespace EnhancedScrollerDemos.SuperSimpleDemo
{
    public class ShopCellView : EnhancedScrollerCellView
    {
        [Header("Panel")]
        [SerializeField] private GameObject rewardPanel;
        [SerializeField] private GameObject bigPanel;
        [SerializeField] private GameObject smallPanel;

        [Header("Other")]
        [SerializeField] private BigPackageController bigPackageController;
        [SerializeField] private Sprite legendFrame;
        [SerializeField] private Sprite defaultFrame;
        [SerializeField] private Image bigPackageBackground;
        [SerializeField] private SmallCellViewController[] rowCellViews;
        [SerializeField] private GameObject getRewardPopup;
        private ShopScrollerData _data;
        private void Start()
        {
        }

        public void SetData(ShopScrollerData data)
        {
            _data = data;
            if (data == null)
            {
                gameObject.SetActive(false);
                return;
            }

            switch (data.type)
            {
                case ShopPackageType.RewardPackage:
                    rewardPanel.SetActive(true);
                    bigPanel.SetActive(false);
                    smallPanel.SetActive(false);
                    break;
                case ShopPackageType.BigPackage:
                    bigPackageBackground.sprite = (data.name != "Legendary Bundle") ? defaultFrame : legendFrame;
                    bigPanel.SetActive(true);
                    smallPanel.SetActive(false);
                    rewardPanel.SetActive(false);
                    bigPackageController.SetUp(data);
                    break;
            }

        }

        public void SetData(ref List<ShopScrollerData> data, int startingIndex)
        {
            smallPanel.SetActive(true);
            rewardPanel.SetActive(false);
            bigPanel.SetActive(false);
            for (var i = 0; i < rowCellViews.Length; i++)
            {
                rowCellViews[i].SetData(startingIndex + i < data.Count ? data[startingIndex + i] : null);
            }
        }

        public void OnClickGetRewards()
        {
            DataMananger.instance.LoadData();
            //if ads true || iap true
            if (_data.type == ShopPackageType.BigPackage)
            {
                GameObject popup = Instantiate(getRewardPopup, GameplayCanvasController.instance.transform);
                popup.GetComponent<GetRewardPopupController>().SetInfo(_data);
            }
            else
            {
                foreach (Reward reward in _data.rewards)
                {
                    switch (reward.id)
                    {
                        case (int)PackageRewardID.Diamond:
                            DataMananger.instance.gameSave.currentDiamonds += reward.amount;
                            break;
                        case (int)PackageRewardID.Bomb:
                            DataMananger.instance.gameSave.bombBooster += reward.amount;
                            break;
                        case (int)PackageRewardID.Hammer:
                            DataMananger.instance.gameSave.hammerBooster += reward.amount;
                            break;
                    }
                }
                DataMananger.instance.SaveGame();
            }
        }
    }
}