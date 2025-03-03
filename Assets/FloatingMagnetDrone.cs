using System.Collections;
using UnityEngine;

public class FloatingMagnetDrone : MonoBehaviour
{
    public enum MagnetType { Attract, Repel }
    public MagnetType magnetMode = MagnetType.Attract;
    public float magneticForce = 5f;
    public Transform[] patrolPoints;
    public float moveSpeed = 3f;
    public float pauseTime = 1f;
    private int currentPointIndex = 0;
    private bool isPaused = false;

    public ParticleSystem magnetEffect;
    public Color attractColor = Color.blue;
    public Color repelColor = Color.red;
    private ParticleSystem.MainModule particleMain;

    private void Start()
    {
        if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[0].position;
        }
        StartCoroutine(Patrol());

        if (magnetEffect != null)
        {
            particleMain = magnetEffect.main;
            UpdateParticleEffect();
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            if (!isPaused && patrolPoints.Length > 1)
            {
                Transform targetPoint = patrolPoints[currentPointIndex];
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
                {
                    currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                    isPaused = true;
                    yield return new WaitForSeconds(pauseTime);
                    isPaused = false;
                }
            }
            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController playerController = other.GetComponent<CharacterController>();
            if (playerController != null)
            {
                Vector3 forceDirection = (magnetMode == MagnetType.Attract) ? (transform.position - other.transform.position) : (other.transform.position - transform.position);
                Vector3 moveVector = forceDirection.normalized * magneticForce * Time.deltaTime;
                playerController.Move(moveVector);
            }
        }
    }

    public void SetMagnetMode(MagnetType newMode)
    {
        magnetMode = newMode;
        UpdateParticleEffect();
    }

    private void UpdateParticleEffect()
    {
        if (magnetEffect != null)
        {
            particleMain.startColor = (magnetMode == MagnetType.Attract) ? attractColor : repelColor;
        }
    }
}
