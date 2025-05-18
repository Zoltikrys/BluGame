using System.Collections;
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

    [SerializeField] private GameObject LeftDoor;
    [SerializeField] private GameObject RightDoor;
    [SerializeField] private CapsuleCollider CapsuleCollider;
    void Start()
    {
        // Assigns key variables
        anim = GetComponent<Animator>();
        goggles = GameObject.Find("Goggles");
        player = GameObject.Find("Player");
        LeftDoor = GameObject.Find("LeftDoor");
        RightDoor = GameObject.Find("RightDoor");
        
        if(powerSources >= reqPower) Unlock();
    }

    public void Unlock()
    {
        LeftDoor.SetActive(false);
        RightDoor.SetActive(false);
        //anim.Play("openTube"); animations broke
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
            CapsuleCollider.enabled = false;
            Upgrade();
        }
    }

    // Function can be added to for more upgrade options in the future
    public void Upgrade()
    {
        // Provides the Goggles upgrade
        if (isGoggles) {
            Debug.Log("Upgrading Goggles");
            CloseDoor();
            player.GetComponent<PlayerController>().LockMovement();
            player.GetComponent<RgbGoggles>().GogglesActivated = true;
            player.GetComponent<RgbGoggles>().gogglesObject.SetActive(true);

            StartCoroutine(Cooldown(2f, OpenDoor));
        }
        // Provides the magnet upgrade
        if(isMagnet){
            Debug.Log("Upgrading Magnet");
            CloseDoor();
            player.GetComponent<PlayerController>().LockMovement();
            player.GetComponent<MagnetAbility>().isMagnetAbilityActive = true;
            player.GetComponent<MagnetAbility>().enabled = true;
            StartCoroutine(Cooldown(2f, OpenDoor));
        }
    }

    // Opens the door using the animator
    public void OpenDoor()
    {
        Debug.Log("Opening door");
        LeftDoor.SetActive(false);
        RightDoor.SetActive(false);
        //anim.StopPlayback();
        //anim.Play("openTube");
        player.GetComponent<PlayerController>().UnlockMovement();

        Interactable interactable;
        TryGetComponent<Interactable>(out interactable);
        interactable.OnFinish();
    }

    public void CloseDoor()
    {
        //anim.StopPlayback();
        //anim.Play("closeTube");
        LeftDoor.SetActive(true);
        RightDoor.SetActive(true);
    }

    IEnumerator Cooldown(float waitTime, Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }
}
