using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;
    public float damage = 10f;
    public float lifeTime = 5000f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy after lifeTime seconds
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assumes player is tagged as "Player"
        {
            Debug.Log("Hit BLU");
            //Code is breaking here because of the sound issue
            HealthManager healthMan = other.gameObject.GetComponent<HealthManager>(); // damage player
            healthMan.Damage();
        }

        Debug.Log("Collided with " + other.gameObject.name); //for debugging

        Destroy(gameObject); // Destroy on collision
    }
}
