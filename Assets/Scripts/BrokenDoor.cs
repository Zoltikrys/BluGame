using UnityEngine;
using UnityEngine.VFX;

public class BrokenDoor : MonoBehaviour
{
    [SerializeField] private int partsRequiredToFix = 5;
    private bool isFixed = false;

    public bool canInteract = false;

    [SerializeField] private doorBehaviour door;

    private GameObject player;

    public VisualEffect smokeEffect;
    public VisualEffect sparksEffect;

    public float sparksTimer = 0f;
    public float sparksThreshold = 5f;


    private void Start()
    {
        smokeEffect.Play();
    }


    private void Update()
    {
        if (isFixed)
        {
            smokeEffect.Stop();
        }

        if (!isFixed)
        {
            sparksTimer += Time.deltaTime;
            if (sparksTimer >= sparksThreshold)
            {
                sparksEffect.Play();
                sparksTimer = 0f;
            }
            
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (isFixed) return;


    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //set player gameObject
            player = other.gameObject;
            //set brokenDoor as gameobject in PlayerLocomotionInput
            player.GetComponent<PlayerLocomotionInput>().brokenDoor = gameObject;
            //player can now interact with door
            canInteract = true;
            //show interact icon above player
            other.transform.GetChild(1).gameObject.GetComponent<InteractText>().ShowText();
        }
    }

    private void OnTriggerExit(Collider other) {
        //player can NOT interact with door
        canInteract = false;
        //hide interact icon above player
        other.transform.GetChild(1).gameObject.GetComponent<InteractText>().HideText();
    }

    public void BrokenDoorInteract() {
        CollectibleCollection playerParts = player.GetComponent<CollectibleCollection>();

        if (playerParts != null && playerParts.TrySpendParts(partsRequiredToFix)) {
            Debug.Log("Door fixed!");
            isFixed = true;
            door.OpenDoor();
        }
        else {
            Debug.Log("Not enough parts to fix the door.");
        }
    }
}
