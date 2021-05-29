using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Controls what is displayed
// on the health bar
// DATE MODIFIED: 5/26/2021
// ===============================


public class HealthBar : MonoBehaviour
{
    public Slider slider;


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth (int health)
    {
        slider.value = health;
    }
}
