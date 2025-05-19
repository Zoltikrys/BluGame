using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneLock : MonoBehaviour
{
    [field: SerializeField] public PlayerController player;
    [field: SerializeField] public PlayableDirector director;

    void Awake()
    {
        director.played += DirectorStarted;
        director.stopped += DirectorEnded;
        director.Play();
    }

    private void DirectorEnded(PlayableDirector director)
    {
        if(player) player.UnlockMovement();
    }

    private void DirectorStarted(PlayableDirector director)
    {
        if(player) player.LockMovement();
    }

}
