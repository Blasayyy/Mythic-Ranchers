using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTileMap;

    [SerializeField]
    private List<TileBase> floorTiles, wallTopTiles, wallSideRightTiles;

    [SerializeField]
    private TileBase pillar, wallSideRight, wallSideLeft, wallBottom, wallFull, wallInnerCornerDownLeft, wallInnerCornerDownRight, wallInnerCornerUpLeft, wallInnerCornerUpRight ,wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintFloorTiles(floorPositions, floorTilemap, floorTiles);
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.pillar.Contains(typeAsInt))
        {
            tile = pillar;
        }
        else if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            float rand = UnityEngine.Random.value;

            if (rand < 0.80f)
            {
                tile = wallTopTiles[0];
            }
            else if (rand < 0.86f)
            {
                tile = wallTopTiles[1];
            }
            else if(rand < 0.93f)
            {
                tile = wallTopTiles[2];
            }
            else
            {
                tile = wallTopTiles[3];
            }
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            float rand = UnityEngine.Random.value;

            if (rand < 0.80f)
            {
                tile = wallSideRightTiles[0];
            }
            else if (rand < 0.86f)
            {
                tile = wallSideRightTiles[1];
            }
            else if (rand < 0.93f)
            {
                tile = wallSideRightTiles[2];
            }
            else
            {
                tile = wallSideRightTiles[3];
            }
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile != null)
        {
            PaintSingleTile(wallTileMap, tile, position);
        }
        
    }

    private void PaintFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, List<TileBase> tiles)
    {
        TileBase tile = null;

        foreach (var position in positions)
        {
            float rand = UnityEngine.Random.value;

            if (rand < 0.45f)
            {
                tile = tiles[0];
            }
            else if (rand < 0.9f)
            {
                tile = tiles[1];
            }
            else if (rand < 0.95f)
            {
                tile = tiles[2];
            }
            else
            {
                tile = tiles[3];
            }
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallInnerCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerUpLeft;
        }
        else if (WallTypesHelper.wallInnerCornerUpRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }

        if (tile != null)
        {
            PaintSingleTile(wallTileMap, tile, position);
        }
    }
}
