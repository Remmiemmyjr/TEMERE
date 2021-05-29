using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Emily Berg
// OTHER EDITORS: 
// DESC: Defines all specific types
// of rooms
// DATE MODIFIED: 1/20/2021
// ===============================


public class RoomTypeList : MonoBehaviour
{
    // public game object lists to store each type of room that have the specified entrances
    public GameObject[] LeftRooms;

    public GameObject[] RightRooms;

    public GameObject[] TopRooms;

    public GameObject[] BottomRooms;

    public List<GameObject> roomList;
}
