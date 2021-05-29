using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Score :)
// DATE MODIFIED: 5/28/2021
// ===============================


public class Score : MonoBehaviour
{
    public static int score = 0;

    public TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.text = $"score: {score}";
    }
}
