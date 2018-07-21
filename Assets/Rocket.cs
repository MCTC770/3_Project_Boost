using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rigidbodyRocket;
	AudioSource rocketThrustSound;
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float rcsRotation = 100f;

	// Use this for initialization
	void Start () {
		rigidbodyRocket = GetComponent<Rigidbody>();
		rocketThrustSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput();
	}

	private void ProcessInput()
	{
		Thrust();
		Rotate();
	}

	private void Thrust()
	{
		float thrustSpeed = rcsThrust * Time.deltaTime;

		if (Input.GetKey(KeyCode.Space))
		{
			rigidbodyRocket.AddRelativeForce(Vector3.up * thrustSpeed);
			if (!rocketThrustSound.isPlaying)
			{
				rocketThrustSound.Play();
			}
		}
		else
		{
			rocketThrustSound.Pause();
		}
	}

	private void Rotate()
	{
		float rotationSpeed = rcsRotation * Time.deltaTime;

		rigidbodyRocket.freezeRotation = true;

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Rotate(Vector3.forward * rotationSpeed);
		}
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			transform.Rotate(Vector3.back * rotationSpeed);
		}

		rigidbodyRocket.freezeRotation = false;
	}
}
