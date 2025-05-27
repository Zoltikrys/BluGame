using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetAbility : MonoBehaviour
{
    [Header("Small Magnet Settings")]
    public string smallMagnetTag = "SmallMagnet"; // Tag for small magnetic objects
    public float smallMagnetPullSpeed = 10f;     // Speed for pulling small magnets
    public float smallMagnetStopDistance = 1.5f; // Distance in front of the player
    bool shootMode = false;
    public int smallMagnetShootForce = 20; // Force applied when shooting

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
    [SerializeField] private Animator animator;

    [Header("Magnet UI")]
    [SerializeField] private Image magnetBack;
    [SerializeField] private Image magnetFront;
    [SerializeField] private GameObject magnetVibes;
    [SerializeField] private List<ParticleSystem> magnetParticles;

    [Header("Magnet Visuals")]
    [SerializeField] private Material magnetBevelMat;
    [SerializeField] private GameObject magneticBeam;
    [SerializeField] private Transform beamOrigin; // the point on the player the beam starts from

    [field: SerializeField] public GameObject smallMagnetTarget { get; private set; }// Currently tracked small magnet
    private bool smallMagnetTargetMagnetised = false;
    private CharacterController characterController; // Reference to player movement
    private GameObject controllerScript;

    [field: SerializeField] public List<BatteryEffect> MagnetBatteryCost {get; set;}

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();

        if (magnetBack == null) {
            var canvasLayer = GameObject.FindGameObjectWithTag("UI_MAGBACK");
            if (canvasLayer != null) canvasLayer.TryGetComponent<Image>(out magnetBack);
        }
        if (magnetFront == null) {
            var canvasScanlines = GameObject.FindGameObjectWithTag("UI_MAGFRONT");
            if (canvasScanlines != null) canvasScanlines.TryGetComponent<Image>(out magnetFront);
        }
    }

    void Update()
    {
        // Toggle magnet ability
        if(smallMagnetTargetMagnetised && smallMagnetTarget == null) TurnOffMagnet(); // fires when our gameobject is consumed or deleted out of the scene.
        if(!isMagnetAbilityActive) return;

        if (isMagnetActive)
        {
            isMagnetized = false;
            HandleSmallMagnets();
            HandleBigMagnets();
        }
    }

    public void MagnetInput() {
        if(!isMagnetAbilityActive) return;
        if (!isMagnetActive) {
            if (GetComponent<Battery>().AttemptAddBatteryEffects(MagnetBatteryCost, true)) {
                isMagnetActive = true;
                magnetFront.color = Color.white;
                animator.SetBool("Magnet?", true);
            }
        }
        else 
        {
            isMagnetActive = false;
            isMagnetized = false;
            magnetFront.color = Color.clear;
            GetComponent<Battery>().RemoveBatteryEffects(MagnetBatteryCost);
            animator.SetBool("Magnet?", false);
            TurnOffMagnet();
        }
    }

    public void ShootMode() {
        if (shootMode) {
            Debug.Log("Shoot mode off");
            shootMode = false;
        }
        else if (!shootMode) {
            Debug.Log("Shoot mode on");
            shootMode = true;
        }
    }

    void ShootSmallMagnet()
    {
        if (smallMagnetTarget != null)
        {
            smallMagnetTarget.transform.SetParent(null); // Unparent the magnet
            Rigidbody rb = smallMagnetTarget.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(0, 0, smallMagnetShootForce); //Shoot magnet
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;

            }
            smallMagnetTarget = null; // Clear the reference
        }
    }

    void HandleSmallMagnets()
    {
        if (smallMagnetTarget == null) // Look for a new small magnet if none is being targeted
        {
            // Disable beam if not pulling anything
            if (magneticBeam != null && magneticBeam.activeSelf)
            {
                magneticBeam.SetActive(false);
            }

            Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, detectionRadius);

            float MinDistance = Mathf.Infinity;
            GameObject newTarget = null;

            foreach (Collider obj in nearbyObjects)
            {
                if (obj.CompareTag(smallMagnetTag))
                {
                    Vector3 toObject = (obj.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, toObject);

                    if (angle <= FrontConeAngle / 2)
                    {
                        float distance = Vector3.Distance(transform.position, obj.transform.position);
                        if (distance <= MinDistance)
                        {
                            MinDistance = distance;
                            newTarget = obj.gameObject;
                        }
                    }
                }
            }

            // Assign new target and apply material if found
            if (newTarget != null)
            {
                smallMagnetTarget = newTarget;

                Renderer rend = smallMagnetTarget.GetComponent<Renderer>();
                if (rend != null && magnetBevelMat != null)
                {
                    Material[] originalMats = rend.materials;
                    if (originalMats.Length == 1)
                    {
                        Material[] newMats = new Material[2];
                        newMats[0] = originalMats[0];
                        newMats[1] = magnetBevelMat;
                        rend.materials = newMats;
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

                if (distanceToStop > 0.1f)
                {
                    rb.velocity = directionToStop * smallMagnetPullSpeed;
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    smallMagnetTarget.transform.SetParent(transform);
                    rb.useGravity = false;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }

                SetOtherSmallMagnetsKinematic(true);
            }

            // Update and show magnetic beam
            if (magneticBeam != null && beamOrigin != null)
            {
                magneticBeam.SetActive(true);

                Vector3 start = beamOrigin.position;
                Vector3 end = smallMagnetTarget.transform.position;
                Vector3 direction = end - start;
                float distance = direction.magnitude;

                // Position at midpoint
                magneticBeam.transform.position = start + direction * 0.5f;

                // Scale lengthwise (Y-axis)
                magneticBeam.transform.localScale = new Vector3(
                    magneticBeam.transform.localScale.x,
                    distance * 0.5f,
                    magneticBeam.transform.localScale.z
                );

                // Rotate to face the magnet
                magneticBeam.transform.rotation = Quaternion.LookRotation(direction);
                magneticBeam.transform.Rotate(90f, 0f, 0f); // Adjust for vertical cylinder
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
                if (shootMode)
                {
                    rb.velocity = transform.forward * smallMagnetShootForce;    //new Vector3(0, 0, smallMagnetShootForce); //Shoot magnet
                    rb.useGravity = true;
                    rb.constraints = RigidbodyConstraints.None;
                }
                else
                {
                    rb.velocity = Vector3.zero; //Reset velocity
                    rb.useGravity = true;
                    rb.constraints = RigidbodyConstraints.None;
                }

            // Remove second material on release
            if (smallMagnetTarget != null)
            {
                Renderer rend = smallMagnetTarget.GetComponent<Renderer>();
                if (rend != null)
                {
                    Material[] currentMats = rend.materials;
                    if (currentMats.Length > 1)
                    {
                        Material[] newMats = new Material[1];
                        newMats[0] = currentMats[0];
                        rend.materials = newMats;
                    }
                }
            }
            magneticBeam.SetActive(false);
            smallMagnetTarget = null; // Clear the reference
        }
        SetOtherSmallMagnetsKinematic(false); // Return all small magnets to non-kinematic
    }

    public void TurnOffMagnet(){
        ReleaseSmallMagnet();
        isMagnetized = false;
        isMagnetActive = false;
        smallMagnetTargetMagnetised = false;
        magnetFront.color = Color.clear;
        GetComponent<Battery>().RemoveBatteryEffects(MagnetBatteryCost);
        animator.SetBool("Magnet?", false);
    }

    void SetOtherSmallMagnetsKinematic(bool makeKinematic)
    {
        GameObject[] allMagnets = GameObject.FindGameObjectsWithTag(smallMagnetTag);
        foreach (GameObject magnet in allMagnets)
        {
            if (smallMagnetTarget != null && magnet == smallMagnetTarget)
                continue; // Skip the one we're holding

            Rigidbody rb = magnet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = makeKinematic;
        }
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
