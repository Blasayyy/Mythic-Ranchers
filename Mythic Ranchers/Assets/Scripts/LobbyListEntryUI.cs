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
    private TextMeshProUGUI gameModeText;

    private Lobby lobby;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener( () =>
        {
            LobbyManager.Instance.JoinLobby(lobby);
        });
    }
}
