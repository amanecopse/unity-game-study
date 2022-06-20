using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager
{
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI;
    int _order = 10;
    GameObject Root
    {
        get
        {
            GameObject root;
            root = GameObject.Find("@Root");
            if (root == null)
                root = new GameObject() { name = "@Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeWorldSpaceUI<T>(string prefabName = null, Transform parent = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(prefabName))
            prefabName = typeof(T).Name;
        GameObject go = Manager.Resource.Instantiate($"UI/WorldSpace/{prefabName}", parent);
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        return go.GetOrAddComponent<T>();
    }

    public T MakeSubItemUI<T>(string prefabName = null, Transform parent = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(prefabName))
            prefabName = typeof(T).Name;
        GameObject go = Manager.Resource.Instantiate($"UI/UI_SubItem/{prefabName}", parent);
        return go.GetOrAddComponent<T>();
    }

    public T ShowPopupUI<T>(string prefabName = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(prefabName))
            prefabName = typeof(T).Name;
        GameObject go = Manager.Resource.Instantiate($"UI/UI_Popup/{prefabName}", Root.transform);
        T popupUI = Util.GetOrAddComponent<T>(go);
        //만약 위에서 Root.tramsform을 부모로 지정 안했으면 go.transform.SetParent(Root.transform);
        _popupStack.Push(popupUI);
        return popupUI;
    }

    public T ShowSceneUI<T>(string prefabName = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(prefabName))
            prefabName = typeof(T).Name;
        GameObject go = Manager.Resource.Instantiate($"UI/UI_Scene/{prefabName}", Root.transform);
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;
        //만약 위에서 Root.tramsform을 부모로 지정 안했으면 go.transform.SetParent(Root.transform);
        return sceneUI;
    }

    public void ClosePopupUI(UI_Popup popupUI)
    {
        if (_popupStack.Count == 0)
            return;

        if (popupUI != _popupStack.Peek())
        {
            Debug.Log("Failed closePopupUI");
            return;
        }
        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;
        UI_Popup popupUI = _popupStack.Pop();
        Manager.Resource.Destroy(popupUI.gameObject);
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Close()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
