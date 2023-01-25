using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLenght = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent=0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potencialRoomPosition = new HashSet<Vector2Int>();

        CreateCorridor(floorPosition, potencialRoomPosition);

        HashSet<Vector2Int> roomPositions = CreateRooms(potencialRoomPosition);

        List<Vector2Int> endsCorridor = FindAllEndsCorridor(floorPosition);

        CreateRoomsAtEndsCorridor(endsCorridor, roomPositions);

        floorPosition.UnionWith(roomPositions);

        tilemapVisualizer.PaintFloorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, tilemapVisualizer);
    }

    private void CreateRoomsAtEndsCorridor(List<Vector2Int> endsCorridor, HashSet<Vector2Int> roomFloor)
    {
        foreach (var position in endsCorridor)
        {
            if (roomFloor.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParametrs, position);
                roomFloor.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllEndsCorridor(HashSet<Vector2Int> floorPosition)
    {
        List<Vector2Int> endsCorridor = new List<Vector2Int>();
        foreach (var position in floorPosition)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPosition.Contains(position + direction))
                    neighboursCount++;
            }
            if (neighboursCount == 1)
                endsCorridor.Add(position);
        }
        return endsCorridor; 
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPosition)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomtoCreateCount = Mathf.RoundToInt(potentialRoomPosition.Count * roomPercent);

        List<Vector2Int> roomtoCreate = potentialRoomPosition.OrderBy(x => Guid.NewGuid()).Take(roomtoCreateCount).ToList();

        foreach (var roomPosition in roomtoCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParametrs, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void CreateCorridor(HashSet<Vector2Int> floorPosition, HashSet<Vector2Int> potencialRoomPosition)
    {
        var currentPosition = startPosition;
        potencialRoomPosition.Add(currentPosition);

        for(int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithm.RandomWalkCorridor(currentPosition, corridorLenght);
            currentPosition = corridor[corridor.Count - 1];
            potencialRoomPosition.Add(currentPosition);
            floorPosition.UnionWith(corridor);
        }
    }
}
