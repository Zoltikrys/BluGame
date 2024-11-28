using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public State currentState = State.Idle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Debug.Log("Idle state...");
                break;
            case State.Attack:
                Debug.Log("Attacking!");
                break;
        }
    }
}

[System.Serializable]
public enum State { Idle, Attack }
