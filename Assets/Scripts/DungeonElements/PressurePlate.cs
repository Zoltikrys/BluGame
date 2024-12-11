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

    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "SmallMagnet")
        //{
        //    float distance = Vector3.Distance(transform.position, other.transform.position);
        //    //Debug.Log("Distance: " + distance);

        //    if(distance < 0.2f)
        //    {
        //        other.tag = "Untagged";
        //        Rigidbody box = GetComponent<Rigidbody>();
        //        if(box != null)
        //        {
                    
        //            box.isKinematic = true;
        //        }

        //        //Destroy(this);
        //    }
        //}

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null) {
            renderer.material.color = Color.blue;
        }

        triggerEventOne?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null) {
             renderer.material.color = Color.red;
        }

        triggerEventTwo?.Invoke();

        //Destroy(this);
    }

}
