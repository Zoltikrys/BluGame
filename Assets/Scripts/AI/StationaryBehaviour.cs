using System.Collections;
using UnityEngine;

public class StationaryBehaviour : Enemy
{
    IEnumerator IdleMovement()
    {
        while (CurrentState == State.Idle)
        {
            Debug.Log("Idling...");

            transform.Rotate(Vector3.forward, 10);
            yield return new WaitForSeconds(0.01f);
        }
    }

    protected override void IdleState(){
        IdleMovement();
    }

    protected override void TargetingState(){
        Quaternion OriginalRot = transform.rotation;
        transform.LookAt(new Vector3(target.position.x,
                                     transform.position.y,
                                     target.transform.position.z));
        Quaternion NewRot = transform.rotation;
        transform.rotation = OriginalRot;
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, 5f * Time.deltaTime);
    }

    protected override void AttackState()
    {
        //
        Attack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CurrentState = State.Targeting;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CurrentState = State.Targeting;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CurrentState = State.Idle;
        }
    }

    IEnumerator Attack()
    {
        if (attackCooldown <= 0f)
        {
            Debug.Log("attack");
            yield return new WaitForSeconds(3);
        }

        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }
}

