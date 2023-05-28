using UnityEngine;

/*******************************************************************************

   Nom du fichier: CursorManager.cs
   
   Contexte: Cette classe sert à changer l'apparence du cursor à travers le programme
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

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
