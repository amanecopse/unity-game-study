using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    CursorType _cursorType = CursorType.None;
    Texture2D _attackCursor;
    Texture2D _handCursor;
    int _mouseMoveMask = (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Ground);

    public enum CursorType
    {
        None,
        Attact,
        Hand,
    }

    void Start()
    {
        _attackCursor = Manager.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handCursor = Manager.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, 100.0f, _mouseMoveMask))
        {
            if (raycastHit.transform.gameObject.layer == (int)Define.Layer.Ground)
            {
                if (_cursorType != CursorType.Hand)
                {
                    _cursorType = CursorType.Hand;
                    Cursor.SetCursor(_handCursor, new Vector2(_handCursor.width / 5, 0), CursorMode.Auto);
                }
            }
            else if (raycastHit.transform.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType != CursorType.Attact)
                {
                    _cursorType = CursorType.Attact;
                    Cursor.SetCursor(_attackCursor, new Vector2(_attackCursor.width / 3, 0), CursorMode.Auto);
                }
            }
        }
    }
}
