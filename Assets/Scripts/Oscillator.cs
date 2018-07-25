using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour {

	[SerializeField] Vector3 movingObstacle = new Vector3(10f, 10f, 10f);
	[SerializeField] float period = 2f;
	enum CycleRoutines { sideways, bothSides };
	[SerializeField] CycleRoutines currentCycleRoutine;

	[Range(-1, 1)][SerializeField] float movementFactor;

	Vector3 startingPos;

	// Use this for initialization
	void Start () {
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		float cycles = Time.time / period;

		const float tau = Mathf.PI * 2;
		float rawSinWave = Mathf.Sin(cycles * tau);

		if (currentCycleRoutine == CycleRoutines.bothSides)
		{
			movementFactor = rawSinWave; // / 2f + 0.5f;
		}
		else if (currentCycleRoutine == CycleRoutines.sideways)
		{
			movementFactor = rawSinWave / 2f + 0.5f; // / 2f + 0.5f;
		}

		Vector3 offset = movingObstacle * movementFactor;
		transform.position = offset + startingPos;
	}
}
