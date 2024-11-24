using UnityEngine;
using System.Collections;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using TMPro;

namespace EnhancedScrollerDemos.SuperSimpleDemo
{
    public class ShopScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<ShopScrollerData> _bigPackageData;
        private List<ShopScrollerData> _smallPackData;
        private List<ShopScrollerData> _data;

        public EnhancedScroller scroller;
        public EnhancedScrollerCellView cellViewPrefab;
        public int numberOfCellsPerRow;
        [SerializeField] private ShopCollection shopCollection;
        [SerializeField] private TextMeshProUGUI currentDiamonds;
        void Start()
        {
            scroller.Delegate = this;
            currentDiamonds.text = DataMananger.instance.gameSave.currentDiamonds.ToString();
            LoadData();
        }
        private void Update()
        {
            currentDiamonds.text = DataMananger.instance.gameSave.currentDiamonds.ToString();
        }
        private void LoadData()
        {
            _bigPackageData = new List<ShopScrollerData>();
            _smallPackData = new List<ShopScrollerData>();
            _data = new List<ShopScrollerData>();

            for (int i = 0; i < shopCollection.shopPackages.Count; i++)
            {
                ShopPackage data = shopCollection.shopPackages[i];
                if (data.type == ShopPackageType.SmallPackage)
                {
                    _smallPackData.Add(new ShopScrollerData()
                    {
                        type = data.type,
                        id = data.id,
                        name = data.name,
                        cash = data.cash,
                        rewards = data.rewards,
                    });
                }
                else
                {
                    _bigPackageData.Add(new ShopScrollerData()
                    {
                        type = data.type,
                        id = data.id,
                        name = data.name,
                        cash = data.cash,
                        rewards = data.rewards,
                    });
                }
            }
            _data.AddRange(_bigPackageData);
            _data.AddRange(_smallPackData);
            scroller.ReloadData();
        }

        #region EnhancedScroller Handlers
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _bigPackageData.Count + Mathf.CeilToInt((float)_smallPackData.Count / (float)numberOfCellsPerRow);
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return (_data[dataIndex].type == ShopPackageType.RewardPackage) ? 320f : 520f;
        }
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            ShopCellView cellView = scroller.GetCellView(cellViewPrefab) as ShopCellView;
            cellView.name = "Cell " + dataIndex.ToString();
            if (_data[dataIndex].type != ShopPackageType.SmallPackage)
            {
                cellView.SetData(_data[dataIndex]);
            }
            else
            {
                cellView.SetData(ref _data, (dataIndex - _bigPackageData.Count + 1) * numberOfCellsPerRow);
            }

            return cellView;
        }
        #endregion
    }
}
