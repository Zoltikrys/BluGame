using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class bossShield : MonoBehaviour
{
    [SerializeField]
    public int powerSourceFlags = 0;
    [SerializeField]
    public int powerSourceTarget;
    [SerializeField]
    private Image endScreen;
    [SerializeField]
    private GameObject mainMenu;

    public void OpenSequence()
    {
        Debug.Log("omg its openinggg");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            DeathSequence();
        }
    }

    public void DeathSequence()
    {
        Time.timeScale = 0.0f;
        endScreen.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainMenu);
    }

    public void MainMenu()
    {

    }
}
