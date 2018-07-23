using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidbodyRocket;
	AudioSource rocketThrustSound;
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float rcsRotation = 100f;
	enum States { Alive, Dying, Transcending };
	States state = States.Alive;
	int currentScene = 0;
	[SerializeField] float sceneLoadDelay = 1f;

	// Use this for initialization
	void Start () {
		rigidbodyRocket = GetComponent<Rigidbody>();
		rocketThrustSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (state != States.Dying)
		{
			ProcessInput();
		}
		else
		{
			rocketThrustSound.Pause();
		}
	}

	private void ProcessInput()
	{
		Thrust();
		Rotate();
	}

	void OnCollisionEnter(Collision collision)
	{
		if (state != States.Alive) { return; }

		switch (collision.gameObject.tag)
		{
			case "Friendly":
				print("Touched launch pad");
				break;
			case "Goal":
				state = States.Transcending;
				Invoke("LoadNextScene", sceneLoadDelay);
				break;
			default:
				state = States.Dying;
				Invoke("LoadFirstScene", sceneLoadDelay);
				break;
		}
	}

	private void LoadNextScene()
	{
		int sceneCount = SceneManager.sceneCountInBuildSettings - 1;
		print("Loads new Scene... " + sceneCount);
		if (currentScene <= sceneCount)
		{
			currentScene = currentScene + 1;
		}
		SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
	}

	private void LoadFirstScene()
	{
		print("Hit obstacle");
		currentScene = 0;
		SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
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
