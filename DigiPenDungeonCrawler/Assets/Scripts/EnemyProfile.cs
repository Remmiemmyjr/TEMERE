using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Sets properties for the
// enemy
// DATE MODIFIED: 5/26/2021
// ===============================

public class EnemyProfile : MonoBehaviour
{
    ContentManager info;
    GameObject manager;

    EnemyAI aiController;

    public enum EnemyType { Slime, Cyclops };
    public EnemyType enemy;

    // values determined by information for the stage
    public float health;
    //int orbMin;
    //int orbMax;
    int addScore;


    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        info = manager.GetComponent<ContentManager>();

        aiController = GetComponent<EnemyAI>();

        if (enemy == EnemyType.Slime)
        {
            health = info.slimeHealth;
            addScore = 1;
        }

        if (enemy == EnemyType.Cyclops)
        {
            health = info.cyclopsHealth;
            addScore = 2;
        }

        //orbMin = info.minOrbs;
        //orbMax = info.maxOrbs;
    }

    private void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        Score.score += addScore;
        for (float i = 1f; i > 0; i -= 0.1f)
        {
            transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(this.gameObject);
    }
}