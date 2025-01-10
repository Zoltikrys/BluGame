using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //audio mixers for sound
    /*public AudioMixer masterMixer;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;*/

    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();


        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionOption = resolutions[i].width + "x" + resolutions[i].height;
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
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float MasterVolume)
    {
        throw new NotImplementedException();
    }

    public void SetMusicVolume(float MusicVolume)
    {
        throw new NotImplementedException();
    }

    public void SFXVolume(float SFXVolume)
    {
        throw new NotImplementedException();
    }

    //I'm doing sound later cause I really don't wanna do it

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    }

    public void SetFullscreen(bool isFullscreen)
    {
        Debug.Log("Toggle fullscreen");
        Screen.fullScreen = isFullscreen; //won't work outside of a build
    }
}
