using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject itemPrefab; // The prefab for the item you want to spawn

    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        GameObject loot = Instantiate(itemPrefab, randomPosition, Quaternion.identity);
        //loot.GetComponent<Loot>().Initialize(item);
    }
}
