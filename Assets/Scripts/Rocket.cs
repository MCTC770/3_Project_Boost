using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidbodyRocket;
	AudioSource rocketAudio;
	int sceneCount;
	int currentScene;
	bool debugCollision = false;

	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float rcsRotation = 100f;
	[SerializeField] float sceneLoadDelay = 1f;
	[SerializeField] float environmentAudioVolume = 0.15f;

	[SerializeField] AudioClip thrustingSound;
	[SerializeField] AudioClip deathSound;
	[SerializeField] AudioClip winSound;

	[SerializeField] AudioSource environmentAudio;

	[SerializeField] ParticleSystem thrustingParticles;
	[SerializeField] ParticleSystem deathParticles;
	[SerializeField] ParticleSystem winParticles;

	enum States { Alive, Dying, Transcending };
	States state = States.Alive;

	// Use this for initialization
	void Start ()
	{
		rigidbodyRocket = GetComponent<Rigidbody>();
		rocketAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (state != States.Dying)
		{
			ProcessInput();
		}
	}

	private void ProcessInput()
	{
		RespondToThrustInput();
		RespondToRotateInput();
		if (Debug.isDebugBuild)
		{
			RespondToCollisionDebugToggle();
			RespondToLevelSkip();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (state != States.Alive)
		{
			return;
		}

		switch (collision.gameObject.tag)
		{
			case "Friendly":
				print("Touched launch pad");
				break;
			case "Goal":
				StartSuccessSequence();
				break;
			default:
				if (debugCollision == false)
				{
					StartDeathSequence();
				}
				break;
		}
	}

	private void StartSuccessSequence()
	{
		state = States.Transcending;
		environmentAudio.volume = environmentAudioVolume;
		environmentAudio.PlayOneShot(winSound);
		winParticles.Play();
		Invoke("LoadNextScene", sceneLoadDelay);
	}

	private void StartDeathSequence()
	{
		state = States.Dying;
		environmentAudio.volume = environmentAudioVolume;
		environmentAudio.PlayOneShot(deathSound);
		deathParticles.Play();
		thrustingParticles.Stop();
		Invoke("LoadFirstScene", sceneLoadDelay);
	}

	private void LoadNextScene()
	{
		sceneCount = SceneManager.sceneCountInBuildSettings - 1;
		currentScene = SceneManager.GetActiveScene().buildIndex + 1;

		if (currentScene > sceneCount)
		{
			currentScene = 0;
		}

		print("Max scene index: " + sceneCount);
		print("Current scene: " + currentScene);
		SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
	}

	private void LoadFirstScene()
	{
		print("Hit obstacle");
		currentScene = 0;
		SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
	}

	private void RespondToThrustInput()
	{
		float thrustSpeed = rcsThrust * Time.deltaTime;

		if (Input.GetKey(KeyCode.Space))
		{
			ApplyThrust(thrustSpeed);
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			rocketAudio.Stop();
			thrustingParticles.Stop();
		}
	}

	private void ApplyThrust(float thrustSpeed)
	{
		rigidbodyRocket.AddRelativeForce(Vector3.up * thrustSpeed); // * Time.deltaTime);
		if (!rocketAudio.isPlaying)
		{
			rocketAudio.PlayOneShot(thrustingSound);
		}
		thrustingParticles.Play();
	}

	private void RespondToRotateInput()
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

	private void RespondToCollisionDebugToggle()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (debugCollision == true)
			{
				debugCollision = false;
			}
			else if (debugCollision == false)
			{
				debugCollision = true;
			}

			print("Debug collision: " + debugCollision);
		}
	}

	private void RespondToLevelSkip()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			LoadNextScene();
		}
	}
}
