using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager s_manager;
    static Manager instance { get { Init(); return s_manager; } }

    #region Contents

    GameManagerEx _game = new GameManagerEx();

    public static GameManagerEx Game { get { return instance._game; } }

    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _UI = new UIManager();
    SoundManager _sound = new SoundManager();

    public static DataManager Data { get { return instance._data; } }
    public static InputManager Input { get { return instance._input; } }
    public static PoolManager Pool { get { return instance._pool; } }
    public static ResourceManager Resource { get { return instance._resource; } }
    public static SceneManagerEx Scene { get { return instance._scene; } }
    public static UIManager UI { get { return instance._UI; } }
    public static SoundManager Sound { get { return instance._sound; } }
    #endregion

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

            s_manager._data.Init();
            s_manager._sound.Init();
            s_manager._pool.Init();
        }
    }

    public static void Close()
    {
        Input.Close();
        Pool.Close();
        Scene.Close();
        Sound.Close();
        UI.Close();
    }
}
