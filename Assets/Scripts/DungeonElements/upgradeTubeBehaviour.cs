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

    [field: SerializeField] public List<UnityEvent> OnFinishEvents = new List<UnityEvent>();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player") {
            Debug.Log("Player entered tube");
            GetComponent<CapsuleCollider>().enabled = false; // turn off collider so it triggers once per room
            Upgrade();
        }
    }

    public void Upgrade()
    {
        if (isGoggles) {
            Debug.Log("Updagrading");
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
        OnFinish();
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

    public void OnFinish(){
        foreach(UnityEvent ev in OnFinishEvents){
            ev?.Invoke();
        }
    }
}
