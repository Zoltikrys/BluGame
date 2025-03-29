using UnityEngine;

public class SecurityBot : MonoBehaviour
{
    public enum AttackType { Multi, Single }
    public AttackType attackMode = AttackType.Multi;

    [SerializeField] private float spawnOffset = 1.8f; // Distance from center the bullet spawns

    public GameObject floorVent;

    public float shootInterval = 2f;
    private float shootTimer;

    public bool isVulnerable = false;
    public bool isHiding = false;

    public GameObject projectilePrefab;
    public Transform multiSpawnPoint;
    public int bulletCount = 8;
    public float bulletSpeed = 5f;
    public Transform singleSpawnPoint;

    void Start()
    {

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
                    FireSingle();
                }

                shootTimer = 0;
            }
        }

        if (transform.childCount == 4) // Minimum should be 4 as there should be at least the vent location, main body mesh and bullet spawn point, may increase later with updates to code
        {
            if (floorVent != null)
            {
                Debug.Log("Hiding in vent");
                transform.position = floorVent.transform.position;
                gameObject.SetActive(false);
            }
        }
    }

    private void FireMulti()
    {
        Debug.Log("Firing Bullet Hell Attack!");
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        // Fixed distance away from the spawn point
        

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



    private void FireSingle()
    {
        Debug.Log("Firing Laser Attack!");
        GameObject bullet = Instantiate(projectilePrefab, singleSpawnPoint.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        Vector3 bulletMoveDirection = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;

        if (bulletRb != null)
        {
            bulletRb.velocity = bulletMoveDirection * bulletSpeed;
        }
    }
}
