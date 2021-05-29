using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Takes the player to the
// next level in the build
// DATE MODIFIED: 5/26/2021
// ===============================


public class Exit : MonoBehaviour
{
    public GameObject instructions;
    bool pressed = false;

    void Start()
    {
        instructions.SetActive(false);

        Invoke("SetExit", 3);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressed = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            pressed = false;
        }
    }

    void SetExit()
    {
        if (RoomSpawner.GetRoomAt(transform.position) != RoomSpawner.exitRoom)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            instructions.SetActive(true);
        }

        if (other.tag == "Player" && pressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            instructions.SetActive(false);
        }
    }
}
