using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerSource : MonoBehaviour
{
    
    [SerializeField]
    private int powerSourceCharge = 3;
    [SerializeField]
    private bool bossCharge;

    [SerializeField]
    private GameObject bossShield;

    [SerializeField] private GameObject batteryMesh;

    private void Update()
    {

    }

    public void TakeDamage()
    {
        powerSourceCharge--;

        if(powerSourceCharge <= 0)
        {
            DestroySequence();
        }
    }

    public void DestroySequence()
    {
        bossShield.GetComponent<bossShield>().powerSourceFlags ++;
        if( bossShield.GetComponent<bossShield>().powerSourceFlags == bossShield.GetComponent<bossShield>().powerSourceTarget)
        {
            bossShield.GetComponent<bossShield>().OpenSequence();
        }

        batteryMesh.GetComponent<MeshFilter>().mesh = null;
        gameObject.GetComponent<BoxCollider>().enabled = false;

        Debug.Log("yeowch! you just destroyed a power cell!");
        //Destroy(gameObject);
    }
}
