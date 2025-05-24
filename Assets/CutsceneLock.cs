using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneLock : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayableDirector director;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("CutsceneLock: PlayerController is not assigned.");
            return;
        }

        if (director == null)
        {
            Debug.LogError("CutsceneLock: PlayableDirector is not assigned.");
            return;
        }

        director.played += OnCutsceneStarted;
        director.stopped += OnCutsceneEnded;

        // Wait one frame to ensure everything is initialized
        StartCoroutine(InitializeCutscene());
    }

    private IEnumerator InitializeCutscene()
    {
        yield return null;

        if (director.state == PlayState.Playing)
        {
            // Director is already playing
            OnCutsceneStarted(director);
        }
        else
        {
            // Start the cutscene
            director.Play();
        }
    }

    private void OnCutsceneStarted(PlayableDirector d)
    {
        Debug.Log("Cutscene started.");
        player.LockMovement();
    }

    private void OnCutsceneEnded(PlayableDirector d)
    {
        Debug.Log("Cutscene ended.");
        player.UnlockMovement();
    }
}
