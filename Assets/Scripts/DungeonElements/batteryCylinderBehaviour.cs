using UnityEngine;

public class batteryCylinderBehaviour : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject internalBatteryModel;

    [field: SerializeField] public bool IsActive {get; set;} = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(IsActive) anim.Play("Rise");
        else anim.Play("Sink");
    }

    public void RecieveBattery(Collider other){
        if(other == null) return;
        
        DungeonElement element;
        if(!other.gameObject.TryGetComponent<DungeonElement>(out element)) return; // if not a dungeon element return
        if(element.type != DungeonElementType.BATTERY) return;  // if not battery, return

        Debug.Log("Battery accepted");
        Destroy(other.gameObject);
        internalBatteryModel.SetActive(true);
        anim.StopPlayback();
        anim.Play("Sink");
        IsActive = false;

        Interactable interactable;
        TryGetComponent<Interactable>(out interactable);
        if(interactable) interactable.OnFinish();
    }

    public void Activate(){
        IsActive = true;
        anim.Play("Rise");
    }
}
