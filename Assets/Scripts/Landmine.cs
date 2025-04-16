using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private bool canExplode = false;
    [SerializeField] private bool hasExploded = false;

    [SerializeField] private KnockbackEffect knockback;

    public float detonateTime = 4f;
    [SerializeField] public float detonateTimer = 0f;

    [SerializeField] private float explosionAnimDone = 5f; //don't know how long this will be yet
    [SerializeField] private float explosionDeltaTime = 0f;

    [SerializeField] Collider explosionArea;

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            //Play vfx, probably just a light or something
            detonateTimer += Time.deltaTime;
            if (detonateTimer >= detonateTime)
            {
                canExplode = true;
            }
        }


        if (canExplode)
        {
            //Play animation
            //play explosion vfx
            //hasExploded = true;
            explosionDeltaTime += Time.deltaTime;

            /*
            if (explosionDeltaTime >= explosionAnimDone)
            {
                hasExploded = true;
                //delete self
            }
            */

        }
    }

        public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (canExplode)
        {
            if (!hasExploded) //So damange only happens once
            {
                if (other.CompareTag("Player"))
                {
                    Debug.Log("Player in explosion radius");
                    //damage player

                    //knockback
                    knockback.ApplyRadialKnockback(other.gameObject.GetComponent<CharacterController>(), transform.position);
                    hasExploded = true; //remove later
                }
            }

        }
    }
}
