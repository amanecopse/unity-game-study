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

        GameObject player = Manager.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.GetComponent<CameraController>().SetCamera(player);

        GameObject sp = new GameObject { name = "SpawningPool" };
        sp.GetOrAddComponent<SpawningPool>();
    }

    public override void Close()
    {
        Debug.Log("GameScene closed");
    }
}
