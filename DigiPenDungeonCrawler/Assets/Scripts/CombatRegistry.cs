using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: takes away health from any
// enemy that collides with the
// players enabled combat hitbox
//
// was also gonna break pots and
// crates but oh well
//
// DATE MODIFIED: 5/26/2021
// ===============================

public class CombatRegistry : MonoBehaviour
{
    bool hit = false;
    Color colorChange = new Vector4(0.96f, 0.4f, 0.4f, 1);

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && !hit)
        {
            other.GetComponent<EnemyProfile>().health -= PlayerController.damage;
            hit = true;
            Debug.Log(other.GetComponent<EnemyProfile>().health);

            var enemy = other.gameObject;

            StartCoroutine(FlashOnHit(enemy));
        }
    }

    IEnumerator FlashOnHit(GameObject other)
    {
        var enemy = other; //.transform.parent.gameObject;
        enemy.GetComponent<SpriteRenderer>().color = colorChange;
        Debug.Log($"{other.name}");
        enemy.transform.localScale *= 0.9f;
        yield return new WaitForSeconds(0.1f);
        enemy.transform.localScale /= 0.9f;
        enemy.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnDisable()
    {
        hit = false;
        transform.localScale = new Vector3();
    }

    private void OnEnable()
    {
        // transform altered only so ontrigger recognizes a change and deals damage upon being enabled more than once
        transform.localScale = new Vector3(1, 1, 1);
    }
}
