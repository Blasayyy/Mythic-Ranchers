using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;
using UnityEngine;

/*******************************************************************************

   Nom du fichier: Relay.cs
   
   Contexte: Cette classe sert à ce que les joueurs puissent rejoindre le même relay
             pour tous avoir la connection au serveur approprié
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class Relay : MonoBehaviour
{
    public static Relay Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in");
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            return joinCode;
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {            
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            return joinAllocation;
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);

            return default;
        }
    }

    public async Task<Allocation> AllocateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(LobbyManager.MAX_PLAYERS - 1);

            return allocation;
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);

            return default;
        }
    }
}
