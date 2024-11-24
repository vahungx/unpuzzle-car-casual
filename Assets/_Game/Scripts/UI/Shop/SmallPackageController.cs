using EnhancedScrollerDemos.SuperSimpleDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPackageController : MonoBehaviour
{
    [SerializeField] private List<SmallCellViewController> smallCellViewControllers;

    //public void SetUp(List<ShopScrollerData> datas)
    //{
    //    for (int i = 0; i < datas.Count; i++)
    //    {
    //        if (datas[i] != null)
    //            smallCellViewControllers[i].SetUp(datas[i]);
    //        else
    //        {
    //            smallCellViewControllers[i].gameObject.SetActive(false);
    //        }
    //    }
    //}
}
