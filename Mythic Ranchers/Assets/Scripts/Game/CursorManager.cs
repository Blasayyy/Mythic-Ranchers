using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;
    public Texture2D cursorNormal;

    private void Awake()
    {
        Instance = this;
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
        DontDestroyOnLoad(this.gameObject);
    }

}
