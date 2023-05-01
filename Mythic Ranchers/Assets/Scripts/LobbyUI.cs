using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance { get; private set; }

    [SerializeField]
    private Transform lobbyPlayerTemplate;

    [SerializeField]
    private Transform container;

    [SerializeField]
    private TextMeshProUGUI lobbyNameText;

    [SerializeField]
    private TextMeshProUGUI playerCountText;

    [SerializeField]
    private TextMeshProUGUI keyLevelText;

    [SerializeField]
    private Button leaveLobbyButton;


    private void Awake()
    {
        Instance = this;

        lobbyPlayerTemplate.gameObject.SetActive(false);

        leaveLobbyButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.LeaveLobby();
        });

    }

    private void Start()
    {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobbyEvent;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobbyEvent;
        LobbyManager.Instance.OnLeaveLobby += OnLeftLobbyEvent;
        LobbyManager.Instance.OnKickFromLobby += OnLeftLobbyEvent;

        HideUI();
    }



    private void UpdateLobbyEvent(object sender, LobbyManager.LobbyEventArgs e)
    {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
        
    }

    private void OnLeftLobbyEvent(object sender, System.EventArgs e)
    {
        ClearLobby();
        HideUI();
    }

    private void UpdateLobby(Lobby lobby)
    {
        Debug.Log("UpdateLobby");
        ClearLobby();

        foreach(Player player in lobby.Players)
        {
            Transform playerTransform = Instantiate(lobbyPlayerTemplate, container);
            playerTransform.gameObject.SetActive(true);
            LobbyPlayerListUI lobbyPlayerListUI = playerTransform.GetComponent<LobbyPlayerListUI>();

            lobbyPlayerListUI.SetKickPlayerButtonVisible(LobbyManager.Instance.IsLobbyHost() && player.Id != AuthenticationService.Instance.PlayerId);

            lobbyPlayerListUI.UpdatePlayer(player);
        }

        lobbyNameText.text = lobby.Name;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

        ShowUI();
    }

    private void ClearLobby()
    {
        foreach(Transform child in container)
        {
            if (child == lobbyPlayerTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        gameObject.SetActive(true);
    }
}
