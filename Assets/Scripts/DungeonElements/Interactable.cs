using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent<Collider> OnEnterEvents = new UnityEvent<Collider>();
    public UnityEvent<Collider> OnExitEvents = new UnityEvent<Collider>();
    public UnityEvent<Collider> OnStayEvents = new UnityEvent<Collider>();
    public UnityEvent OnFinishEvents = new UnityEvent();
    public GameObject RequiredGameObjectToTrigger;
    private Collider cachedRequiredCollider;
    private bool isActive = true;

    private void Awake() {
        if (RequiredGameObjectToTrigger != null) {
            cachedRequiredCollider = RequiredGameObjectToTrigger.GetComponent<Collider>();
        }
        ForceEventCompilation();
    }

    private void ForceEventCompilation()
    {
        OnEnterEvents?.Invoke(null);
        OnStayEvents?.Invoke(null);
        OnExitEvents?.Invoke(null);
    }

    private void OnTriggerEnter(Collider collider){
        if(IsCorrectCollider(collider) && isActive) OnEnterEvents?.Invoke(collider);
    }

    private void OnTriggerExit(Collider collider){
        if(IsCorrectCollider(collider) && isActive) OnExitEvents?.Invoke(collider);
    }

    private void OnTriggerStay(Collider collider){
        if(IsCorrectCollider(collider) && isActive) OnStayEvents?.Invoke(collider);
    }

    public void OnFinish(){
        isActive = false;
        OnFinishEvents?.Invoke();
    }

    private bool IsCorrectCollider(Collider collider){
        return cachedRequiredCollider == null || cachedRequiredCollider == collider;
    }


    public void DebugExitEvent(string name){
        Debug.Log($"{name} Exit Event fired");
    }
    public void DebugEnterEvent(string name){
        Debug.Log($"{name} Enter Event fired");
    }
    public void DebugStayEvent(string name){
        Debug.Log($"{name} Stay Event fired");
    }
}
