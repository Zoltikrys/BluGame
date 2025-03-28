using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuFirst;

    public AudioMixer mainMixer;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public TMP_Dropdown qualityDropdown;

    private List<Resolution> resolutions = new List<Resolution>();

    private void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        // Load Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        Screen.fullScreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;

        // Load Resolution
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRateRatio.value == 60).ToList();
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            string resolutionOption = resolutions[i].width + "x" + resolutions[i].height;
            resolutionOptions.Add(resolutionOption);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        int savedResolutionIndex = PlayerPrefs.GetInt("Resolution", currentResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedResolutionIndex);

        // Load Quality
        int qualityIndex = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();

        // Load Audio Levels
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0);

        mainMixer.SetFloat("masterVolume", masterVolume);
        mainMixer.SetFloat("musicVolume", musicVolume);
        mainMixer.SetFloat("soundFXVolume", sfxVolume);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions.Count > 0)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("Resolution", resolutionIndex);
        }
    }

    public void SetMasterVolume(float masterVolume)
    {
        mainMixer.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        mainMixer.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float soundFXVolume)
    {
        mainMixer.SetFloat("soundFXVolume", soundFXVolume);
        PlayerPrefs.SetFloat("SFXVolume", soundFXVolume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void EnteringMenu()
    {
        EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
    }

    public void LeavingMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
