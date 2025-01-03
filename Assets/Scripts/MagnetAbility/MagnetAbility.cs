using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public float FrontConeAngle = 60f;
    public Color DebugSphereColour = Color.cyan;
    public Color DebugConeColour = Color.yellow;
    private bool isMagnetActive = false;
    public bool isMagnetized;
    public bool isMagnetAbilityActive = true;

    [field: SerializeField] public GameObject smallMagnetTarget { get; private set; }// Currently tracked small magnet
    private bool smallMagnetTargetMagnetised = false;
    private CharacterController characterController; // Reference to player movement
    private GameObject controllerScript;

    [field: SerializeField] public List<BatteryEffect> MagnetBatteryCost {get; set;}

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //controllerScript = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Toggle magnet ability
        if(smallMagnetTargetMagnetised && smallMagnetTarget == null) TurnOffMagnet(); // fires when our gameobject is consumed or deleted out of the scene.
   
        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Fire2")) // Change "M" to your preferred key
        {
            if(!isMagnetActive){
                if(GetComponent<Battery>().AttemptAddBatteryEffects(MagnetBatteryCost, true)){
                    isMagnetActive = true;
                }
            }
            else {
                isMagnetActive = false;
                GetComponent<Battery>().RemoveBatteryEffects(MagnetBatteryCost);
            }

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

            float MinDistance = Mathf.Infinity;

            foreach (Collider obj in nearbyObjects)
            {
                if (obj.CompareTag(smallMagnetTag))
                {
                    Vector3 toObject = (obj.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, toObject);

                    if (angle <= FrontConeAngle / 2) // angle detection to make sure the magnet is magnetising from the front
                    {
                        if(Vector3.Distance(transform.position, obj.transform.position) <= MinDistance){
                            MinDistance = Vector3.Distance(transform.position, obj.transform.position);
                            smallMagnetTarget = obj.gameObject;
                        }
                    }
                }
            }
        }

        if (smallMagnetTarget != null)
        {
            smallMagnetTargetMagnetised = true;
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
                    rb.useGravity = false;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
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
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;

            }
            smallMagnetTarget = null; // Clear the reference
        }
    }

    public void TurnOffMagnet(){
        ReleaseSmallMagnet();
        isMagnetized = false;
        isMagnetActive = false;
        smallMagnetTargetMagnetised = false;
        GetComponent<Battery>().RemoveBatteryEffects(MagnetBatteryCost);
    }

    private void OnDrawGizmos()
{
    // Draw the OverlapSphere
    Gizmos.color = DebugSphereColour;
    Gizmos.DrawWireSphere(transform.position, detectionRadius);

    // Draw the cone
    Gizmos.color = DebugConeColour;

    // Calculate the cone edges
    Vector3 forward = transform.forward * detectionRadius;
    Quaternion leftRotation = Quaternion.Euler(0, -FrontConeAngle / 2, 0);
    Quaternion rightRotation = Quaternion.Euler(0, FrontConeAngle / 2, 0);

    Vector3 leftEdge = leftRotation * forward;
    Vector3 rightEdge = rightRotation * forward;

    // Draw cone lines
    Gizmos.DrawLine(transform.position, transform.position + leftEdge);
    Gizmos.DrawLine(transform.position, transform.position + rightEdge);

    // Optional: Draw a wire arc for the cone
    int segments = 20; // Number of segments for the arc
    for (int i = 0; i <= segments; i++)
    {
        float angle = -FrontConeAngle / 2 + (FrontConeAngle * i / segments);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        Vector3 point = transform.position + rotation * forward;
        Gizmos.DrawLine(transform.position + rotation * forward, point);
    }
}
}
