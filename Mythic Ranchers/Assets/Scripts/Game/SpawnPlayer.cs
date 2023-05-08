using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnPlayer : MonoBehaviour    
{
    [SerializeField]
    private Transform playerPrefab;

    public void Spawn()
    {
        NetworkManager.Singleton.StartHost();
    }
}
