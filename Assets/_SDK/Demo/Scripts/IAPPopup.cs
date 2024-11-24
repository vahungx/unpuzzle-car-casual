using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if GleyIAPGooglePlay
public class IAPPopup : MonoBehaviour, IIAPListerner
{

    /// <summary>
    /// Viết sự kiện sau khi mua thành công 1 product nào đó
    /// Tất cả các sự kiện đều thông qua enum ShopProductNames
    /// enum ShopProductNames được thêm bằng cáhc thêm product trong Windown->Glay->Easy IAP-> Add product(phải add xong product mới có)
    /// </summary>
    /// <param name="productName"></param>
    public void OnBuyCompleted(ShopProductNames productName)
    {

    }

    /// <summary>
    /// Đăng ký
    /// </summary>
    void Start()
    {
        HandleIAP.Instance.Register(this);
    }
    /// <summary>
    /// Huỷ đăng ký
    /// </summary>
    private void OnDestroy()
    {
        HandleIAP.Instance.UnRegister(this);
    }
    /// <summary>
    /// Lấy giá
    /// </summary>
    public void GetProduct()
    {
        //HandleIAP.Instance.GetProduct(ShopProductNames.ABC).localizedPriceString
    }
    /// <summary>
    /// Mua sản phẩm nào đó
    /// </summary>
    public void BuyProduct()
    {
        // HandleIAP.Instance.BuyProduct(ShopProductNames.ABC))
    }
    /// <summary>
    /// Kiểm tra xem đã mua sản phầm nào chưa
    /// ví dụ như đã mua RemoveADS rồi thì sẽ phải code phần bật ADSManager.Instance.IsRemoveADS lên để không show ADS nữa
    /// </summary>
    public void HaveBought()
    {
        //  HandleIAP.Instance.HaveBought(ShopProductNames.ABC)
    }

    /// <summary>
    /// Lấy tất cả sản phầm trên store
    /// </summary>
    public void Get1List()
    {
        //  HandleIAP.Instance.ShopProducts;

    }

    /// <summary>
    /// Kiểm tra xem gói thuê (Sub)(thuê 3 ngày, thuê 1 ngày, 1 tháng, 1 năm...) đã hết hạn chưa
    /// </summary>
    public void IsExpired()
    {
        //  HandleIAP.Instance.IsExpired(ShopProductNames.Demo)
    }
}
#endif
