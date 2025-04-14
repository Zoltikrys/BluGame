using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class CollectibleCollection : MonoBehaviour
{
    private int Nuts = 0;

    public TextMeshProUGUI collectibleText;

    public void Start()
    {
        collectibleText =  GameObject.Find("SparePartsCounter").GetComponent<TextMeshProUGUI>();
    }


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

    public bool TrySpendParts(int amount)
    {
        if (Nuts >= amount)
        {
            Nuts -= amount;
            collectibleText.text = "Spare Parts: " + Nuts.ToString();
            return true;
        }
        return false;
    }

}