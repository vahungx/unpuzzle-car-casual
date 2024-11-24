using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using System;
using System.Collections.Generic;

namespace EnhancedScrollerDemos.GridSimulation
{
    public class SkinCellView : EnhancedScrollerCellView
    {
        public SkinRowCellView[] rowCellViews;

        public void SetData(ref List<SkinScrollerData> data, int startingIndex)
        {
            for (var i = 0; i < rowCellViews.Length; i++)
            {
                rowCellViews[i].SetData(startingIndex + i < data.Count ? data[startingIndex + i] : null);
            }
        }
    }
}