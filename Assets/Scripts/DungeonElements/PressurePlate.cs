using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    [Header("What should this pressure plate activate?")]
    private UnityEvent triggerEventOne;
    [SerializeField]
    [Header("What should happen when the pressure is removed?")]
    private UnityEvent triggerEventTwo;

    public BoxCollider BoxCollider;

    [SerializeField] private Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.Play("Press");

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null) {
            renderer.material.color = Color.blue;
        }
        Debug.Log("trying to do it");
        triggerEventOne?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        anim.Play("Release");

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null) {
             renderer.material.color = Color.red;
        }

        triggerEventTwo?.Invoke();

        //Destroy(this);
    }

}
