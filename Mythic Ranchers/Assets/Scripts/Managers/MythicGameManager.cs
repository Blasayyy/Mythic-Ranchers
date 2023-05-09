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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
