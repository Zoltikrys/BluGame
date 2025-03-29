using UnityEngine;

public class SecurityBot : MonoBehaviour
{
    public enum AttackType { Multi, Single }
    public AttackType attackMode = AttackType.Multi;

    public GameObject floorVent;

    public float shootInterval = 2f;
    private float shootTimer;

    public bool isVulnerable = false;
    public bool isHiding = false;

    public GameObject projectilePrefab;
    public Transform multiSpawnPoint;
    public int bulletCount = 8;
    public float bulletSpeed = 5f;
    //public GameObject laserPrefab;
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

        if (transform.childCount == 3) // Minimum should be 3 as there should be at least the vent location, main body mesh and bullet spawn point, may increase later with updates to code
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

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirZ = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, 0f, bulletDirZ).normalized;

            GameObject bullet = Instantiate(projectilePrefab, multiSpawnPoint.position, Quaternion.identity);
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
       //bullet.transform.forward = transform.forward;
    }
}
