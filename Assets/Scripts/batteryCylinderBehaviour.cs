using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batteryCylinderBehaviour : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject battery;

    [SerializeField] private GameObject powerPurpose;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "BatteryPrefab") {
            Debug.Log("print");
            Destroy(other.gameObject);
            battery.SetActive(true);
            anim.StopPlayback();
            anim.Play("Sink");

            upgradeTubeBehaviour powerPurposeScript = powerPurpose.GetComponent<upgradeTubeBehaviour>();
            powerPurposeScript.powerSources += 1;
            powerPurposeScript.PowerCheck();
        }
    }
}
