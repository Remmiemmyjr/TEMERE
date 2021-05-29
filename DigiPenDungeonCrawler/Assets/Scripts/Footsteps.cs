using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------------------------------------------------
//
// File Name:	MenuButtonLogic.cs
// Author(s):	Jeremy Kings (j.kings)
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------


public class Footsteps : MonoBehaviour
{
    public float timeBetweenSteps = 0.5f;

    public List<AudioClip> audioClips = new List<AudioClip>();

    private AudioSource audioSource = null;

    private Rigidbody2D rb;

    private float timer = 0.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            return;
        }

        if(timer < timeBetweenSteps)
        {
            return;
        }

        PlaySound();
    }

    void PlaySound()
    {
        int numClips = audioClips.Count;
        int randomClipIndex = Random.Range(0, numClips);

        audioSource.clip = audioClips[randomClipIndex];
        audioSource.Play();

        timer = 0.0f;
    }
}
