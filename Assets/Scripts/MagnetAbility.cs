using UnityEngine;

public class MagnetAbility : MonoBehaviour
{
    [Header("Small Magnet Settings")]
    public string smallMagnetTag = "SmallMagnet"; // Tag for small magnetic objects
    public float smallMagnetPullSpeed = 10f;     // Speed for pulling small magnets
    public float smallMagnetStopDistance = 1.5f; // Distance in front of the player

    [Header("Big Magnet Settings")]
    public string bigMagnetTag = "BigMagnet";    // Tag for big magnetic objects
    public float bigMagnetPullSpeed = 5f;       // Speed to pull player toward big magnets
    public float bigMagnetRange = 20f;          // Range for detecting big magnets

    [Header("General Settings")]
    public float detectionRadius = 20f; // Radius for detecting magnets
    private bool isMagnetActive = false;
    public bool isMagnetized;

    private GameObject smallMagnetTarget; // Currently tracked small magnet
    private CharacterController characterController; // Reference to player movement
    private GameObject controllerScript;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //controllerScript = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Toggle magnet ability
        if (Input.GetKeyDown(KeyCode.M)) // Change "M" to your preferred key
        {
            isMagnetActive = !isMagnetActive;

            if (!isMagnetActive) // Release any active magnet
            {
                ReleaseSmallMagnet();
                isMagnetized = false;
            }
        }

        if (isMagnetActive)
        {
            HandleSmallMagnets();
            HandleBigMagnets();
        }
    }

    void HandleSmallMagnets()
    {
        if (smallMagnetTarget == null) // Look for a new small magnet if none is being targeted
        {
            Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius);

            foreach (Collider obj in nearbyObjects)
            {
                if (obj.CompareTag(smallMagnetTag))
                {
                    smallMagnetTarget = obj.gameObject;
                    break; // Stop searching after finding one
                }
            }
        }

        if (smallMagnetTarget != null)
        {
            // Calculate stop position in front of the player
            Vector3 stopPosition = transform.position + transform.forward * smallMagnetStopDistance;

            // Move the small magnet toward the stop position
            Rigidbody rb = smallMagnetTarget.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 directionToStop = (stopPosition - smallMagnetTarget.transform.position).normalized;
                float distanceToStop = Vector3.Distance(smallMagnetTarget.transform.position, stopPosition);

                if (distanceToStop > 0.1f) // If not yet at stop position
                {
                    rb.velocity = directionToStop * smallMagnetPullSpeed;
                }
                else
                {
                    rb.velocity = Vector3.zero; // Stop movement
                    smallMagnetTarget.transform.SetParent(transform); // Parent to the player
                }
            }
        }
    }

    void HandleBigMagnets()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider obj in nearbyObjects)
        {
            if (obj.CompareTag(bigMagnetTag))
            {
                Vector3 directionToMagnet = (obj.transform.position - transform.position).normalized;
                float distanceToMagnet = Vector3.Distance(transform.position, obj.transform.position);

                if (distanceToMagnet <= bigMagnetRange)
                {
                    // Move the player toward the magnet
                    characterController.Move(directionToMagnet * bigMagnetPullSpeed * Time.deltaTime);
                    isMagnetized = true;
                    
                }
            }
        }
    }

    void ReleaseSmallMagnet()
    {
        if (smallMagnetTarget != null)
        {
            smallMagnetTarget.transform.SetParent(null); // Unparent the magnet
            Rigidbody rb = smallMagnetTarget.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // Reset velocity
            }
            smallMagnetTarget = null; // Clear the reference
        }
    }
}
