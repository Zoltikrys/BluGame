using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private SceneManager sceneManager;
    void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
    }

    public void Continue(){
        Debug.Log("Gameover Continue Presed");
        sceneManager.GameOverRespawn();
    }
    public void LoadGame(){
        Debug.Log("Gameover loadgame Presed");
        sceneManager.LoadGame();
    }
    public void Quit(){
        Debug.Log("Gameover Quit Presed");
        sceneManager.RequestLoadScene(LEVELS.MainMenu, 0, 0);
    }
}
