using UnityEngine;
using UnityEngine.AI;


public class LaserInput : MonoBehaviour{
    public bool active = true;
    public float activeTime = 1.0f;
    public float lastTimeChecked = 0.0f;

    void Start(){
        lastTimeChecked = Time.deltaTime;
        InvokeRepeating(nameof(RefreshInput), 0.0f, 0.2f);
    }


    private void RefreshInput(){
        if(active){
            if(lastTimeChecked - Time.deltaTime < 1.0f){}
        }
    }

    private void Activated(){
        active = true;
    }

}