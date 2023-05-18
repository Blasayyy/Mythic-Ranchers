using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSetter : MonoBehaviour
{

    [SerializeField] private Tilemap tilemapFloor;
    [SerializeField] private Tilemap tilemapWalls;
    // Start is called before the first frame update
    void Start()
    {
        TilemapVisualizer.Instance.SetTilemaps(tilemapFloor, tilemapWalls);
    }

}
