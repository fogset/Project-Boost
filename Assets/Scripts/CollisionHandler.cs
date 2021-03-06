using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    float levelLoadDelay = 2f;

    [SerializeField]
    AudioClip Crash;

    [SerializeField]
    AudioClip Success;

    AudioSource audioSource;

    [SerializeField]
    ParticleSystem successParticles;

    [SerializeField]
    ParticleSystem crashParticles;

    bool isTransitioning = false;

    bool collisionDisable = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        successParticles.Play();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; //toggle collision
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning == true || collisionDisable == true)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("You picked up fuel");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        crashParticles.Play();
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot (Crash);

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void StartSuccessSequence()
    {
        successParticles.Play();
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot (Success);

        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene (currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene (nextSceneIndex);
    }
}
