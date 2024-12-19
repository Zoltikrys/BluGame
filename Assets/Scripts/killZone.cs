using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player") {
            HealthManager healthMan = other.GetComponent<HealthManager>();
            healthMan.Damage();
            healthMan.Damage();
            healthMan.Damage();
        }
    }
}
