using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    [SerializeField]
    int _count = 0;

    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText,
    }

    enum Images
    {
        ItemIcon,
    }

    enum GameObjects
    {
        TestObject,
    }

    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        GameObject pointButton = GetButton((int)Buttons.PointButton).gameObject;
        GameObject scoreText = GetText((int)Texts.ScoreText).gameObject;
        GameObject itemIcon = GetImage((int)Images.ItemIcon).gameObject;

        BindUIEvent(itemIcon, (PointerEventData eventData) => { itemIcon.transform.position = eventData.position; }, Define.UIEvent.Drag);
        pointButton.BindUIEvent((PointerEventData eventData) => { scoreText.GetComponent<Text>().text = $"점수: {++_count}"; }, Define.UIEvent.Click);
    }
}
