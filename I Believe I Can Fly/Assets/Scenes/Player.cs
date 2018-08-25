using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] float rcsthrust = 100f;
    [SerializeField] float mainthrust = 100f;

    Rigidbody rigidbody;
    AudioSource JetPackAudio;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        JetPackAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Friendly"){
            print("fine");
        }
        else
        {
            print("dead");
        }
    }

        private void Rotate()
    {
        rigidbody.freezeRotation = true; //take manual control of rotation

        float rotationspeed = rcsthrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationspeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationspeed);
        }

        rigidbody.freezeRotation = false; //resume physics control of rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        { //able to thrust and rotate the same time
            rigidbody.AddRelativeForce(Vector3.up * mainthrust);
            if (JetPackAudio.isPlaying == false)
            {
                JetPackAudio.Play();
            }
        }
        else
        {
            JetPackAudio.Stop();
        }
    }
}
