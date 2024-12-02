using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StationaryBehaviour : MonoBehaviour
{
    public Transform target;
    public Transform rotateBody;

    public bool targetTag = false;

    public float attackCooldown = 3f;


    public enum EnemyState
    {
        Idle,
        Track,
        Attack
    }

    [SerializeField] EnemyState _currentState = EnemyState.Idle;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
                targetTag = false;
                IdleMovement();
                break;
            case EnemyState.Track:
                targetTag = true;
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }

        if (_currentState == EnemyState.Idle)
        {
            IdleMovement();
            transform.Rotate(Vector3.up * 10f * Time.deltaTime);
        }

        if (_currentState == EnemyState.Track)
        {
            //Vector3 targetPosition = new Vector3(target.transform.position.x,
            // transform.position.y,
            // target.transform.position.z);

            //transform.LookAt(targetPosition);

            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(new Vector3(target.transform.position.x,
            transform.position.y,
            target.transform.position.z));
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, 5f * Time.deltaTime);

        }
    }

    IEnumerator IdleMovement()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward, 10);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _currentState = EnemyState.Track;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _currentState = EnemyState.Track;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _currentState = EnemyState.Idle;
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

