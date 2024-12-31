using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public BoxCollider BoxCollider;
    [SerializeField] private Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Press(){
        anim.Play("Press");

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null) {
            renderer.material.color = Color.blue;
        }
    }

    public void Release(){
        anim.Play("Release");

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null) {
             renderer.material.color = Color.red;
        }
    }

}
