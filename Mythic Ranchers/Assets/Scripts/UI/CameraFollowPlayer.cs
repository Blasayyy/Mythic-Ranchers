using UnityEngine;

/*******************************************************************************

   Nom du fichier: CameraFollowPlayer.cs
   
   Contexte: Cette classe sert à faire en sorte que la caméra suive le bon joueur
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class CameraFollowPlayer : MonoBehaviour
{
    public static CameraFollowPlayer instance;

    private PlayerUnit player;

    public void SetPlayerUnit(PlayerUnit playerUnit)
    {
        this.player = playerUnit;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (player)
        {
            transform.position = this.player.transform.position;
        }
    }
}
