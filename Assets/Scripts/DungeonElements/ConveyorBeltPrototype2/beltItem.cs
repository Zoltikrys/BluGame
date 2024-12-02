using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beltItem : MonoBehaviour
{
    public GameObject item;

    private void Awake() 
    {
        item = gameObject;
    }
}
