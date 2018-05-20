using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float mainThrust = 200f;
    [SerializeField] float rcsThrust = 150f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip revEngine;
    [SerializeField] AudioClip WinAudio;
    [SerializeField] AudioClip LoseAudio;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    bool deathstat = false;
    bool winstat = false;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        GameObject.Find("DeathNote").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("WinNote").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Restart").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Continue").transform.localScale = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        WinLoss();
        if (winstat == false && deathstat == false)
        {
            Thrust();
            Rotate();
        }
    }

    void WinLoss()
    {
        if (winstat == true)
        {
            GameObject.Find("Continue").transform.localScale = new Vector3(1, 1, 1);
            GameObject.Find("Restart").transform.localScale = new Vector3(1, 1, 1);
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                winstat = false;
                deathstat = false;
                return;
            }
            else if (Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene(1);
                winstat = false;
                deathstat = false;
                return;
            }
        }
        if (deathstat == true)
        {
            GameObject.Find("Restart").transform.localScale = new Vector3(1, 1, 1);
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(0);
                winstat = false;
                deathstat = false;
            }
        }
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
        mainEngineParticles.Stop();
        successParticles.Play();
        audioSource.PlayOneShot(WinAudio);
        print("You won!");
        GameObject.Find("WinNote").transform.position = transform.position + new Vector3(0, 5, 0);
        GameObject.Find("Restart").transform.position = transform.position + new Vector3(0, -2, 0);
        GameObject.Find("Continue").transform.position = transform.position + new Vector3(0, -7, 0);
        GameObject.Find("WinNote").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("Backdrop").transform.localScale = new Vector3(0, 0, 0);
        winstat = true;
        WinLoss();
    }

    void DeathScreen()
    {
        mainEngineParticles.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(LoseAudio);
        print("You died!");
        GameObject.Find("Restart").transform.position = transform.position + new Vector3(0,-7,0);
        GameObject.Find("DeathNote").transform.position = transform.position;
        GameObject.Find("DeathNote").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("Backdrop").GetComponent<Renderer>().material.mainTexture = Resources.Load("Materials/Red") as Texture;
        deathstat = true;
        WinLoss();
    }

    private void AddThrust2X(float forceThisFrame)
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        rigidBody.AddRelativeForce(Vector3.up * forceThisFrame * 2f);
    }

    private void AddThrust(float forceThisFrame)
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        rigidBody.AddRelativeForce(Vector3.up * forceThisFrame);
    }

    private void AddDownthrust(float forceThisFrame)
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(revEngine);
        rigidBody.AddRelativeForce(Vector3.down * forceThisFrame);
    }

    private void Thrust()
    {
        float forceThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift)) //thrusting
        {
            AddThrust2X(forceThisFrame);
            mainEngineParticles.Play();
        }
        else if (Input.GetKey(KeyCode.Space)) //thrusting
        {
            AddThrust(forceThisFrame);
            mainEngineParticles.Play();
        }
        else if (Input.GetKey(KeyCode.S)) //rotating right regardless of thrusting
        {
            AddDownthrust(forceThisFrame);
            mainEngineParticles.Play();
        }
        else
        {
            StopAllAudio();
            mainEngineParticles.Stop();
        }
    }

    void StopAllAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
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