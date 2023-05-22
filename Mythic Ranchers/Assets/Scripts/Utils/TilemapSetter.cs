using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSetter : MonoBehaviour
{

    [SerializeField] private Tilemap tilemapFloor;
    [SerializeField] private Tilemap tilemapWalls;

    [SerializeField]
    private List<Sprite> moveableSprites, nonMoveableSprites, rollableSprites;

    [SerializeField]
    private GameObject moveablePropPrefab, torchPrefab, rollablePrefab, nonMoveablePrefab;

    void Awake()
    {

        TilemapVisualizer.Instance.SetTilemaps(tilemapFloor, tilemapWalls);
    }

    private void Start()
    {
        (List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) mapData = MythicGameManager.Instance.mapData;
        PaintMap(mapData);

    }


    public void PaintMap((List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) mapData)
    {
        TilemapVisualizer.Instance.Clear();
        TilemapVisualizer.Instance.PaintFloorTiles(mapData.Item3);
        foreach (var position in mapData.Item5)
        {
            float rand = UnityEngine.Random.value;

            if (rand < 0.3)
            {
                PropsSpawner.SpawnSprites(position, moveablePropPrefab, moveableSprites);
            }
            else if (rand < 0.6)
            {
                PropsSpawner.SpawnSprites(position, rollablePrefab, rollableSprites);
            }
            else if (rand < 0.9)
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
            TilemapVisualizer.Instance.PaintSingleBasicWall(wall.Item1, wall.Item2);
        }
        foreach (var wall in wallData.Item2)
        {
            TilemapVisualizer.Instance.PaintSingleCornerWall(wall.Item1, wall.Item2);
        }
    }
}
