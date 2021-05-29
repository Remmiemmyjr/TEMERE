using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Picks either a weapon from
// a linear array or extra health
// randomly to occupy the chest when
// opened
// DATE MODIFIED: 5/27/2021
// ===============================

public class Chest : MonoBehaviour
{
    ContentManager info;
    SpriteRenderer sr;
    AudioSource audioSource;
    TextMeshProUGUI descText;
    PlayerController pC;

    public GameObject instructions;
    public GameObject canvas;
    public Image item;
    public Sprite chestClosed;
    public Sprite chestOpen;

    bool opened = false;
    bool pressed = false;
    int type;

    // Start is called before the first frame update
    void Start()
    {
        instructions.SetActive(false);

        canvas.SetActive(false);

        sr = gameObject.GetComponent<SpriteRenderer>();

        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        info = manager.GetComponent<ContentManager>();

        audioSource = GetComponent<AudioSource>();

        descText = canvas.transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        pC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            pressed = true;
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            pressed = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !opened)
        {
            instructions.SetActive(true);
        }
        
        if(other.gameObject.tag == "Player" && pressed && !opened)
        {
            canvas.SetActive(true);
            audioSource.Play();
            OpenChest();
            sr.sprite = chestOpen;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            instructions.SetActive(false);
        }
    }

    void OpenChest()
    {
        instructions.SetActive(false);

        if (!opened)
        {
            // weapon = 1
            // health = 2
            type = Random.Range(1, 2);
            type = Random.Range(1, 3);
            opened = true;

            if(type == 1)
            {
                PickWeapon();
            }

            if(type == 2)
            {
                ExtraHealth();
            }

            Debug.Log(type);
            
        }
    }

    void PickWeapon()
    {
        var weapon = info.possibleItemToSpawnInChest.GetNextWeapon();

        if (weapon == null)
        {
            // out of weapons, give health instead
            ExtraHealth();
        }
        else
        {
            var thing = canvas.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
            thing.sprite = weapon.weaponImage;
            PlayerController.damage = weapon.damage;
            descText.text = $"{weapon.weaponName}\n+{weapon.damage} DMG";
        }
    }

    void ExtraHealth()
    {
        var thing = canvas.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        var hi = info.possibleItemToSpawnInChest.healthItem;
        thing.sprite = hi.healthImage;
        int healthToAdd = Mathf.Min(hi.extraHealth, pC.totalHealth - pC.currHealth);
        if(pC.currHealth < pC.totalHealth)
        {
            pC.AddHealth(healthToAdd);
            descText.text = $"{hi.healthName}\n+{healthToAdd} HP";
        }
        else
        {
            descText.text = $"{hi.healthName}\nBut youre full";
        }
        Debug.Log(pC.totalHealth);
    }

    public void CloseOut()
    {
        canvas.SetActive(false);
    }
}
