using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

public class LobbyListUI : MonoBehaviour
{
    public static LobbyListUI Instance { get; private set; }

    [SerializeField]
    private Transform lobbyEntryTemplate;

    [SerializeField]
    private Transform container;

    [SerializeField]
    private Button refreshButton;

    [SerializeField]
    private Button createLobbyButton;


    private void Awake()
    {
        Instance = this;

        lobbyEntryTemplate.gameObject.SetActive(false);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        LobbyManager.Instance.OnLobbyListChanged += LM_OnLobbyListChanged;
        LobbyManager.Instance.OnJoinedLobby += LM_OnJoinedLobby;
        LobbyManager.Instance.OnKickFromLobby += LM_OnJoinedLobby;
        LobbyManager.Instance.OnLeaveLobby += LM_OnLeaveLobby;
    }

    
    private void LM_OnLobbyListChanged(object sender, LobbyManager.OnLobbyListChangedEventArgs e)
    {
        
    }

    private void LM_OnJoinedLobby(object sender, LobbyManager.LobbyEventArgs e)
    {
        HideUI();
    }

    private void LM_OnKickFromLobby(object sender, LobbyManager.LobbyEventArgs e)
    {
        ShowUI();
    }

    private void LM_OnLeaveLobby(object sender, EventArgs e)
    {
        ShowUI();
    }

    private void RefreshLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in container)
        {
            if (child == lobbyEntryTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);

            foreach(Lobby lobby in lobbyList)
            {
                Transform lobbyTransform = Instantiate(lobbyEntryTemplate, container);
                lobbyTransform.gameObject.SetActive(true);
                LobbyListEntryUI lobbyListEntryUI = lobbyTransform.GetComponent<LobbyListEntryUI>();
                lobbyListEntryUI.UpdateLobby(lobby);
            }
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
