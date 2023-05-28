using Unity.Netcode.Components;

/*******************************************************************************

   Nom du fichier: OwnerNetworkAnimator.cs
   
   Contexte: Cette classe sert a g�rer qui peux envoyer des informations � un
             animator sp�cifique
   
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