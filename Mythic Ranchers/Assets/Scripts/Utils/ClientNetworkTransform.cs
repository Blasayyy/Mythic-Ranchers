using Unity.Netcode.Components;
using UnityEngine;

/*******************************************************************************

   Nom du fichier: ClientNetworkTransform.cs
   
   Contexte: Cette classe sert a gérer qui peux envoyer des informations à un
             transform spécifique
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    /// Used for syncing a transform with client side changes. This includes host. Pure server as owner isn't supported by this. Please use NetworkTransform
    /// for transforms that'll always be owned by the server.
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        /// Used to determine who can write to this transform. Owner client only.
        /// This imposes state to the server. This is putting trust on your clients. Make sure no security-sensitive features use this transform.
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}