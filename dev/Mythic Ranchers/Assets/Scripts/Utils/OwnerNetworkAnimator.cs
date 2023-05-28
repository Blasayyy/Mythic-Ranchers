using Unity.Netcode.Components;

/*******************************************************************************

   Nom du fichier: OwnerNetworkAnimator.cs
   
   Contexte: Cette classe sert a gérer qui peux envoyer des informations à un
             animator spécifique
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class OwnerNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}