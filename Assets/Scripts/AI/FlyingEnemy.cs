using UnityEngine;

public class FlyingEnemy : Enemy
{
    public float forwardBoost = 3f;
    public float timeToTarget = 5;
    private float timeRemaining = 5;
    [field: SerializeField] public Color TargettingColour;
    [field: SerializeField] public Color PatrollingColour;
    [field: SerializeField] public Color AttackingColour;
    [field: SerializeField] public GameObject spotlight;
    [field: SerializeField] public Animator anim;

    protected override void Start()
    {
        base.Start();
        CurrentState = NpcState.Patrolling;
        FOV = GetComponent<FieldOfView>();
        anim.Play("AngryFlying");
    }

    protected override void Update(){
        base.Update();
        playerSeen = FOV.canSeePlayer;
    }

    protected override void PatrollingState()
    {
        //anim.Play("NormalFlying");
        spotlight.GetComponent<Light>().color = PatrollingColour;
        base.PatrollingState();
    }

    protected override void SearchingState()
    {
        base.SearchingState();
    }
    protected override void TargetingState()
    {
        spotlight.GetComponent<Light>().color = TargettingColour;

        playerSeen = false; // Reset playerSeen
        hasHit = false; // Reset hasHit

        // Face player
        transform.LookAt(target);
        anim.Play("Spotted");


        if (timeRemaining > 0)
        {


            timeRemaining -= Time.deltaTime;
        }
        else
        {
            playerPos = target.position;
            CurrentState = NpcState.Attack;
        }
    }

    protected override void AttackState()
    {
        spotlight.GetComponent<Light>().color = AttackingColour;
        //anim.Play("AngryFlying");
        // Reset timeRemaining
        timeRemaining = timeToTarget;

        // Move towards player
        Vector3 targetPos = playerPos + (transform.forward * forwardBoost);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * speed);

        // If the enemy reaches the target, switch to searching
        if (transform.localPosition == targetPos)
        {
            CurrentState = NpcState.Patrolling;
            //currentState = State.Searching; implement for vertical slice - stays stationary but rotates to look around for player
        }
    }

    protected override void DeadState()
    {
        anim.SetBool("Dead", true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit BLU");
            hasHit = true;
            HealthManager healthMan = collision.gameObject.GetComponent<HealthManager>(); // damage player
            healthMan.Damage();
            GetComponent<HealthManager>().Damage();
            if(KnockbackEffect != null){
                KnockbackEffect.ApplyKnockback(collision.gameObject.GetComponent<CharacterController>(), transform.forward);
                KnockbackEffect.ApplyKnockback(GetComponent<Rigidbody>(), -transform.forward);
            }
        }
        if (collision.gameObject.GetComponent<powerSource>())
        {
            Debug.Log("Hit Power Source");
            hasHit = true;
            powerSource powerSource = collision.gameObject.GetComponent<powerSource>();
            powerSource.TakeDamage();
            GetComponent<HealthManager>().Damage();
            
        }
        else
        {
            Debug.Log("Gonna explode now");
            //Destroy(this.gameObject); // deletes self
        }

        if (GetComponent<HealthManager>().b_Health <= 0) {
            CurrentState = NpcState.Dead;
        }

        // Handle state transitions based on collisions
        else if (hasHit)
        {
            CurrentState = NpcState.Targeting;
        }
        else
        {
            CurrentState = NpcState.Patrolling;
            //currentState = State.Searching; implement for vertical slice - stays stationary but rotates to look around for player
        }
    }
}


