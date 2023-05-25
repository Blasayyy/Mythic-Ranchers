using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PropsSpawner : MonoBehaviour
{
    public static PropsSpawner Instance { get; set; }


    [SerializeField]
    private List<Sprite> moveableSprites, nonMoveableSprites, rollableSprites;


    [SerializeField]
    private GameObject moveablePropPrefab, torchPrefab, rollablePrefab, nonMoveablePrefab;

    public void Awake()
    {
        Instance = this;
    }


    public void SpawnProps(HashSet<Vector2Int> propData)
    {
        foreach (var position in propData)
        {
            float rand = UnityEngine.Random.value;

            if (rand < 0.3)
            {
                SpawnSprites(position, moveablePropPrefab, moveableSprites);
            }
            else if (rand < 0.6)
            {
                SpawnSprites(position, rollablePrefab, rollableSprites);
            }
            else if (rand < 0.9)
            {
                SpawnSprites(position, nonMoveablePrefab, nonMoveableSprites);
            }
            else
            {
                SpawnSprites(position, torchPrefab);
            }

        }
    }

    public static void SpawnSprites(Vector2Int position, GameObject spriteGameObjectPrefab, List<Sprite> sprites)
    {

        GameObject newGameObject = Instantiate(spriteGameObjectPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        newGameObject.GetComponent<NetworkObject>().Spawn();
    }


    public static void SpawnSprites(Vector2Int position, GameObject spriteGameObjectPrefab)
    {
        GameObject newGameObject = Instantiate(spriteGameObjectPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        newGameObject.GetComponent<NetworkObject>().Spawn();
    }


}
