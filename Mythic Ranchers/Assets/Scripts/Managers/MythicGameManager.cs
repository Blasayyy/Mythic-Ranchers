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

    [SerializeField]
    private Transform ghoulPrefab;

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
            //playerTransform.position = firstRoomCenter;

            Debug.Log("player : " + clientId + "spawned at location " + playerTransform.position);

        }

        SpawnEnemiesOnMap();
    }

    public void SpawnEnemiesOnMap()
    {
        // Retrieve the mapData from your instance.
        var mapData = MythicGameManager.Instance.mapData;

        // Get the list of rooms.
        var roomsList = mapData.roomsList;

        const float enemySpawnPercentage = 0.2f;

        var enemyPrefab = ghoulPrefab;

        for (int roomIndex = 1; roomIndex < roomsList.Count; roomIndex++)
        {
            BoundsInt room = roomsList[roomIndex];

            List<Vector2Int> floorTilesInRoom = new List<Vector2Int>();
            foreach (var tile in mapData.floor)
            {
                bool isTrue = tile.x >= room.xMin && tile.x < room.xMax && tile.y >= room.yMin && tile.y < room.yMax;
                if (isTrue)
                {
                    if (!mapData.propData.Contains(tile))
                    {
                        floorTilesInRoom.Add(tile);
                    }
                }
            }

            int enemiesToSpawn = Mathf.CeilToInt(floorTilesInRoom.Count * enemySpawnPercentage);


            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (floorTilesInRoom.Count == 0)
                    break;

                int randomIndex = Random.Range(0, floorTilesInRoom.Count);

                Vector2Int tileToSpawnOn = floorTilesInRoom[randomIndex];

                var enemyTransform = Instantiate(enemyPrefab, new Vector3(tileToSpawnOn.x, tileToSpawnOn.y, 0), Quaternion.identity);
                enemyTransform.GetComponent<NetworkObject>().Spawn();

                floorTilesInRoom.RemoveAt(randomIndex);
            }
        }
    }



}
