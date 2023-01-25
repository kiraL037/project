using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPosition, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPosition = FindWallsInDirection(floorPosition, Direction2D.cardinalDirectionsList);
        foreach(var position in basicWallPosition)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirection(HashSet<Vector2Int> floorPosition, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPosition = new HashSet<Vector2Int>();
        foreach (var position in floorPosition)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPosition.Contains(neighbourPosition) == false)
                { 
                    wallPosition.Add(neighbourPosition); 
                }
            }
        }
        return wallPosition;
    }

}
