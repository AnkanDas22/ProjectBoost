using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource[] audioSource;
    AudioSource audio1;
    AudioSource audio2;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rcsThrust = 100f;
    bool deathstat = false;
    bool winstat = false;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponents<AudioSource>();
        audio1 = audioSource[0];
        audio2 = audioSource[1];
        GameObject.Find("DeathNote").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("WinNote").transform.localScale = new Vector3(0, 0, 0);
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
            case "Finish":
                if (deathstat == true || winstat == true)
                    break;
                WinScreen();
                break;
            default:
                if (winstat == true || deathstat == true)
                    break;
                DeathScreen();
                break;
        }
    }

    void WinScreen()
    {
        print("You won!");
        GameObject.Find("WinNote").transform.position = transform.position;
        GameObject.Find("WinNote").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("Backdrop").transform.localScale = new Vector3(0, 0, 0);
        winstat = true;
    }

    void DeathScreen()
    {
        print("You died!");
        GameObject.Find("DeathNote").transform.position = transform.position;
        GameObject.Find("DeathNote").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("Backdrop").GetComponent<Renderer>().material.mainTexture = Resources.Load("Materials/Red") as Texture;
        deathstat = true;
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
            audio2.Stop();
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
