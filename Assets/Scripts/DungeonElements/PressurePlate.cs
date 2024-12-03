using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "SmallMagnet")
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            Debug.Log("Distance: " + distance);

            if(distance < 0.2f)
            {
                other.tag = "Untagged";
                Rigidbody box = GetComponent<Rigidbody>();
                if(box != null)
                {
                    
                    box.isKinematic = true;
                }

                MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
                if(renderer != null)
                {
                    renderer.material.color = Color.blue;
                }

                Destroy(this);
            }
        }
    }
}
