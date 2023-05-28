using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

/*******************************************************************************

   Nom du fichier: LobbyListEntryUI.cs
   
   Contexte: Cette classe sert a gérer le UI lorsqu'on entre dans un lobby
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class LobbyListEntryUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lobbyNameText;

    [SerializeField]
    private TextMeshProUGUI playerCountText;

    [SerializeField]
    private TextMeshProUGUI keyLevelText;

    private Lobby lobby;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener( () =>
        {
            LobbyManager.Instance.JoinLobby(lobby);
        });
    }

    public void UpdateLobby(Lobby lobby)
    {
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        keyLevelText.text = lobby.Data[LobbyManager.KEY_KEY_LEVEL].Value;
    }
}
