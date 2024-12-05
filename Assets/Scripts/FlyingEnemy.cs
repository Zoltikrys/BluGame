using System.Collections;
using UnityEngine;

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
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if (playerSeen)
                {
                    currentState = State.Targeting;
                }
                break;

            case State.Targeting:
                playerSeen = false;  // reset playerSeen
                transform.LookAt(target);
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                timeRemaining = timeToTarget;  // reset timeRemaining
                // Move towards player position + forward
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, (playerPos + (transform.forward * forwardBoost)), Time.deltaTime * speed);

                // Start coroutine for handling collision logic
                StartCoroutine(HandleAttack());

                break;

            case State.Searching:
                Debug.Log("Searching for BLU");
                if (playerSeen)
                {
                    currentState = State.Targeting;
                }
                break;
        }
    }

    // Coroutine for handling attack logic
    private IEnumerator HandleAttack()
    {
        yield return new WaitForSeconds(1f);  // Delay before checking for collision

        if (hasHit) // Collision already happened
        {
            currentState = State.Targeting;
        }
        else
        {
            currentState = State.Searching;
        }
    }

    // Unity collision detection
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit BLU");
            hasHit = true;
            HealthManager healthMan = collision.gameObject.GetComponent<HealthManager>();
            healthMan.DamagePlayer();
        }
        else
        {
            Debug.Log("Gonna explode now");
            Destroy(this.gameObject); // Destroys the enemy
        }
    }
}

[System.Serializable]
public enum State { Idle, Targeting, Attack, Searching }
