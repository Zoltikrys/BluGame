using UnityEngine;

public class FanPush : MonoBehaviour
{
    public float pushForce = 10f; // The strength of the fan's push
    public Vector3 fanDirection = Vector3.forward; // Direction the fan pushes, make sure this is local direction (relative to fan's rotation)
    //public float pushRadius = 5f; // Radius of effect of the fan - this is useless right now actually because the fan only pushes if you're in the collider

    [SerializeField] private bool isOn = true; //on by default

    public Animator animator;

    private void Update()
    {
        if (isOn)
        {
            animator.enabled = true;
        }

        if (!isOn)
        {
            animator.enabled = false;
        }
    }






    private void OnTriggerStay(Collider other)
    {

        if (isOn)
        {
            
            //animator.Play("AnimStateName");
            // Check if the object is the player (assuming player has a tag "Player") - added a bit for small magnets, if we're having the bombs pick-up-able then giving those the magnet tag and putting them on fans could lead to interesting gameplay?
            if (other.CompareTag("Player") || other.CompareTag("SmallMagnet"))
            {
                CharacterController characterController = other.GetComponent<CharacterController>();
                if (characterController != null)
                {
                    // Apply a force in the direction the fan is pointing
                    Vector3 forceDirection = transform.TransformDirection(fanDirection); // Apply local fan direction
                                                                                         // Move the player character by applying the force
                    characterController.Move(forceDirection * pushForce * Time.deltaTime);
                }
            }
        }
        else if (!isOn)
        {
            
        }
        
    }

    public void ToggleOnOff()
    {
        isOn = !isOn;
    }
    
    public void TurnOn()
    {
        isOn = true;
    }

    public void TurnOff()
    {
        isOn = false;
    }

    /*private void OnDrawGizmos()
    {
        // Show the fan's direction in the editor as a line
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(fanDirection) * pushRadius);
    }*/
}
