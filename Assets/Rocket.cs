using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rigidbodyRocket;
	AudioSource rocketThrustSound;

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
		if (Input.GetKey(KeyCode.Space))
		{
			rigidbodyRocket.AddRelativeForce(Vector3.up);
			if (!rocketThrustSound.isPlaying)
			{
				rocketThrustSound.Play();
			}
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			rocketThrustSound.Pause();
		}

		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(Vector3.back);
		}
	}
}
