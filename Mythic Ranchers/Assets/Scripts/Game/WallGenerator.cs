using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static (List<(Vector2Int, string)>, List<(Vector2Int, string)>) CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);
        List<(Vector2Int, string)> basicWalls = CreateBasicWallsMethod(tilemapVisualizer, basicWallPositions, floorPositions);
        List<(Vector2Int, string)> cornerWalls = CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
        

        return (basicWalls, cornerWalls);
    }

    private static List<(Vector2Int, string)> CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        List<(Vector2Int, string)> result = new List<(Vector2Int, string)>();
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }


            //tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
            result.Add((position, neighboursBinaryType));
        }
        return result;
    }

    private static List<(Vector2Int, string)> CreateBasicWallsMethod(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        List<(Vector2Int, string)> result = new List<(Vector2Int, string)>();
        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryValue = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var neighboursPosition = position + direction;
                if (floorPositions.Contains(neighboursPosition))
                {
                    neighboursBinaryValue += "1";
                }
                else
                {
                    neighboursBinaryValue += "0";
                }
            }
            //tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryValue);
            result.Add((position, neighboursBinaryValue));
        }
        return result;
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionsList)
            {
                var neighbourPosition = position + direction;
                if(floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
}
