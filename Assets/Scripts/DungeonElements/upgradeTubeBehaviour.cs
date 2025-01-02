using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using UnityEngine.Events;

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
            Debug.Log($"Powersources ({powerSources}/{reqPower}) in tube");
            Unlock();
        }
    }

    public void IncrementPower(){
        powerSources += 1;
        PowerCheck();
    }

    public void TryUpgrade(Collider other){
        if(other == null) return;
        
        if(other.gameObject == player){
            GetComponent<CapsuleCollider>().enabled = false;
            Upgrade();
        }
    }

    public void Upgrade()
    {
        if (isGoggles) {
            Debug.Log("Upgrading");
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
        Debug.Log("Opening door");
        anim.StopPlayback();
        anim.Play("openTube");
        player.GetComponent<PlayerController>().UnlockMovement();

        Interactable interactable;
        TryGetComponent<Interactable>(out interactable);
        interactable.OnFinish();
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
