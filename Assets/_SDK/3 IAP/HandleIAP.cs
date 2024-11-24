using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.Events;
#if GleyIAPGooglePlay
using UnityEngine.Purchasing;
#endif
#if GleyIAPGooglePlay
public interface IIAPListerner
{
    public void OnBuyCompleted(ShopProductNames productName);
}
#endif

public class HandleIAP : unity_base.Singleton<HandleIAP>
{
#if GleyIAPGooglePlay

    [SerializeField] UnityEvent<ShopProductNames> onBuyComplete;
    List<StoreProduct> shopProducts;
    bool isSuccess = false;
    public List<StoreProduct> ShopProducts => shopProducts;
    public string GetLocalizedPriceString(ShopProductNames shopProduct)
    {
        return IAPManager.Instance.GetLocalizedPriceString(shopProduct);
    }
    public void Initialize()
    {
        IAPManager.Instance.InitializeIAPManager(InitializeResult);
    }
    public void Register(IIAPListerner iAPListerner)
    {
        onBuyComplete.AddListener(iAPListerner.OnBuyCompleted);
    }
    public void UnRegister(IIAPListerner iAPListerner)
    {
        onBuyComplete.RemoveListener(iAPListerner.OnBuyCompleted);
    }
    private void Start()
    {
        shopProducts = new List<StoreProduct>();
    }
    public StoreProduct GetProduct(ShopProductNames shopProduct) => shopProducts.Find(e => IAPManager.Instance.ConvertNameToShopProduct(e.productName) == shopProduct);
    void InitializeResult(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
    {
        if (status == IAPOperationStatus.Success)
        {
            this.shopProducts = shopProducts;
            isSuccess = true;

        }
    }

    public void BuyProduct(ShopProductNames productName)
    {
        if (!isSuccess) return;
        IAPManager.Instance.BuyProduct(productName, OnBuyCompleted);
    }
    void OnBuyCompleted(IAPOperationStatus status, string message, StoreProduct product)
    {
        if (!isSuccess) return;
        onBuyComplete?.Invoke(IAPManager.Instance.ConvertNameToShopProduct(product.productName));
        HandleAppsflyer.Instance.LogEventPurchase(product.price.ToString(), product.isoCurrencyCode, "1", product.idGooglePlay);
    }

    public bool IsExpired(ShopProductNames shopProduct)
    {
        if (!isSuccess) return true;
        SubscriptionInfo info = IAPManager.Instance.GetSubscriptionInfo(shopProduct);
        if (info.isSubscribed() == Result.True) return info.isExpired() == Result.False;
        return true;
    }
    private void RestoreDone()
    {
    }
    public bool HaveBought(ShopProductNames shopProduct)
    {
        return IAPManager.Instance.IsActive(shopProduct);
    }
#endif
}
