using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuBehaviour : MonoBehaviour
{
    public SceneAsset scenetoload;
    public SceneManager sceneManager;

    public void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }

    public void LoadLevel()
    {
        sceneManager.RequestLoadScene(scenetoload, 0, 0);
    }
}
