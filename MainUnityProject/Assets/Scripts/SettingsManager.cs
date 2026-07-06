using System;
using UnityEngine;
using UnityEngine.Events;

public class SettingsManager
{
    [HideInInspector] public static float CameraSensX, CameraSensY, MusicVolume, SoundVolume;

    public static UnityEvent SettingsChangedEvent;

    void Start()
    {
        
    }
    
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
