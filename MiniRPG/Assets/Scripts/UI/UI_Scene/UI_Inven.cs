using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel,
    }

    void Start()
    {
        Init();
    }

    void Update()
    {

    }

    protected override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in gridPanel.transform)
        {
            Manager.Resource.Destroy(child.gameObject);
        }
        for (int i = 0; i < 8; i++)
        {
            UI_Inven_Item invenItem = Manager.UI.MakeSubItemUI<UI_Inven_Item>(parent: gridPanel.transform);
            invenItem.SetText($"아이템{i}");
        }
    }
}
