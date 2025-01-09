using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StationaryAI : MonoBehaviour
{
    protected FieldOfView FOV;

    public CurrentState currentState;
    public bool playerSeen; // public for testing purposes, change to private when implementation finished
    public float rotateSpeed = 0.1f;
    [SerializeField] protected Transform target;

    //for shooting
    public GameObject projectilePrefab;
    public Transform firePoint; // Point from where the projectile will be shot

    public float shootInterval = 2f;

    private float shootTimer;



    // Start is called before the first frame update
    void Start()
    {
        currentState = CurrentState.Idle;
        FOV = GetComponent<FieldOfView>();
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerSeen = FOV.canSeePlayer;

        

        switch (currentState)
        {
            case CurrentState.Idle:
                IdleState();
                break;
            case CurrentState.Targeting:
                TargetingState();
                break;
            case CurrentState.Attack:
                AttackState();
                break;
            }
        }

    private void IdleState()
    {
        Debug.Log("Idling...");
        transform.Rotate(Vector3.up, 1*rotateSpeed);
        if (playerSeen)
        {
            currentState = CurrentState.Targeting;
        }
    }


    private void TargetingState()
    {
        Debug.Log("Targeting...");

        Quaternion OriginalRot = transform.rotation;
        transform.LookAt(new Vector3(target.position.x,
                                     transform.position.y,
                                     target.transform.position.z));
        Quaternion NewRot = transform.rotation;
        transform.rotation = OriginalRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, 5f * Time.deltaTime);

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Attack();
            shootTimer = 0;
        }


        if (!playerSeen && shootTimer == 0)
        {
            Debug.Log("Returning to idle state");
            currentState = CurrentState.Idle;
        }
        //wait for x amount of time idk

    }

    private void AttackState()
    {
        Debug.Log("Attacking...");
        //attack code
        //shoot projectile at player ig
        //losing my mIIIIIIND

    }

    private void Attack()
    {
        Vector3 direction = (target.position - firePoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectile.GetComponent<Projectile>().speed;
        }
    }

}


[System.Serializable]
public enum CurrentState { Idle, Targeting, Attack }