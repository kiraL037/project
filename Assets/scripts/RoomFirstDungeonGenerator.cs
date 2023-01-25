using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidht = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidht = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRoom = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = ProceduralGenerationAlgorithm.BinarySpacePart(new BoundsInt((Vector3Int)startPosition,
            new Vector3Int(dungeonWidht, dungeonHeight, 0)), minRoomWidht, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomList);

        List<Vector2Int> roomCenter = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridor = ConnectRooms(roomCenter);
        floor.UnionWith(corridor);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenter)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenter[Random.Range(0, roomCenter.Count)];
        roomCenter.Remove(currentRoomCenter);

        while (roomCenter.Count > 0)
        {
            Vector2Int closeCenter = FindClosestPointto(currentRoomCenter, roomCenter);
            roomCenter.Remove(closeCenter);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closeCenter);
            currentRoomCenter = closeCenter;
            corridor.UnionWith(newCorridor);
        }
        return corridor;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int closeCenter)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y!=closeCenter.y)
        {
            if (closeCenter.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (closeCenter.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x!=closeCenter.x)
        {
           if (closeCenter.x > position.x) 
           {
                position += Vector2Int.right;
           }
           else if (closeCenter.x < position.x)
           {
                position += Vector2Int.left;
           }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointto(Vector2Int currentRoomCenter, List<Vector2Int> roomCenter)
    {
        Vector2Int closeCenter = Vector2Int.zero;
        float lenght = float.MaxValue;
        foreach (var position in roomCenter)
        {
            float currentLenght = Vector2Int.Distance(position, currentRoomCenter);
            if (currentLenght < lenght)
            {
                lenght = currentLenght;
                closeCenter = position;
            }
        }
        return closeCenter;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for(int colm = offset; colm < room.size.x - offset; colm++)
            {
                for(int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(colm, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
