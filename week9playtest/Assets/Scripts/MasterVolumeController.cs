using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeController : MonoBehaviour
{
    public Slider volumeSlider; // Assign in the inspector
    public Toggle backgroundMusicToggle; // Assign in the inspector
    public Toggle battleFxToggle; // Assign in the inspector

    private AudioSource[] audioSources;

    void Start()
    {
        // Find all audio sources in the scene
        audioSources = FindObjectsOfType<AudioSource>();

        // Set the slider's value to the current volume
        if (audioSources.Length > 0)
        {
            volumeSlider.value = audioSources[0].volume;
        }

        // Add listeners for the slider and toggles
        volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        backgroundMusicToggle.onValueChanged.AddListener(delegate { OnBackgroundMusicToggleChanged(); });
        battleFxToggle.onValueChanged.AddListener(delegate { OnBattleFxToggleChanged(); });

        // Initialize toggle states
        backgroundMusicToggle.isOn = true;
        battleFxToggle.isOn = true;
    }

    void OnSliderValueChanged()
    {
        // Update the volume of all audio sources
        foreach (AudioSource source in audioSources)
        {
            source.volume = volumeSlider.value;
        }
    }

    void OnBackgroundMusicToggleChanged()
    {
        if (BackgroundMusicController.Instance != null)
        {
            BackgroundMusicController.Instance.ToggleMusic(backgroundMusicToggle.isOn);
        }
    }

    void OnBattleFxToggleChanged()
    {
        unit[] units = FindObjectsOfType<unit>();
        foreach (var unit in units)
        {
            if (unit.battleFX != null)
            {
                unit.battleFX.enabled = battleFxToggle.isOn;
            }
        }
    }
}
