using System;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CraneControls : MonoBehaviour
{
    public Crane Crane;
    [field: SerializeField] public int MaxDepth {get; set;}
    [field: SerializeField] public int MaxHeight {get; set;}
    [field: SerializeField] public int DescendSpeed {get; set;}
    [field: SerializeField] public int MovementSpeed{get; set;}
    private bool playerInZone = false;
    private bool craneActive = false;
    private CraneDirection craneDirection = CraneDirection.NONE;
    private float direction = 0.0f;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") playerInZone = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player") playerInZone = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && playerInZone && !craneActive) ActivateCrane();
        else if(Input.GetKeyDown(KeyCode.F) && craneActive) DectivateCrane();

        if(!craneActive) return;

        ProcessCraneControls();
        if(Crane) Crane.SetCrane(craneDirection, DescendSpeed, MovementSpeed, MaxDepth, MaxHeight);
        
    }

    private void ProcessCraneControls()
    {
        float currentInput = Input.GetAxis("Mouse ScrollWheel");
        if(currentInput < 0) direction -= 1.0f;
        if(currentInput > 0) direction += 1.0f;

        direction = Mathf.Clamp(direction, -1.0f, 1.0f);

        if(direction < 0) craneDirection = CraneDirection.DOWN;
        else if(direction > 0) craneDirection = CraneDirection.UP;
        else craneDirection = CraneDirection.NONE;

    }

    void ActivateCrane(){
        Debug.Log("Activated crane");
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.LockMovement();
        craneActive = true;
    }

    void DectivateCrane(){
        Debug.Log("Deactivated crane");
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.UnlockMovement();
        craneActive = false;
    }


}
