using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ToggleSteam : MonoBehaviour
{
    public VisualEffect steamEffect;
    public bool canIntereact = false;
    public bool isPlaying = false;
    //public GameObject Valve;

    // Start is called before the first frame update
    void Start()
    {
        steamEffect.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        canIntereact = true;

        /*if (Input.GetKeyDown(KeyCode.I))
        {
            steamEffect.Play();
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        //steamEffect.Stop();
        canIntereact = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (canIntereact)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isPlaying)
                {
                    isPlaying = false;
                    steamEffect.Stop();
                    //Valve.transform.Rotate(new Vector3(0, 0, 1f));
                    //animate the valve turning but i dont wanna do that rn
                }

                else if (!isPlaying)
                {
                    isPlaying = true;
                    steamEffect.Play();
                }
            }
        }        
    }
}
