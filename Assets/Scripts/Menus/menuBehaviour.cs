using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class menuBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuFirst;

    public LEVELS scenetoload;
    public SceneManager sceneManager;
    public GameObject fadeScreen;
    public bool fadeOut = false;

    public void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
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

    public void QuitGame()
    {
        Debug.Log("You have tried to QUIT the game :(");
        Application.Quit();
    }

    public void SetSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }


    public void LeavingMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
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
