using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

/*******************************************************************************

   Nom du fichier: TilemapSetter.cs
   
   Contexte: Cette classe sert à peinturer les bonnes tuiles lors de la création de niveau
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class TilemapSetter : MonoBehaviour
{
    [SerializeField] private Tilemap tilemapFloor;
    [SerializeField] private Tilemap tilemapWalls;
     
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

        if (NetworkManager.Singleton.IsHost)
        {
            SpawnProps(mapData.Item5);
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

    private void SpawnProps(HashSet<Vector2Int> propData)
    {
        PropsSpawner.Instance.SpawnProps(propData);
    }    
}
