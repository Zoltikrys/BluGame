using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetAbility : MonoBehaviour
{
    public float pullForce = 10f;
    public float pushForce = 10f;
    public LayerMask MagneticSmall;  // For objects that the player pulls
    public LayerMask MagneticLarge;  // For large objects that pull the player
    public float interactionRadius = 20f;  // Range of magnetic ability
    public float largeMagnetThreshold = 5f;  // Distance at which large objects affect the player

    private bool isMagnetActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isMagnetActive = !isMagnetActive;
        }

        if (isMagnetActive)
        {
            HandleMagnetInteraction();
        }
    }

    void HandleMagnetInteraction()
    {
        AttractSmallObjects();  // Pull small objects towards the player
        PullPlayerToLargeObjects();  // Pull player towards large objects
    }

    // Pull small objects towards the player
    void AttractSmallObjects()
    {
        Collider[] smallObjects = Physics.OverlapSphere(transform.position, interactionRadius, MagneticSmall);

        foreach (Collider obj in smallObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 directionToPlayer = (transform.position - obj.transform.position).normalized;
                rb.AddForce(directionToPlayer * pullForce);
            }
        }
    }

    // Pull player towards large magnetic objects
    void PullPlayerToLargeObjects()
    {
        Collider[] largeObjects = Physics.OverlapSphere(transform.position, interactionRadius, MagneticLarge);

        foreach (Collider obj in largeObjects)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 directionToObject = (obj.transform.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, obj.transform.position);

                // Only apply force when within the threshold
                if (distance < largeMagnetThreshold)
                {
                    rb.AddForce(directionToObject * pushForce);
                }
            }
        }
    }
}


