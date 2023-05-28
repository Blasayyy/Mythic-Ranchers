using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/*******************************************************************************

   Nom du fichier: MythicGameManager.cs
   
   Contexte: Cette classe sert a gérer le jeu et ses paramètres
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class MythicGameManager : NetworkBehaviour
{
    public static MythicGameManager Instance { get; private set; }

    private Dictionary<ulong, CharacterData> playerCharacterData = new Dictionary<ulong, CharacterData>();

    public (List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) mapData;

    [SerializeField]
    private Transform berzerkerPrefab, necroPrefab, magePrefab;

    [SerializeField]
    private Transform[] enemyPrefabs;

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
            else if (characterClass == "Necromancer")
            {
                playerTransform = Instantiate(necroPrefab);
            }
            else     // mage
            {
                playerTransform = Instantiate(magePrefab);
            }
            PlayerUnit player = playerTransform.GetComponent<PlayerUnit>();


            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            Vector3 firstRoomCenter = mapData.Item1[0].center;
            MythicGameManagerMultiplayer.Instance.PlayerCount.Value += 1;
            playerTransform.position = firstRoomCenter;

            Debug.Log("player : " + clientId + "spawned at location " + playerTransform.position);
        }

        SpawnEnemiesOnMap();
        MythicGameManagerMultiplayer.Instance.TimerCount.Value = CalculateTimer();
        MythicGameManagerMultiplayer.Instance.StartTimer();
    }

    private float CalculateTimer()
    {
        float time = 0;
        int roomsCount = mapData.roomsList.Count;
        int enemiesCount = MythicGameManagerMultiplayer.Instance.EnemiesCount.Value;
        time = 60 * roomsCount + 3 * enemiesCount;
        return time;
    }

    public void SpawnEnemiesOnMap()
    {
        // Retrieve the mapData from your instance.
        var mapData = MythicGameManager.Instance.mapData;

        // Get the list of rooms.
        var roomsList = mapData.roomsList;

        const float enemySpawnPercentage = 0.2f;

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
                Transform enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                if (floorTilesInRoom.Count == 0)
                    break;

                int randomIndex = Random.Range(0, floorTilesInRoom.Count);

                Vector2Int tileToSpawnOn = floorTilesInRoom[randomIndex];

                var enemyTransform = Instantiate(enemyPrefab, new Vector3(tileToSpawnOn.x + 0.5f, tileToSpawnOn.y + 0.5f, 0), Quaternion.identity);
                enemyTransform.GetComponent<NetworkObject>().Spawn();
                MythicGameManagerMultiplayer.Instance.EnemiesCount.Value += 1;

                floorTilesInRoom.RemoveAt(randomIndex);
            }

            MythicGameManagerMultiplayer.Instance.totalEnemyCount = MythicGameManagerMultiplayer.Instance.EnemiesCount.Value;
            MythicGameManagerMultiplayer.Instance.enemiesSpawned.Value = true;
        }
    }
}
