using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MythicGameManager : NetworkBehaviour
{
    public static MythicGameManager Instance { get; private set; }

    private Dictionary<ulong, CharacterData> playerCharacterData = new Dictionary<ulong, CharacterData>();


    [SerializeField]
    private Transform playerPrefab;

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted1;
        }
    }

    public enum ArmorTypes
    {
        Cloth,
        Leather,
        Mail
    }

    private void SceneManager_OnLoadEventCompleted1(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            PlayerUnit player = playerTransform.GetComponent<PlayerUnit>();

            if (playerCharacterData.TryGetValue(clientId, out CharacterData characterData))
            {
                player.SetCharacterData(characterData);
            }

            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    public void AddPlayerCharacterData(ulong clientId, CharacterData characterData)
    {
        if (!playerCharacterData.ContainsKey(clientId))
        {
            playerCharacterData.Add(clientId, characterData);
        }
    }


}
