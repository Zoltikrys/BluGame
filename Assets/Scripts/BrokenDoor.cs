using UnityEngine;
using UnityEngine.VFX;

public class BrokenDoor : MonoBehaviour
{
    [SerializeField] private int partsRequiredToFix = 5;
    private bool isFixed = false;

    [SerializeField] private doorBehaviour door;

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

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.L)) // Or your interact key
        {
            CollectibleCollection playerParts = other.GetComponent<CollectibleCollection>();

            if (playerParts != null && playerParts.TrySpendParts(partsRequiredToFix))
            {
                Debug.Log("Door fixed!");
                isFixed = true;
                door.OpenDoor(); 
            }
            else
            {
                Debug.Log("Not enough parts to fix the door.");
            }
        }
    }
}
