using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    protected override void Init()
    {
        Manager.UI.SetCanvas(gameObject, true);
    }

    protected virtual void ClosePopupUI()
    {
        Manager.UI.ClosePopupUI(this);
    }
}
