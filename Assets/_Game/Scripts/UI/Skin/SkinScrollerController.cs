using UnityEngine;
using System.Collections;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using System.Linq;

namespace EnhancedScrollerDemos.GridSimulation
{
    public class SkinScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public EnhancedScroller scroller;
        public EnhancedScrollerCellView cellViewPrefab;
        public int numberOfCellsPerRow = 3;
        [SerializeField] private SkinCollection skinCollection;
        private List<SkinScrollerData> _data;

        public static SkinScrollerController instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else Destroy(gameObject);
        }
        void Start()
        {
            scroller.Delegate = this;
            LoadData();
        }

        private bool SkinContains(int id, List<Skin> skins)
        {
            bool contains = false;
            foreach (Skin skin in skins)
            {
                if (skin.id == id)
                {
                    contains = true; break;
                }
            }
            return contains;
        }

        private SkinState GetStateById(int id, List<Skin> skins)
        {
            SkinState state = SkinState.CantGet;
            foreach (Skin skin in skins)
            {
                if (id == skin.id)
                {
                    state = skin.state; break;
                }
            }
            return state;
        }

        public void LoadData()
        {
            DataMananger.instance.LoadData();
            List<Skin> skins = skinCollection.skins;
            List<Skin> curSkins = DataMananger.instance.gameSave.currentSkins;

            _data = new List<SkinScrollerData>();

            float delayTime = 0.25f;
            foreach (Skin skin in skins)
            {
                if (SkinContains(skin.id, curSkins))
                {
                    _data.Add(new SkinScrollerData()
                    {
                        id = skin.id,
                        activeSprite = skin.elements[Constant.SKIN_DEFAULT].right,
                        state = GetStateById(skin.id, curSkins),
                        isSelected = (skin.id == DataMananger.instance.gameSave.currentSkinId),
                        openDelayTime = delayTime,
                    });
                }
                else
                {
                    _data.Add(new SkinScrollerData()
                    {
                        id = skin.id,
                        activeSprite = skin.elements[Constant.SKIN_DEFAULT].right,
                        state = skin.state,
                        isSelected = false,
                        openDelayTime = delayTime,
                    });

                }
                delayTime += 0.25f;
            }
            scroller.ReloadData();
        }

        #region EnhancedScroller Handlers


        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)_data.Count / (float)numberOfCellsPerRow);
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 300f;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {

            SkinCellView cellView = scroller.GetCellView(cellViewPrefab) as SkinCellView;

            cellView.name = "Cell " + (dataIndex * numberOfCellsPerRow).ToString() + " to " + ((dataIndex * numberOfCellsPerRow) + numberOfCellsPerRow - 1).ToString();

            cellView.SetData(ref _data, dataIndex * numberOfCellsPerRow);

            return cellView;
        }

        #endregion
    }
}
