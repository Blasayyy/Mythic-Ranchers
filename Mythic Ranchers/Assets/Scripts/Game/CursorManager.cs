using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Cursor instance;
    public Texture2D cursorNormal;

    private void Awake()
    {        
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
        DontDestroyOnLoad(this.gameObject);
    }

}
