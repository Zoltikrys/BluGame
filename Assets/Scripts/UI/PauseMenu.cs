using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;
    public static bool settingsMenuActive = false;

    public GameObject PauseMenuUI;

    [SerializeField] private GameObject pauseMenuFirst;

    private void Start()
    {
        PauseMenuUI.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) && settingsMenuActive == false)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

            //gameIsPaused = true;
        }

    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);

    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //0 is the SceneManager, may need to change if that changes idk
    }

    public void ActivateSettings()
    {
        settingsMenuActive = true;
    }

    public void DeactivateSettings()
    {
        settingsMenuActive = false;
    }

    public void ReturnToMenu()
    {
        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); //Won't work outside of a build
    }
}
