using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    Stat _stat;

    enum GameObjects
    {
        HPBar,
    }

    protected override void Init()
    {
        _stat = transform.parent.GetComponent<Stat>();
        Bind<GameObject>(typeof(GameObjects));

    }

    void Update()
    {
        transform.position = transform.parent.position + Vector3.up * (transform.parent.GetComponent<Collider>().bounds.size.y + 0.2f);
        transform.rotation = Camera.main.transform.rotation;

        setHpRatio(_stat.Hp / (float)_stat.MaxHp);
    }

    void setHpRatio(float ratio)
    {
        GetGameObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
