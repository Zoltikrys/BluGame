using System.Collections;
using UnityEngine;

public class ArmouredSecurityBot : MonoBehaviour
{
    public enum AttackType { BulletHell, Laser }
    public AttackType attackMode = AttackType.BulletHell;

    public GameObject floorVent;
    public float attackInterval = 2f;
    private bool isVulnerable = false;
    private bool isHiding = false;

    void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    private void Update()
    {

        if (transform.childCount == 2) //Minimum should be 2 as there should be at least the vent location and main body mesh
        {
            if (floorVent != null)
            {
                Debug.Log("Hiding in vent");
                transform.position = floorVent.transform.position;
                gameObject.SetActive(false);
            }   
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (!isVulnerable)
        {
            yield return new WaitForSeconds(attackInterval);
            if (attackMode == AttackType.BulletHell)
            {
                FireBulletHell();
            }
            else if (attackMode == AttackType.Laser)
            {
                FireLaser();
            }
        }
    }


    /*private void BecomeVulnerable()
    {
        isVulnerable = true;
        StopAllCoroutines();
        StartCoroutine(HideInVent());
    }

    private IEnumerator HideInVent()
    {
        yield return new WaitForSeconds(1f); // Small delay before escaping
        if (floorVent != null)
        {
            transform.position = floorVent.transform.position;
            gameObject.SetActive(false);
        }
    }*/

    private void FireBulletHell()
    {
        Debug.Log("Firing Bullet Hell Attack!");
        // TODO: Implement actual projectile spawning
    }

    private void FireLaser()
    {
        Debug.Log("Firing Laser Attack!");
        // TODO: Implement laser attack logic
    }
}
