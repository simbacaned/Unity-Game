using UnityEngine.UI;
using UnityEngine;


public class MusicVolume : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider volumeSlider;

    private const string VOLUME_KEY = "volume";

    private void Start()
    {
        // Load the saved volume value from PlayerPrefs
        if (PlayerPrefs.HasKey(VOLUME_KEY))
        {
            float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY);
            audioSource.volume = savedVolume;
            volumeSlider.value = savedVolume;
            Debug.Log(savedVolume);
        }
    }

    public void SetVolume(float volume)
    {
        // Set the volume of the audio source to the value of the slider
        audioSource.volume = volume;
        // Save the volume value to PlayerPrefs
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
    }
}

