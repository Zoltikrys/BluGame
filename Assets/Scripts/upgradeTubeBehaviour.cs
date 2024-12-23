using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class upgradeTubeBehaviour : MonoBehaviour
{
    [SerializeField] private int reqPower;
    [SerializeField] public int powerSources;
    [SerializeField] private GameObject goggles;
    [SerializeField] private GameObject player;

    [SerializeField] public Animator anim;

    [SerializeField] private bool isGoggles;
    [SerializeField] private bool isMagnet;
    //etc.

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        goggles = GameObject.Find("Goggles");
        player = GameObject.Find("Player");
        
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
            GetComponent<CapsuleCollider>().enabled = false; // turn off collider so it triggers once per room
            Upgrade();
        }
    }

    public void Upgrade()
    {
        if (isGoggles) {
            CloseDoor();
            player.GetComponent<PlayerController>().LockMovement();
            player.GetComponent<RgbGoggles>().GogglesActivated = true;
            //goggles.SetActive(true);   Was this for the top of the upgrade tube?

            StartCoroutine(Cooldown(2f, OpenDoor));
            //anim.SetTrigger("Upgraded");
        }
    }

    public void OpenDoor()
    {
        anim.StopPlayback();
        anim.Play("openTube");
        player.GetComponent<PlayerController>().UnlockMovement();
    }

    public void CloseDoor()
    {
        anim.StopPlayback();
        anim.Play("closeTube");
    }

    IEnumerator Cooldown(float waitTime, Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }
}
