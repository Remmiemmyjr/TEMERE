using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Manages ingame content in
// the stage (i.e. chests, drops)
// DATE MODIFIED: 5/15/2021
// ===============================

[System.Serializable]
public struct ChestContents
{
    public Weapon[] weapons;
    public ExtraHealth healthItem;
    public int nextWeapon;
    public Weapon GetNextWeapon()
    {
        // if theres another weapon, return the object
        if (nextWeapon < weapons.Length)
        {
            return weapons[nextWeapon++];
        }
        return null;
    }

}
public class ContentManager : MonoBehaviour
{
    public ChestContents possibleItemToSpawnInChest;

    public int slimeHealth;

    public int cyclopsHealth;

    //public int minOrbs;

    //public int maxOrbs;

    
}
