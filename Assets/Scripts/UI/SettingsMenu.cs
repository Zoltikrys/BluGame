using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
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
}
