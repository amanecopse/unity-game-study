using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Manager.Scene.LoadScene(Define.Scene.Game);
    }

    public override void Close()
    {
        Debug.Log("GameScene closed");
    }
}
