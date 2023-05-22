using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MythicGameManagerMultiplayer : NetworkBehaviour
{

    public static MythicGameManagerMultiplayer Instance { get; private set; }

    private const int MAX_PLAYER_COUNT = 4;

    public string mapDataJson;

    public const int MAX_CHUNK_SIZE = 900;
    private Dictionary<ulong, List<string>> mapDataChunks = new Dictionary<ulong, List<string>>();


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if(SceneManager.GetActiveScene().name != Loader.Scene.Lobby.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started!";
            return;
        }

        if(NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_COUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }

        connectionApprovalResponse.Approved = true;
    }

    public void StartClient()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
        NetworkManager.Singleton.StartClient();
    }

    private void ClientConnected(ulong clientId)
    {
        Debug.Log("About to call RequestToJoinServerRpc...");
        RequestToJoinServerRpc();
    }

    [ClientRpc]
    public void SendMapDataClientRpc(string mapDataJson, ClientRpcParams clientRpcParams = default)
    {
        MapDataClass mapDataClass = JsonUtility.FromJson<MapDataClass>(mapDataJson);


        (List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) mapData = mapDataClass.ToOriginalFormat();

        MythicGameManager.Instance.mapData = mapData;
    }


    [ServerRpc(RequireOwnership = false)]
    public void RequestToJoinServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong senderId = serverRpcParams.Receive.SenderClientId;
        string[] chunks = Chunkify(mapDataJson, MAX_CHUNK_SIZE);
        for (int i = 0; i < chunks.Length; i++)
        {
            SendMapDataChunkServerRpc(i, chunks[i], senderId);
        }
    }


    private string[] Chunkify(string str, int chunkSize)
    {
        int chunkCount = (str.Length + chunkSize - 1) / chunkSize;
        string[] chunks = new string[chunkCount];
        for (int i = 0; i < chunkCount; i++)
        {
            chunks[i] = str.Substring(i * chunkSize, Math.Min(chunkSize, str.Length - i * chunkSize));
        }
        return chunks;
    }


    [ServerRpc(RequireOwnership = false)]
    public void SendMapDataChunkServerRpc(int chunkId, string chunk, ulong targetClientId, ServerRpcParams serverRpcParams = default)
    {
        SendMapDataChunkClientRpc(NetworkManager.Singleton.LocalClientId, chunkId, chunk, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { targetClientId } } });
    }



    [ClientRpc]
    public void SendMapDataChunkClientRpc(ulong senderId, int chunkId, string chunk, ClientRpcParams clientRpcParams = default)
    {

        if (!mapDataChunks.ContainsKey(senderId))
        {
            mapDataChunks[senderId] = new List<string>();
        }

        mapDataChunks[senderId].Add(chunk);

        if (chunk.Length < MAX_CHUNK_SIZE)
        {
            string mapDataJson = string.Join("", mapDataChunks[senderId]);
            mapDataChunks.Remove(senderId);

            MapDataClass mapDataClass = JsonUtility.FromJson<MapDataClass>(mapDataJson);
            (List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) mapData = mapDataClass.ToOriginalFormat();

            MythicGameManager.Instance.mapData = mapData;
        }
    }


}
