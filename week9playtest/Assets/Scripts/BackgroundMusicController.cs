using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public static BackgroundMusicController Instance { get; private set; }
    private AudioSource audioSource;

    void Awake()
    {
        // Check if instance already exists and if it's not this instance, destroy it
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Don't destroy this object when loading a new scene

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on " + gameObject.name);
        }
    }

    void Start()
    {
        // Only play if the audio source is not already playing
        if (!audioSource.isPlaying)
        {
            // Start playing the music with a delay to synchronize the loop timing
            audioSource.PlayDelayed(149f);
        }
    }

    void Update()
    {
        // Check if 149 seconds have passed since the music started
        if (!audioSource.isPlaying)
        {
            // Restart the music exactly after 149 seconds
            audioSource.Play();
        }
    }

    public void ToggleMusic(bool isPlaying)
    {
        if (audioSource != null)
        {
            audioSource.enabled = isPlaying;
        }
    }
}
