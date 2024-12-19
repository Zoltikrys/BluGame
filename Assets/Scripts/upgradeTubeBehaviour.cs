using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeTubeBehaviour : MonoBehaviour
{
    [SerializeField] private int reqPower;
    [SerializeField] public int powerSources;
    [SerializeField] private GameObject goggles;

    [SerializeField] public Animator anim;

    [SerializeField] private bool isGoggles;
    [SerializeField] private bool isMagnet;
    //etc.

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        goggles = GameObject.Find("Goggles");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlock()
    {
        anim.Play("openTube");
    }

    public void PowerCheck()
    {
        if (powerSources >= reqPower) {
            Debug.Log("power check should have worked");
            Unlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player") {
            
            Upgrade();
        }
    }

    public void Upgrade()
    {
        if (isGoggles) {
            CloseDoor();
            GameObject player = GameObject.Find("Player");
            player.GetComponent<RgbGoggles>().enabled = true;
            goggles.SetActive(true);
            StartCoroutine(Cooldown(2f));
            anim.SetTrigger("Upgraded");
            //OpenDoor();
        }
    }

    public void OpenDoor()
    {
        anim.StopPlayback();
        anim.Play("openTube");
    }

    public void CloseDoor()
    {
        anim.StopPlayback();
        anim.Play("closeTube");
    }

    IEnumerator Cooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
