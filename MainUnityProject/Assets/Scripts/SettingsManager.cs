using System;
using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    [HideInInspector] public static SettingsManager Instance;
    [HideInInspector] public float cameraSensX, cameraSensY, musicVolume, soundVolume;

    public UnityEvent settingsChangedEvent;
    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        PermaLoad();
        settingsChangedEvent.AddListener(SaveSettings);
        DontDestroyOnLoad(gameObject);
    }

    void OnDisable()
    {
        if (Instance != this) return;
        settingsChangedEvent.RemoveAllListeners();
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("soundVolume", soundVolume);
        PlayerPrefs.SetFloat("cameraSensX", cameraSensX);
        PlayerPrefs.SetFloat("cameraSensY", cameraSensY);
    }

    void PermaLoad()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        soundVolume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        cameraSensX = PlayerPrefs.GetFloat("cameraSensX", 0.25f);
        cameraSensY = PlayerPrefs.GetFloat("cameraSensY", 0.15f);
    }
}
