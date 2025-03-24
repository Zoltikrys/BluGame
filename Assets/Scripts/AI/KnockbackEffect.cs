using System.Collections;
using UnityEngine;

public class KnockbackEffect : MonoBehaviour
{
    public float strength;
    public float duration = 1.0f; 

    public void ApplyKnockback(Rigidbody targetRigidbody, Vector3 knockbackDirection)
    {
        if (targetRigidbody == null) return;

        knockbackDirection.Normalize();
        targetRigidbody.AddForce(knockbackDirection * strength, ForceMode.VelocityChange);

        StartCoroutine(ResetVelocityAfterDelay(targetRigidbody));
    }

     public void ApplyKnockback(CharacterController targetRigidbody, Vector3 knockbackDirection)
    {
        if (targetRigidbody == null) return;
        if (targetRigidbody.GetComponent<HealthManager>().b_Health < 1) return;
        //if (targetRigidbody.GetComponent<HealthManager>().m_DamageCooldown > 0) return;

        knockbackDirection.Normalize();

        StartCoroutine(PerformKnockback(targetRigidbody, knockbackDirection));
    }

    public void ApplyRadialKnockback(CharacterController targetController, Vector3 stompOrigin)
    {
        if (targetController == null) return;
        if (targetController.GetComponent<HealthManager>().b_Health < 1) return;

        // Calculate knockback direction outward from the stomp center
        Vector3 knockbackDirection = (targetController.transform.position - stompOrigin).normalized;

        StartCoroutine(PerformKnockback(targetController, knockbackDirection));
    }


    private IEnumerator PerformKnockback(CharacterController targetController, Vector3 knockbackDirection)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate displacement for this frame
            Vector3 displacement = knockbackDirection * strength * Time.deltaTime;

            // Move the CharacterController
            targetController.Move(displacement);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ResetVelocityAfterDelay(Rigidbody targetRigidbody)
    {
        yield return new WaitForSeconds(duration);
        //targetRigidbody.velocity = Vector3.zero;
    }

}