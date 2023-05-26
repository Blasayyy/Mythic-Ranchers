using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI titleText, dungeonLevelText, expGainText, levelUpText, timeLeftText, lootUnlockedText;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Image lootImage;
    // Start is called before the first frame update
    void Start()
    {
        continueButton.onClick.AddListener(GoToHome);
        SetTextFields();
        levelUpText.gameObject.SetActive(false);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] props = GameObject.FindGameObjectsWithTag("Prop");

        List<GameObject> entitiesToDestroy = new List<GameObject>(enemies);
        entitiesToDestroy.AddRange(players);
        entitiesToDestroy.AddRange(props);

        DespawnAndDeleteEntities(entitiesToDestroy);
    }

    private void SetTextFields()
    {
        int expGained = MythicGameManagerMultiplayer.Instance.totalEnemyCount / 2;
        CharacterData playerData = AccountManager.Instance.CharacterDatas[AccountManager.Instance.SelectedCharacter];
        if (MythicGameManagerMultiplayer.Instance.TimerCount.Value <= 0f)
        {
            titleText.text = "Timer failed!";
        }
        else
        {
            expGained += 100;
            playerData.Current_key += 1;
        }
        dungeonLevelText.text = "Dungeon level: " + MythicGameManagerMultiplayer.Instance.DungeonKeyLevel.Value;
        timeLeftText.text = "Time left: " + MythicGameManagerMultiplayer.Instance.TimerCount.Value;
        if(playerData.Level * 100 <= expGained + playerData.Experience_points)
        {
            levelUpText.gameObject.SetActive(true);
            playerData.Experience_points = (expGained + playerData.Experience_points) - (playerData.Level * 100); 
            playerData.Level += 1;
        }
        else
        {
            playerData.Experience_points += expGained;
        }

        SendCharacterDataToServer(playerData);
    }

    private void DespawnAndDeleteEntities(List<GameObject> entities)
    {
        foreach (GameObject entity in entities)
        {
            // If these are NetworkObjects and you're using Unity Netcode, you should despawn them first
            // before you destroy them.
            if (entity.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
            {
                networkObject.Despawn();
            }

            // Then you can destroy the game object
            Destroy(entity);
        }
    }

    private void GoToHome()
    {
        NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
        NetworkManager.Singleton.Shutdown();

        if (MythicGameManager.Instance != null)
        {
            Destroy(MythicGameManager.Instance.gameObject);
        }

        if (MythicGameManagerMultiplayer.Instance != null)
        {
            Destroy(MythicGameManagerMultiplayer.Instance.gameObject);
        }

        if(NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if(LobbyManager.Instance != null)
        {
            Destroy(LobbyManager.Instance.gameObject);
        }
      
        if(TilemapVisualizer.Instance != null)
        {
            Destroy(TilemapVisualizer.Instance.gameObject);
        }

        if (TooltipFormater.Instance != null)
        {
            Destroy(TooltipFormater.Instance.gameObject);
        }

        if (CursorManager.Instance != null)
        {
            Destroy(CursorManager.Instance.gameObject);
        }


        SceneManager.LoadScene("MenuScene");
    }

    private async void SendCharacterDataToServer(CharacterData playerData)
    {
        bool success = await DatabaseManager.Instance.UpdateCharacter(playerData);
        if (!success)
        {
            Debug.LogError("Failed to update character data in database.");
        }
    }
}
