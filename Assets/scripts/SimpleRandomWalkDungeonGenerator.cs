using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected SimpleRandomWalkData randomWalkParametrs;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPosition = RunRandomWalk(randomWalkParametrs, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkData parmeters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        for (int i = 0; i < parmeters.iterations; i++) 
        {
            var path = ProceduralGenerationAlgorithm.SimpleRandomWalk(currentPosition, parmeters.walkLenght);
            floorPosition.UnionWith(path);
            if (parmeters.startRandomEachIteration)
                currentPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
        }
        return floorPosition;
    }
}
