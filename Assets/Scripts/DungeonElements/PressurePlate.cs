using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public BoxCollider BoxCollider;
    [SerializeField] private Animator anim;
    private bool pressed = false;

    public void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Release");
    }

    public void Press(Collider other){
        if(other == null) return;
        Debug.Log($"Pressed {other.name}");
        if(!pressed){
            anim.Play("Press");
            MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
            if (renderer != null) {
                renderer.material.color = Color.blue;
            }
            pressed = true;
        }
    }
        

    public void Release(Collider other){
        if(other == null) return;
        Debug.Log($"release {other.name}");
        if(pressed){
            anim.Play("Release");

            MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
            if (renderer != null) {
                renderer.material.color = Color.red;
            }
            pressed = false;
        }
        
    }

}
