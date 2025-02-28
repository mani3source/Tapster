using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public AudioSource[] sfxAudioSources;  // Array of AudioSource components for SFX

    void Start()
    {
        LoadSettings();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void AdjustVolume()
    {
        // Loop through all AudioSources and set their volume
        foreach (var audioSource in sfxAudioSources)
        {
            audioSource.volume = volumeSlider.value;
        }
        
        // Save the volume setting
        PlayerPrefs.SetFloat("SFXVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();  // Reset all saved data
        PlayerPrefs.Save();
        Debug.Log("Game Progress Reset!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);  // Restart the scene
    }

    void LoadSettings()
    {
        // Load saved volume if it exists
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("SFXVolume");
            volumeSlider.value = savedVolume;
            AdjustVolume();  // Apply the saved volume to all audio sources
        }
    }
}
