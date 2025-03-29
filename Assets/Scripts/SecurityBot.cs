using System.Collections;
using UnityEngine;

public class SecurityBot : MonoBehaviour
{
    public enum AttackType { Multi, Single }
    public AttackType attackMode = AttackType.Multi;

    [SerializeField] private float spawnOffset = 1.8f; // Distance from center the bullet spawns

    //public GameObject floorVent;

    public GameObject activeCollision;
    public GameObject hiddenCollision;
    public GameObject vent;
    [SerializeField] private Transform ventPos;

    public Animator animator;

    public float shootInterval = 2f;
    private float shootTimer;

    private bool deathWait = false;

    public bool isVulnerable = false;

    public GameObject projectilePrefab;
    public Transform multiSpawnPoint;
    public int bulletCount = 8;
    public float bulletSpeed = 5f;
    public Transform singleSpawnPoint;

    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Finds Animator in child objects

        activeCollision.SetActive(true);
        hiddenCollision.SetActive(false);
        ventPos.transform.position = new Vector3(ventPos.transform.position.x, vent.transform.position.y, ventPos.transform.position.z);
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (!isVulnerable)
        {
            if (shootTimer >= shootInterval)
            {
                if (attackMode == AttackType.Multi)
                {
                    FireMulti();
                }
                else if (attackMode == AttackType.Single)
                {
                    StartCoroutine(FireSingle());
                }
                shootTimer = 0;
            }
        }

        if (transform.childCount == 6) // Minimum should be 6 (childCount - 3)
        {
            isVulnerable = true;
            Debug.Log("Hiding in vent");
            animator.SetTrigger("Hiding");
        }

        if (isVulnerable)
        {
            if (gameObject.transform.position.y >= ventPos.transform.position.y)
            {
                if (!deathWait)
                {
                    StartCoroutine(HideOffsetFix());
                }
                else if (deathWait)
                {
                    float xPos = gameObject.transform.position.x;
                    float yPos = gameObject.transform.position.y - 0.01f;
                    float zPos = gameObject.transform.position.z;
                    gameObject.transform.position = new Vector3(xPos, yPos, zPos);
                }
            }
            else
            {
                activeCollision.SetActive(false);
                hiddenCollision.SetActive(true);
            }
            
        }
    }

    private void FireMulti()
    {
        Debug.Log("Firing Bullet Hell Attack!");
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            // Calculate the spawn position based on the fixed distance and angle
            float offsetX = Mathf.Cos(angle * Mathf.Deg2Rad) * spawnOffset;
            float offsetZ = Mathf.Sin(angle * Mathf.Deg2Rad) * spawnOffset;

            // The spawn position is offset from the center spawn point by the calculated offsets
            Vector3 spawnPosition = multiSpawnPoint.position + new Vector3(offsetX, 0f, offsetZ);

            // Calculate the direction for the bullet to move
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirZ = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, 0f, bulletDirZ).normalized;

            // Instantiate the bullet at the calculated spawn position
            GameObject bullet = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = bulletMoveDirection * bulletSpeed;
            }
            angle += angleStep;
        }
    }

    private IEnumerator FireSingle()
    {
        Debug.Log("Firing Single Attack!");
        animator.SetTrigger("Firing");
        yield return new WaitForSeconds(1.01f); // Shoot animation time
        Debug.Log("Firing Laser Attack!");
        GameObject bullet = Instantiate(projectilePrefab, singleSpawnPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        Vector3 bulletMoveDirection = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;

        if (bulletRb != null)
        {
            bulletRb.velocity = bulletMoveDirection * bulletSpeed;
        }
    }

    private IEnumerator HideOffsetFix()
    {
        yield return new WaitForSeconds(1f); // Hide animation time
        deathWait = true;
    }
}
