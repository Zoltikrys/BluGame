using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RgbFilterObject : MonoBehaviour
{
    [field: SerializeField]
    public RGBSTATE FilterLayer{get; set;}

    [field: SerializeField]
    public bool CollisionWhenHidden = false;

    [field: SerializeField]
    public bool DestroyAfterRevealed = false;

    [field: SerializeField]
    public int TimeToDestroy = 0;


    public void Hide(){
        if(CollisionWhenHidden){
            MeshRenderer meshRenderer = transform.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
        }
        else transform.gameObject.SetActive(false);

    }

    public void Show(){
        if(CollisionWhenHidden){
            MeshRenderer meshRenderer = transform.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
        }else transform.gameObject.SetActive(true);
    }

}
