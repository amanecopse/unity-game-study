using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabExam : MonoBehaviour
{
    void Start()
    {
        GameObject go = Manager.Resource.Instantiate("Tank");
        //Manager.Resource.Destroy(go, 3.0f);
    }

    void Update()
    {

    }
}
