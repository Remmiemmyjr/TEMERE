using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Spawns rooms from an origin
// within specified map boundaries,
// indentifies which rooms are
// appropriate to instantiate and
// connect to other rooms.
// DATE MODIFIED: 5/17/2021
// ===============================


public class RoomSpawner : MonoBehaviour
{
    [Flags] public enum OpeningSide {NONE = 0, LEFT = 1, RIGHT = 2, TOP = 4, BOTTOM = 8};

    public OpeningSide typeOfRoom;

    RoomTypeList template;

    private bool spawned = false;

    public int spawnBounds = 4;

    private int random;

    static int roomMapSize;

    int xMap = -1;

    int yMap = -1;

    static GameObject[,] roomMap;
    
    public GameObject exitObj;

    public static GameObject exitRoom;

    static Vector3 heightOffset = new Vector2(0, 22);

    static Vector3 widthOffset = new Vector2(30, 0);

    static Vector3 zOffset = new Vector3(0, 0, 1);


    private void Start()
    {
        template = GameObject.FindGameObjectWithTag("RoomsList").GetComponent<RoomTypeList>();

        // Allows us to call a method with a delay. Collision would not be detected properly if spawned at the same time.
        Invoke("SpawnRooms", 0.1f);
    }
    
    bool CanSpawn(int x, int y)
    {
        // If rooms spawns out of bounds
        if (x < 0 || y < 0 || x >= roomMapSize || y >= roomMapSize)
        {
            return false;
        }

        // If theres already a room there
        if (roomMap[x,y] != null)
        {
            return false;
        }

        // If theres no conflicts, room can spawn in passed in location
        return true;
    }

    private void SpawnARoomIfNeeded(int xDirection, int yDirection, OpeningSide thisOpening, OpeningSide otherOpening, GameObject[] potentialRooms)
    {
        if (typeOfRoom.HasFlag(thisOpening) && CanSpawn(xMap + xDirection, yMap + yDirection))
        {
            List<GameObject> matchingRooms = new List<GameObject>(potentialRooms.Length);

            foreach(GameObject room in potentialRooms)
            {
                if (canPlace(xMap + xDirection, yMap + yDirection, room))
                {
                    matchingRooms.Add(room);
                }
            }

            random = UnityEngine.Random.Range(0, matchingRooms.Count);
            PlaceRoom(xMap + xDirection, yMap + yDirection, Instantiate(matchingRooms[random],
                transform.position + xDirection * widthOffset + yDirection * heightOffset,
                Quaternion.identity));
        }
    }

    private bool canPlace(int x, int y, GameObject room)
    {
        RoomSpawner rS = room.GetComponent<RoomSpawner>();

        bool isOk =
        IsWallMatch(x - 1, y, rS.typeOfRoom & OpeningSide.LEFT, OpeningSide.RIGHT) &&
        IsWallMatch(x + 1, y, rS.typeOfRoom & OpeningSide.RIGHT, OpeningSide.LEFT) &&
        IsWallMatch(x, y + 1, rS.typeOfRoom & OpeningSide.TOP, OpeningSide.BOTTOM) &&
        IsWallMatch(x, y - 1, rS.typeOfRoom & OpeningSide.BOTTOM, OpeningSide.TOP);
        return isOk;
    }

    private bool IsWallMatch(int x, int y, OpeningSide openingSide, OpeningSide openingSideMask)
    {
        bool looking4Wall = openingSide == OpeningSide.NONE;

        if (x < 0 || y < 0 || x >= roomMapSize || y >= roomMapSize)
        {
            // There's no rooms outside the boundary, so the room that called this is 
            // up against a boundary, so we only match a "wall" room
            return looking4Wall;
        }
        else
        {
            if (roomMap[x, y] == null)
            {
                // No room next to this wall so we can match anything
                return true;
            }

            // Need to figure out what wall this room has so we can match it
            RoomSpawner rS = roomMap[x, y].GetComponent<RoomSpawner>();
            var temp = rS.typeOfRoom & openingSideMask;

            if (looking4Wall)
            {
                // Looking for a wall 
                return temp == OpeningSide.NONE;
            }
            else
            {
                // Looking for a door
                return temp != OpeningSide.NONE;
            }
        }
    }

    static RoomSpawner entryRoom;

    void SpawnRooms()
    {

        // Make sure we only spawn one room in a spot
        if (spawned == false)
        {
            // Identifying the first room with this script (the entry room, already placed)
            if (xMap == -1 || yMap == -1)
            {
                // Spawn bound only exists on the first room since its a member var, we need to copy it into a static
                roomMap = new GameObject[spawnBounds, spawnBounds];
                roomMapSize = spawnBounds;
                // This room goes in the middle of the map 
                xMap = spawnBounds / 2;
                yMap = spawnBounds / 2;

                entryRoom = this;

                // adding this to the array
                PlaceRoom(xMap, yMap, this.gameObject);

                // This logic only gets called once, and we only want this exit room method called once.
                Invoke("SetExitRoom", 1.5f);
            }

            SpawnARoomIfNeeded(-1, 0, OpeningSide.LEFT, OpeningSide.RIGHT, template.RightRooms);
            SpawnARoomIfNeeded(+1, 0, OpeningSide.RIGHT, OpeningSide.LEFT, template.LeftRooms);
            SpawnARoomIfNeeded(0, +1, OpeningSide.TOP, OpeningSide.BOTTOM, template.BottomRooms);
            SpawnARoomIfNeeded(0, -1, OpeningSide.BOTTOM, OpeningSide.TOP, template.TopRooms);
            
            spawned = true;

        }

        template.roomList.Add(this.gameObject);
    }
    
    void PlaceRoom(int xMap, int yMap, GameObject room)
    {
        //Debug.Log($"Place Room: {xMap}, {yMap}");

        // stored the room passed in within our array
        roomMap[xMap, yMap] = room;
        RoomSpawner spawner = room.GetComponent<RoomSpawner>();
        // telling the room where its at (re-setting -1)
        spawner.xMap = xMap;
        spawner.yMap = yMap;
    }

    void SetExitRoom()
    {
        // Gets the last room spawned in, so we can turn it into an exit room
        exitRoom = template.roomList[template.roomList.Count - 1];
        //Instantiate(exitObj, exitRoom.transform.position + zOffset, Quaternion.identity);
    }

    public static GameObject GetRoomAt(Vector3 position)
    {
        if(entryRoom == null)
        {
            return null;
        }

        Vector3 distance = position - entryRoom.transform.position;
        float xRoomDelta = distance.x / widthOffset.x;
        float yRoomDelta = distance.y / heightOffset.y;

        int xMap = entryRoom.xMap + (int)Math.Round(xRoomDelta);
        int yMap = entryRoom.yMap + (int)Math.Round(yRoomDelta);

        //Debug.Log($"Getting Room at {xMap},{yMap} from delta {xRoomDelta},{yRoomDelta} distance {distance.x},{distance.y}");
        GameObject room = roomMap[xMap, yMap];

        return room;
    }
}
