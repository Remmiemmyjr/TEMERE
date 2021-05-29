using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: Brackeys
// DESC: Manages ingame content in
// the stage (i.e. chests, drops)
// DATE MODIFIED: 5/15/2021
//
// CREDITS: Brackeys 2d pathfinding
// tutorial and A* pathfinder
// package by Aron Granberg
// ===============================

public class EnemyFollowAI : MonoBehaviour
{
    enum States {Idle, Following, Attacking, Home};

    States state = States.Idle;

    public float speed = 5f;

    public float nextWaypointDistance = 1f;

    public float followDistance = 2.5f;

    public float attackRadius = 1;

    //public bool following = false;

    

    Transform target;
    Vector3 home;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;

    [HideInInspector]
    public Rigidbody2D rb;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        home = transform.position;
    }


    float nextUpdateTime = 0;
    void UpdatePath()
    {
        if (Time.time > nextUpdateTime)
        {
            if (seeker.IsDone())
            {
                nextUpdateTime = Time.time + 0.5f;

                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }
    }

    void HomePath()
    {
        if (Time.time > nextUpdateTime)
        {
            if (seeker.IsDone())
            {
                nextUpdateTime = Time.time + 0.5f;

                seeker.StartPath(rb.position, home, OnPathComplete);
            }
        }
    }

    //make on collision check, have it move in random dir and recalc

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        StateMachine();

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 movement = direction * speed;
        rb.velocity = movement;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void StateMachine()
    {
        switch (state)
        {
            case States.Idle:
                if (Vector2.Distance(rb.position, target.position) < followDistance && Vector2.Distance(rb.position, target.position) > attackRadius)
                {
                    state = States.Following;
                    Debug.Log("following");
                }
                break;

            case States.Following:
                UpdatePath();

                if (RoomSpawner.GetRoomAt(target.position) != RoomSpawner.GetRoomAt(this.transform.position))
                {
                    state = States.Home;
                    path.vectorPath.Clear();
                    // temp, will have "return" movement
                    rb.velocity = new Vector2();
                    Debug.Log("not following");
                }

                if (Vector2.Distance(rb.position, target.position) < attackRadius)
                {
                    state = States.Attacking;
                }

                break;

            case States.Attacking:
                state = States.Following;
                break;

            case States.Home:

                HomePath();
                break;
        }
    }

}
