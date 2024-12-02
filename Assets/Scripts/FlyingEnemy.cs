using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyingEnemy : MonoBehaviour
{
    public State currentState = State.Idle;

    private bool isTargeting;
    private bool isIdle;
    private bool isSearching;
    private bool isAttacking;
    private bool hasHit = false;

    [SerializeField] private Transform target;

    public bool playerSeen; //public for testing purposes, change to private when implementation finished

    public float forwardBoost = 3f;
    public float speed = 10f;
    public float timeToTarget = 5;
    private float timeRemaining = 5;

    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    async void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                //Debug.Log("Idle state...");
                //create a vision cone to spot player later
                //also make enemy patrol ig? ask logan on specifics later
                if (playerSeen)
                {
                    currentState = State.Targeting;
                }
                break;
            case State.Targeting:
                //Debug.Log("Targeting!");
                //reset playerSeen
                playerSeen = false;
                //face player
                transform.LookAt(target);
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    //Debug.Log(timeRemaining);
                }
                else
                {
                    //take player's current pos + forward
                    playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                    //switch state to attacking
                    currentState = State.Attack;
                }

                break;
            case State.Attack:
                //Debug.Log("Attacking!!!");

                //reset timeRemaining
                timeRemaining = timeToTarget;

                //move towards player pos + forward
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, (playerPos + (transform.forward * forwardBoost)), Time.deltaTime * speed);

                //await OnCollisionEnter(Collision collision);

                /*if (hasHit) //////////// this is doing my head in need to sort it
                {
                    currentState = State.Targeting;
                }
                else
                {
                    currentState = State.Searching;
                }*/
                    break;
            case State.Searching:
                Debug.Log("Searching for BLU");
                //Rotate and move around area


                //if BLU spotted: switch state to targeting
                if (playerSeen)
                {
                    currentState = State.Targeting; 
                }
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //print(collision.gameObject.name + " is colliding");

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Player")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Hit BLU");
            hasHit = true;
            HealthManager healthMan = collision.gameObject.GetComponent<HealthManager>();
            healthMan.DamagePlayer();
        }
        else
        {
            //has hit something else
            //explode animation ig?
            Debug.Log("Gonna explode now");
            Destroy(this.gameObject); //deletes self
        }
    }
}

[System.Serializable]
public enum State { Idle, Targeting, Attack, Searching }
