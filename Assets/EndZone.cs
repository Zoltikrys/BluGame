using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndZone : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject mainMenu;

    // Update is called once per frame
    public void OnTriggerEnter()
    {
        Time.timeScale = 0.0f;
        endScreen.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainMenu);
    }

    void MainMenu()
    {
        Debug.Log("try if you must, but this hasn't been programmed yet.");
    }
}
