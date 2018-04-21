﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource[] audioSource;
    AudioSource audio1;
    AudioSource audio2;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rcsThrust = 100f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponents<AudioSource>();
        audio1 = audioSource[0];
        audio2 = audioSource[1];
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
	}

    void OnCollisionEnter(Collision collision){
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Safe object");
                break;
            case "Fuel":
                print("Picked up fuel");
                break;
            default:
                print("You died");
                break;
        }
    }

    private void Thrust()
    {
        float forceThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift)) //thrusting
        {
            if (!audio1.isPlaying)
                audio1.Play();
            rigidBody.AddRelativeForce(Vector3.up * forceThisFrame * 2f);
        }
        else if (Input.GetKey(KeyCode.Space)) //thrusting
        {
            if (!audio1.isPlaying)
                audio1.Play();
            rigidBody.AddRelativeForce(Vector3.up * forceThisFrame);
        }
        else if (Input.GetKey(KeyCode.S)) //rotating right regardless of thrusting
        {
            if (!audio2.isPlaying)
                audio2.Play();
            rigidBody.AddRelativeForce(Vector3.down * forceThisFrame);
        }
        else
        {
            audio1.Stop();
            //audio2.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) //rotating left regardless of thrusting
        {

            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) //rotating right regardless of thrusting
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.S)) //rotating left regardless of thrusting
        {
            transform.Rotate(Vector3.left); //towards user
        }
        else if (Input.GetKey(KeyCode.W)) //rotating right regardless of thrusting
        {
            transform.Rotate(Vector3.right); //away from user

        }
        rigidBody.freezeRotation = false;
    }
}