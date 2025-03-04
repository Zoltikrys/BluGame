using UnityEngine;

public class ArmouredSecurityBot : MonoBehaviour
{
    public enum AttackType { BulletHell, Laser }
    public AttackType attackMode = AttackType.BulletHell;

    public GameObject floorVent;

    public float shootInterval = 2f;
    private float shootTimer;

    public bool isVulnerable = false;
    public bool isHiding = false;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public int bulletCount = 8;
    public float bulletSpeed = 5f;
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;

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
                if (attackMode == AttackType.BulletHell)
                {
                    FireBulletHell();
                }
                else if (attackMode == AttackType.Laser)
                {
                    FireLaser();
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


    private void FireBulletHell()
    {
        Debug.Log("Firing Bullet Hell Attack!");
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0f).normalized;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = bulletMoveDirection * bulletSpeed;
            }

            angle += angleStep;
        }
    }

    private void FireLaser()
    {
        Debug.Log("Firing Laser Attack!");
        GameObject laser = Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
        laser.transform.forward = transform.forward;
    }
}
