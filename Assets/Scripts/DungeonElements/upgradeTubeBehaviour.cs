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

    void Start()
    {
        // Assigns key variables
        anim = GetComponent<Animator>();
        goggles = GameObject.Find("Goggles");
        player = GameObject.Find("Player");
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

    // Function can be added to for more upgrade options in the future
    public void Upgrade()
    {
        // Provides the Goggles upgrade
        if (isGoggles) {
            Debug.Log("Upgrading");
            CloseDoor();
            player.GetComponent<PlayerController>().LockMovement();
            player.GetComponent<RgbGoggles>().GogglesActivated = true;
            player.GetComponent<RgbGoggles>().gogglesObject.SetActive(true);

            StartCoroutine(Cooldown(2f, OpenDoor));
        }
    }

    // Opens the door using the animator
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
