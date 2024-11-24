using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughGemsPopupController : BasePopup
{
    public override void Close()
    {
        base.Close();
        GameplayCanvasController.instance.ResetUI();
    }
}
