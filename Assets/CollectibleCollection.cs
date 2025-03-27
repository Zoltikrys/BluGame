using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColectibleCollection : MonoBehaviour
{
    private int Nuts = 0;

    public TextMeshProUGUI collectibleText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Collectible")
        {
            Nuts++;
            collectibleText.text = "Spare Parts: " + Nuts.ToString();
            Debug.Log(Nuts);
            Destroy(other.gameObject);
        }
    }
}