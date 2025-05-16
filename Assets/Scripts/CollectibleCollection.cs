using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine;

public class CollectibleCollection : MonoBehaviour
{
    public int Nuts = 0;

    public TextMeshProUGUI collectibleText;

    public void Start()
    {
        collectibleText =  GameObject.Find("SparePartsCounter").GetComponent<TextMeshProUGUI>();
        if(collectibleText) collectibleText.text = Nuts.ToString();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Collectible")
        {
            AddNut(1);
            Debug.Log(Nuts);
            Destroy(other.gameObject);
        }
    }

    public void AddNut(int amount){
        Nuts += amount;
        if(collectibleText) collectibleText.text = Nuts.ToString();
        Debug.Log(Nuts);
    }

    public void SetNut(int amount){
        Nuts = amount;
        if(collectibleText) collectibleText.text = Nuts.ToString();
        Debug.Log(Nuts);
    }

    public bool TrySpendParts(int amount)
    {
        if (Nuts >= amount)
        {
            Nuts -= amount;
            collectibleText.text = Nuts.ToString();
            return true;
        }
        return false;
    }

}