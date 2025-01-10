using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuBehaviour : MonoBehaviour
{
    public LEVELS scenetoload;
    public SceneManager sceneManager;
    public GameObject fadeScreen;
    public bool fadeOut = false;

    //Stuff for settings menu
    //audio mixers for sound
    public AudioMixer mainMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        //Resolution settings
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

    public void LoadLevel()
    {
        fadeScreen.SetActive(true);
        StartCoroutine(Fade(fadeScreen.GetComponent<Image>(), 1f));
        if (fadeOut == true) {
            sceneManager.RequestLoadScene(scenetoload, 0, 0, CAMERA_TRANSITION_TYPE.FADE_TO_BLACK, CAMERA_TRANSITION_TYPE.FADE_TO_BLACK);

        }

        StartCoroutine(LoadSceneAfterFade());
    }

    //Settings menu
    //resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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











    public void QuitGame()
    {
        Debug.Log("You have tried to QUIT the game :(");
        Application.Quit();
    }

    IEnumerator Fade(Image img, float targetAlpha)
    {
        while (img.color.a != targetAlpha) {
            var newAlpha = Mathf.MoveTowards(img.color.a, targetAlpha, 0.7f * Time.deltaTime);
            img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);
            yield return null;
        }
    }

    IEnumerator LoadSceneAfterFade()
    {
        yield return Fade(fadeScreen.GetComponent<Image>(), 1f);
        sceneManager.RequestLoadScene(scenetoload, 0, 0, CAMERA_TRANSITION_TYPE.FADE_TO_BLACK, CAMERA_TRANSITION_TYPE.FADE_TO_BLACK);
    }

}
