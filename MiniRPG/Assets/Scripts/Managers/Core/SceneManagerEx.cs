using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scene sceneType)
    {
        Manager.Close();
        SceneManager.LoadScene(GetSceneName(sceneType));
    }

    string GetSceneName(Define.Scene sceneType)
    {
        return System.Enum.GetName(typeof(Define.Scene), sceneType);
    }

    public void Close()
    {
        CurrentScene.Close();
    }
}
