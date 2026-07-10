using UnityEngine;
using UnityEngine.Events;

public class SoundPlayer: MonoBehaviour
{
    AudioSource audioSource;
    public static UnityEvent stopMusic;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SettingsManager.SettingsChangedEvent.AddListener(ReloadSettings);
        stopMusic = new UnityEvent();
        stopMusic.AddListener(StopMusic);
        ReloadSettings();
    }

    void OnDisable()
    {
        SettingsManager.SettingsChangedEvent.RemoveListener(ReloadSettings);
        stopMusic.RemoveListener(StopMusic);
    }

    void ReloadSettings()
    {
        audioSource.volume = SettingsManager.MusicVolume;
    }

    void StopMusic()
    {
        audioSource.Stop();
    }
}