using System;
using UnityEngine;

public class SoundPlayer: MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SettingsManager.SettingsChangedEvent.AddListener(ReloadSettings);
        ReloadSettings();
    }

    void OnDisable()
    {
        SettingsManager.SettingsChangedEvent.RemoveListener(ReloadSettings);
    }

    void ReloadSettings()
    {
        audioSource.volume = SettingsManager.MusicVolume;
    }
}