using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MythicGameManager : NetworkBehaviour
{
    public static MythicGameManager Instance { get; private set; }

    private Dictionary<ulong, CharacterData> playerCharacterData = new Dictionary<ulong, CharacterData>();

    public (List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) mapData; 


    [SerializeField]
    private Transform playerPrefab;

    [SerializeField]
    private TilemapVisualizer tilemapVisualizer;

    [SerializeField]
    private List<Sprite> moveableSprites, nonMoveableSprites, rollableSprites;

    [SerializeField]
    private GameObject moveablePropPrefab, torchPrefab, rollablePrefab, nonMoveablePrefab;


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
        if (hasLoaded) return;
        hasLoaded = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            PlayerUnit player = playerTransform.GetComponent<PlayerUnit>();

            if (playerCharacterData.TryGetValue(clientId, out CharacterData characterData))
            {
                player.SetCharacterData(characterData);
            }

            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            Vector3 firstRoomCenter = mapData.Item1[0].center;
            //playerTransform.position = firstRoomCenter;

        }

        
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(mapData.Item3);
        foreach (var position in mapData.Item5)
        {
            float rand = UnityEngine.Random.value;

            if(rand < 0.3)
            {
                PropsSpawner.SpawnSprites(position, moveablePropPrefab, moveableSprites);
            }
            else if(rand < 0.6)
            {
                PropsSpawner.SpawnSprites(position, rollablePrefab, rollableSprites);
            }
            else if(rand < 0.9)
            {
                PropsSpawner.SpawnSprites(position, nonMoveablePrefab, nonMoveableSprites);
            }
            else
            {
                PropsSpawner.SpawnSprites(position, torchPrefab);
            }
            
        }
        

        (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData = mapData.Item4;

        foreach (var wall in wallData.Item1)
        {
            tilemapVisualizer.PaintSingleBasicWall(wall.Item1, wall.Item2);
        }
        foreach(var wall in wallData.Item2)
        {
            tilemapVisualizer.PaintSingleCornerWall(wall.Item1, wall.Item2);
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
