using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private bool canExplode = false;

    public float detonatePoint;
    public float detonateTimer = 0f;
    [SerializeField] Collider explosionArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            detonateTimer += Time.deltaTime;
            if (detonateTimer >= detonatePoint)
            {
                canExplode = true;
            }

        }

    }

    public void OnTriggerStay(Collider other)
    {
        if (canExplode)
        {
            //Play animation
            if (other.CompareTag("Player"))
            {
                //damage player
            }




            //Delete self
        }
    }


}
