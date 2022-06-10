using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    string _text = "아이템";
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
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
        Bind<GameObject>(typeof(GameObjects));
        Get<GameObject>((int)GameObjects.ItemIcon).BindUIEvent((eventData) => { Debug.Log($"{_text} clicked"); }, Define.UIEvent.Click);
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _text;
    }

    public void SetText(string text)
    {
        _text = text;
    }
}
