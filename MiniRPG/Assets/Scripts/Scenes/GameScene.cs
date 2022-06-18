using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;
        Manager.UI.ShowSceneUI<UI_Inven>("UI_Inven");

        gameObject.GetOrAddComponent<CursorController>();
    }

    public override void Close()
    {
        Debug.Log("GameScene closed");
    }
}
