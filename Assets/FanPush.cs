using UnityEngine;

public class FanPush : MonoBehaviour
{
    public float pushForce = 10f; // The strength of the fan's push
    public Vector3 fanDirection = Vector3.forward; // Direction the fan pushes, make sure this is local direction (relative to fan's rotation)
    public float pushRadius = 5f; // Radius of effect of the fan

    private void OnTriggerStay(Collider other)
    {
        // Check if the object is the player (assuming player has a tag "Player")
        if (other.CompareTag("Player"))
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

    private void OnDrawGizmos()
    {
        // Show the fan's direction in the editor as a line
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(fanDirection) * pushRadius);
    }
}
