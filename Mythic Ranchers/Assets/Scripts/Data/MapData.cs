using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*******************************************************************************

   Nom du fichier: MapData.cs
   
   Contexte: Cette classe représente les informations d'une map générée aléatoirement
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

[System.Serializable]
public class Vector2IntClass
{
    public int x;
    public int y;

    // Constructor to convert from Vector2Int to this class
    public Vector2IntClass(Vector2Int vector)
    {
        x = vector.x;
        y = vector.y;
    }
}

[System.Serializable]
public class BoundsIntClass
{
    public Vector2IntClass position;
    public Vector2IntClass size;

    // Constructor to convert from BoundsInt to this class
    public BoundsIntClass(BoundsInt bounds)
    {
        position = new Vector2IntClass((Vector2Int)bounds.position);
        size = new Vector2IntClass((Vector2Int)bounds.size);
    }
}

[System.Serializable]
public class WallDataClass
{
    public Vector2IntClass position;
    public string direction;

    // Constructor to convert from (Vector2Int, string) to this class
    public WallDataClass((Vector2Int, string) wallData)
    {
        position = new Vector2IntClass(wallData.Item1);
        direction = wallData.Item2;
    }
}

[System.Serializable]
public class MapDataClass
{
    public List<BoundsIntClass> roomsList;
    public List<Vector2IntClass> roomsCenters;
    public List<Vector2IntClass> floor;
    public List<WallDataClass> basicWallData;
    public List<WallDataClass> cornerWallData;
    public List<Vector2IntClass> propData;

    // Constructor to convert from your original mapData to this class
    public MapDataClass((List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) mapData)
    {
        // Note that you need to use Select to convert from one type of list to another type of list
        this.roomsList = mapData.roomsList.Select(room => new BoundsIntClass(room)).ToList();
        this.roomsCenters = mapData.roomsCenters.Select(center => new Vector2IntClass(center)).ToList();
        this.floor = mapData.floor.Select(tile => new Vector2IntClass(tile)).ToList();
        this.basicWallData = mapData.wallData.Item1.Select(wall => new WallDataClass(wall)).ToList();
        this.cornerWallData = mapData.wallData.Item2.Select(wall => new WallDataClass(wall)).ToList();
        this.propData = mapData.propData.Select(prop => new Vector2IntClass(prop)).ToList();
    }

    public (List<BoundsInt> roomsList, List<Vector2Int> roomsCenters, HashSet<Vector2Int> floor, (List<(Vector2Int, string)>, List<(Vector2Int, string)>) wallData, HashSet<Vector2Int> propData) ToOriginalFormat()
    {
        List<BoundsInt> roomsList = this.roomsList.Select(room => new BoundsInt
        {
            position = (Vector3Int)new Vector2Int(room.position.x, room.position.y),
            size = (Vector3Int)new Vector2Int(room.size.x, room.size.y)
        }).ToList();

        List<Vector2Int> roomsCenters = this.roomsCenters.Select(center => new Vector2Int(center.x, center.y)).ToList();

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>(this.floor.Select(tile => new Vector2Int(tile.x, tile.y)));

        List<(Vector2Int, string)> basicWallData = this.basicWallData.Select(wall => (new Vector2Int(wall.position.x, wall.position.y), wall.direction)).ToList();

        List<(Vector2Int, string)> cornerWallData = this.cornerWallData.Select(wall => (new Vector2Int(wall.position.x, wall.position.y), wall.direction)).ToList();

        HashSet<Vector2Int> propData = new HashSet<Vector2Int>(this.propData.Select(prop => new Vector2Int(prop.x, prop.y)));

        return (roomsList, roomsCenters, floor, (basicWallData, cornerWallData), propData);
    }

}

