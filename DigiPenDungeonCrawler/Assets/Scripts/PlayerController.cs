using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: Cainos
// DESC: Handles player movement
// and combat
// DATE MODIFIED: 5/17/2021
//
// CREDITS: Player movement from
// Cainos's basic tilemap package
// ===============================

public class PlayerController : MonoBehaviour
{
    public float speed;
    public static float damage = 1;
    public int totalHealth = 15;
    public int currHealth;

    public BoxCollider2D North;
    public BoxCollider2D South;
    public BoxCollider2D East;
    public BoxCollider2D West;
    public HealthBar healthBar;

    public AudioSource HitAudio;
    public AudioSource DamageAudio;
    public AudioSource HealAudio;

    private float attackTime = 0.25f;
    private float attackCounter = 0.25f;
    private bool isAttacking = false;

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if(currHealth == 0)
        {
            currHealth = totalHealth;
        }

        damage = 1;

        healthBar.SetMaxHealth(totalHealth);
        healthBar.SetHealth(currHealth);

        North.gameObject.SetActive(false);
        South.gameObject.SetActive(false);
        East.gameObject.SetActive(false);
        West.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(damage);
            Debug.Log(totalHealth);
        }

        if(currHealth <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dead");
        }

        // MOVEMENT CODE ============================================
        // Developed by Cainos on itch.io, edited by Emmy
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);
        rb.velocity = speed * dir;

        if (rb.velocity == Vector2.zero)
        {
            animator.SetBool("IsMoving", dir.magnitude < 0);
        }


        // ATTACK CODE ==============================================
        // Made by Emmy
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.K))
        {
            // play attack anim
            isAttacking = true;

            animator.SetBool("isAttacking", true);

            HitAudio.PlayOneShot(HitAudio.clip, 1);

            attackCounter = attackTime;

            if (animator.GetInteger("Direction") == 0)
            {
                South.gameObject.SetActive(true);
                //Debug.Log($"{South.isActiveAndEnabled}");
            }

            else if (animator.GetInteger("Direction") == 1)
            {
                North.gameObject.SetActive(true);
                //Debug.Log($"{North.isActiveAndEnabled}");
            }

            if (animator.GetInteger("Direction") == 2)
            {
                East.gameObject.SetActive(true);
                //Debug.Log($"{East.isActiveAndEnabled}");
            }

            else if (animator.GetInteger("Direction") == 3)
            {
                West.gameObject.SetActive(true);
                //Debug.Log($"{West.isActiveAndEnabled}");
            }
        }

        if (isAttacking)
        {
            rb.velocity = Vector2.zero;

            attackCounter -= Time.deltaTime;

            //Invoke("DisableHitbox", 0.25f);

            if(attackCounter < 0)
            {
                DisableHitbox();
            }
        }
    }

    void DisableHitbox()
    {
        North.gameObject.SetActive(false);

        South.gameObject.SetActive(false);

        East.gameObject.SetActive(false);

        West.gameObject.SetActive(false);

        attackCounter = attackTime;

        isAttacking = false;

        animator.SetBool("isAttacking", false);
    }

    public void TakeDamage()
    {
        currHealth--;
        DamageAudio.PlayOneShot(DamageAudio.clip, 1);
        Debug.Log($"players health is now {currHealth}");
        healthBar.SetHealth(currHealth);
    }

    public void AddHealth(int add)
    {
        currHealth += add;
        HealAudio.PlayOneShot(HealAudio.clip, 1);
        Debug.Log($"players health is now {currHealth}");
        healthBar.SetHealth(currHealth);
    }

}
