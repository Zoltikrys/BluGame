using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;
    public float damage = 10f;
    public float lifeTime = 5000f;
    public KnockbackEffect knockback;
    [SerializeField] private Rigidbody bulletRB;

    void Start()
    {
        Destroy(gameObject, lifeTime); //Destroy after lifeTime seconds
        bulletRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //Assumes player is tagged as "Player"
        {
            Debug.Log("Hit BLU");
            //Code is breaking here because of the sound issue -- fix is just to add a SoundFXManager into the scene and give that a SoundFXObject reference
            HealthManager healthMan = other.gameObject.GetComponent<HealthManager>(); // damage player
            healthMan.Damage();
            knockback.ApplyKnockback(other.gameObject.GetComponent<CharacterController>(), bulletRB.velocity);
        }




        Debug.Log("Collided with " + other.gameObject.name); //for debugging
        if (other.CompareTag("Projectile"))
        {
            return; //ignores collision with projectiles
        }
        Destroy(gameObject); //Destroy on collision
    }
}
