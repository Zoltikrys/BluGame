using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuFirst;



    //audio mixers for sound
    public AudioMixer mainMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;

    List<Resolution> resolutions = new List<Resolution>();
    Resolution fallbackResolution;

    //resolution
    private void Start()
    {
        fallbackResolution.width = 1920;
        fallbackResolution.height = 1080;

        //resolutions = Screen.resolutions.ToList();
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRateRatio.value == 60).ToList();

        resolutionDropdown.ClearOptions(); //clears options

        List<string> resolutionOptions = new List<string>();


        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Count; i++) //populating list with available options dependant on monitor
        {
            string resolutionOption = resolutions[i].width + "x" + resolutions[i].height; //.ToString();
            resolutionOptions.Add(resolutionOption);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i; //gets current resolution
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;//automatically sets current resolution
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex)
    {
        if(resolutions.Count == 0){
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
        else{
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }

    //volume
    public void SetMasterVolume(float masterVolume)
    {
        mainMixer.SetFloat("masterVolume", masterVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        mainMixer.SetFloat("musicVolume", musicVolume);
    }

    public void SFXVolume(float soundFXVolume)
    {
        mainMixer.SetFloat("soundFXVolume", soundFXVolume);
    }

    //graphics quality
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    }

    //fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        Debug.Log("Toggle fullscreen");
        Screen.fullScreen = isFullscreen; //won't work outside of a build
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
