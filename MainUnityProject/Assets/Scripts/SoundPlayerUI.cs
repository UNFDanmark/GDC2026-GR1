using UnityEngine;

public class SoundPlayerUI: MonoBehaviour
{
    public AudioSource audioSourceEnter, audioSourceExit;
    void Start()
    {
        SettingsManager.SettingsChangedEvent.AddListener(ReloadSettings);
        ReloadSettings();
    }

    void OnDisable()
    {
        SettingsManager.SettingsChangedEvent.RemoveListener(ReloadSettings);
    }

    void ReloadSettings()
    {
        audioSourceEnter.volume = SettingsManager.SoundVolume;
        audioSourceExit.volume = SettingsManager.SoundVolume;
    }

    void OnPointerEnter()
    {
        audioSourceEnter.Play();
    }

    void OnPointerExit()
    {
        audioSourceExit.Play();
    }
}