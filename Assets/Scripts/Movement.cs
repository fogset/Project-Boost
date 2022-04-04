using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;

    AudioSource audioSource;

    [SerializeField]
    float mainThrust = 1000f;

    [SerializeField]
    float rotationThrust = 1f;

    [SerializeField]
    AudioClip mainEngine;

    [SerializeField]
    ParticleSystem mainBooster;

    [SerializeField]
    ParticleSystem leftBooster;

    [SerializeField]
    ParticleSystem rightBooster;

    bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            rightBooster.Stop();
            leftBooster.Stop();
        }
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            audioSource.Stop();
            mainBooster.Stop();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot (mainEngine);
        }

        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }

    void RotateRight()
    {
        ApplyRoattion(-rotationThrust);

        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
    }

    void RotateLeft()
    {
        ApplyRoattion (rotationThrust);

        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
    }

    void ApplyRoattion(float rotationThisFrame)
    {
        rb.freezeRotation = true; //freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //freezing rotation so we can manually rotate
    }
}
