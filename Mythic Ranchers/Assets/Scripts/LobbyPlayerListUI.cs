using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

public class LobbyPlayerListUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerText;

    [SerializeField]
    private Button kickPlayerButton;

    private Player player;

    private void Awake()
    {
        kickPlayerButton.onClick.AddListener(KickPlayer);
    }

    public void SetKickPlayerButtonVisible(bool isVisible)
    {
        kickPlayerButton.gameObject.SetActive(isVisible);
    }

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
        
    }

    private void KickPlayer()
    {
        if(player != null)
        {
            LobbyManager.Instance.KickPlayer(player.Id);
        }
    }
}
