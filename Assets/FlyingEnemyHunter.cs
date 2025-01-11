using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlyingEnemyHunter : MonoBehaviour
{
    [field: SerializeField] public Color AttackingColour;
    [field: SerializeField] public GameObject spotlight;
    [field: SerializeField] public Animator anim;

    [SerializeField] private Vector3 targetPos;

    public bool playerSeen; // public for testing purposes, change to private when implementation finished
    protected bool hasHit = false;
    public float speed = 10f;
    public float attackCooldown = 3f;
    protected Vector3 playerPos;
    [SerializeField] protected Transform target;

    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private ParticleSystem destroyParticle;

    public KnockbackEffect KnockbackEffect;


    public void Start()
    {
        TryGetComponent<KnockbackEffect>(out KnockbackEffect);
        spotlight.GetComponent<Light>().color = AttackingColour;
        target = GameObject.Find("Player").transform;
        anim.Play("NormalFlying");
    }

    public void Update()
    {
        playerPos = target.position;
        targetPos = playerPos;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * speed);
        transform.LookAt(targetPos);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player") {
            Debug.Log("Hit BLU");
            hasHit = true;
            HealthManager healthMan = collision.gameObject.GetComponent<HealthManager>(); // damage player
            healthMan.Damage();
            if (KnockbackEffect != null) {
                KnockbackEffect.ApplyKnockback(collision.gameObject.GetComponent<CharacterController>(), transform.forward);
                KnockbackEffect.ApplyKnockback(GetComponent<Rigidbody>(), -transform.forward);
            }
        }
        if (collision.gameObject.GetComponent<powerSource>()) {
            Debug.Log("Hit Power Source");
            hasHit = true;
            powerSource powerSource = collision.gameObject.GetComponent<powerSource>();
            powerSource.TakeDamage();
            if (KnockbackEffect != null) {
                KnockbackEffect.ApplyKnockback(GetComponent<Rigidbody>(), -transform.forward);
            }
        }
        else {
            //Destroy(this.gameObject); // deletes self
        }

        SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 1f);
        Instantiate(destroyParticle, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
