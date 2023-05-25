using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class MythicGameManager : NetworkBehaviour
{
    public static MythicGameManager Instance { get; private set; }

    private Dictionary<ulong, CharacterData> playerCharacterData = new Dictionary<ulong, CharacterData>();

    public (List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) mapData; 


    [SerializeField]
    private Transform berzerkerPrefab, necroPrefab;

    private bool hasLoaded = false;

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
        Transform playerTransform;
        if (hasLoaded) return;
        hasLoaded = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!MythicGameManagerMultiplayer.Instance.playerCharacterClasses.ContainsKey(clientId))
            {
                Debug.Log("No character data found for client ID: " + clientId);
                continue;
            }

            string characterClass = MythicGameManagerMultiplayer.Instance.playerCharacterClasses[clientId];
            if (characterClass == "Berzerker")
            {
                 playerTransform = Instantiate(berzerkerPrefab);
            } 
            else
                //(data.ClassName == "necromancer")
            {
                playerTransform = Instantiate(necroPrefab);
            }
            PlayerUnit player = playerTransform.GetComponent<PlayerUnit>();


            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            Vector3 firstRoomCenter = mapData.Item1[0].center;
            playerTransform.position = firstRoomCenter;

            Debug.Log("player : " + clientId + "spawned at location " + playerTransform.position);

        }
    }


}
