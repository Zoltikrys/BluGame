using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossShield : MonoBehaviour
{
    [SerializeField]
    public int powerSourceFlags = 0;
    [SerializeField]
    public int powerSourceTarget;

    public void OpenSequence()
    {
        Debug.Log("omg its openinggg");
    }
}
