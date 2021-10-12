using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager s_manager;
    static Manager instance { get { Init(); return s_manager; } }
    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    public static InputManager Input { get { return instance._input; } }
    public static ResourceManager Resource { get { return instance._resource; } }

    void Start()
    {
        Init();
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if (s_manager == null)
        {
            GameObject gameObject;
            gameObject = GameObject.Find("@Manager");
            if (gameObject == null)
            {
                gameObject = new GameObject() { name = "@Manager" };
                gameObject.AddComponent<Manager>();
            }
            DontDestroyOnLoad(gameObject);
            s_manager = gameObject.GetComponent<Manager>();
        }
    }
}