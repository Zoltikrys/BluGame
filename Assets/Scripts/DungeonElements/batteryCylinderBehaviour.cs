using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batteryCylinderBehaviour : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject battery;

    [SerializeField] private GameObject powerPurpose;

    [field: SerializeField] public bool IsActive {get; set;} = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(IsActive) anim.Play("Rise");
        else anim.Play("Sink");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == battery.name && IsActive) {
            Debug.Log("Battery accepted");
            Destroy(other.gameObject);
            battery.SetActive(true);
            anim.StopPlayback();
            anim.Play("Sink");

            upgradeTubeBehaviour powerPurposeScript = powerPurpose.GetComponent<upgradeTubeBehaviour>();
            powerPurposeScript.powerSources += 1;
            powerPurposeScript.PowerCheck();
            IsActive = false;
        }
    }

    public void Activate(){
        IsActive = true;
        anim.Play("Rise");
    }
}
