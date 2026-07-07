using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsApplier : MonoBehaviour
{
    public InputActionReference actionPause;
    public Canvas pauseMenuCanvas;
    public Slider CameraSensXSlider, CameraSensYSlider, MusicVolumeSlider, SoundVolumeSlider;

    static bool paused;
    public static bool canPause = true;
    
    void Awake()
    {
        SettingsManager.PermaLoad();
        paused = false;
        canPause = true;
    }

    void Start()
    {
        CameraSensXSlider.normalizedValue = SettingsManager.CameraSensX;
        CameraSensYSlider.normalizedValue = SettingsManager.CameraSensY;
        MusicVolumeSlider.normalizedValue = SettingsManager.MusicVolume;
        SoundVolumeSlider.normalizedValue = SettingsManager.SoundVolume;
        CameraSensXSlider.onValueChanged.AddListener(delegate { ApplySettings();});
        CameraSensYSlider.onValueChanged.AddListener(delegate { ApplySettings();});
        MusicVolumeSlider.onValueChanged.AddListener(delegate { ApplySettings();});
        SoundVolumeSlider.onValueChanged.AddListener(delegate { ApplySettings();});
        actionPause.action.Enable();
        actionPause.action.started += PauseHandler;
    }

    void OnDisable()
    {
        CameraSensXSlider.onValueChanged.RemoveListener(delegate { ApplySettings();});
        CameraSensYSlider.onValueChanged.RemoveListener(delegate { ApplySettings();});
        MusicVolumeSlider.onValueChanged.RemoveListener(delegate { ApplySettings();});
        SoundVolumeSlider.onValueChanged.RemoveListener(delegate { ApplySettings();});
        actionPause.action.Disable();
        actionPause.action.started -= PauseHandler;
    }

    void ApplySettings()
    {
        SettingsManager.CameraSensX = CameraSensXSlider.normalizedValue;
        SettingsManager.CameraSensY = CameraSensYSlider.normalizedValue;
        SettingsManager.MusicVolume = MusicVolumeSlider.normalizedValue;
        SettingsManager.SoundVolume = SoundVolumeSlider.normalizedValue;
        SettingsManager.SettingsChangedEvent.Invoke();
        SettingsManager.SaveSettings();
    }

    void PauseHandler(InputAction.CallbackContext context)
    {
        PauseToggle();
    }
    
    void PauseToggle()
    {
        if (!canPause) return;
        if (paused)
        {
            pauseMenuCanvas.enabled = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            paused = false;
        }
        else
        {
            pauseMenuCanvas.enabled = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
        }
    }
}
