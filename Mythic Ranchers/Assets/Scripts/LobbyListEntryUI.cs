using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

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
