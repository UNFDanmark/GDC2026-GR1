using UnityEngine;
using UnityEngine.Events;

public static class SettingsManager
{
    [HideInInspector] public static float CameraSensX = 0.25f, CameraSensY = 0.15f, MusicVolume = 0.5f, SoundVolume = 0.5f;

    public static UnityEvent SettingsChangedEvent = new UnityEvent();
    
    public static void SaveSettings()
    {
        PlayerPrefs.SetFloat("musicVolume", MusicVolume);
        PlayerPrefs.SetFloat("soundVolume", SoundVolume);
        PlayerPrefs.SetFloat("cameraSensX", CameraSensX);
        PlayerPrefs.SetFloat("cameraSensY", CameraSensY);
    }

    public static void PermaLoad()
    {
        MusicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        SoundVolume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        CameraSensX = PlayerPrefs.GetFloat("cameraSensX", 0.25f);
        CameraSensY = PlayerPrefs.GetFloat("cameraSensY", 0.15f);
    }
}
