using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndZone : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private SceneManager sceneManager;

    private void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<CharacterController>() == null) return;
        Time.timeScale = 0.0f;
        endScreen.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainMenu);
    }

    public void MainMenu()
    {
        Debug.Log("try if you must, but this hasn't been programmed yet.");
        Application.Quit();
    }
}
