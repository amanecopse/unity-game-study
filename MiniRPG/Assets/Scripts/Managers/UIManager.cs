using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager
{
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene sceneUI;
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

    public T MakeSubItemUI<T>(string prefabName = null, Transform parent = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(prefabName))
            prefabName = typeof(T).Name;
        GameObject go = Manager.Resource.Instantiate($"UI/UI_SubItem/{prefabName}", parent);
        T subItemUI = Util.GetOrAddComponent<T>(go);
        return subItemUI;
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
        T SceneUI = Util.GetOrAddComponent<T>(go);
        //만약 위에서 Root.tramsform을 부모로 지정 안했으면 go.transform.SetParent(Root.transform);
        return SceneUI;
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
}
