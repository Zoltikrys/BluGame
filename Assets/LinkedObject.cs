using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Used to link two or more objects together. An example of this would be the electrical pylons.
/// </summary>
public class LinkedObject : MonoBehaviour
{
    [field: SerializeField] public LINKED_OBJECT_TYPE Type {get; set;}
    [field: SerializeField] public LINKED_OBJECT_AMOUNT Amount{ get; set;}
    [field: SerializeField] public LINKED_OBJECT_BLOCK_TYPE BlockType{ get; set;}
    [field: SerializeField] public bool IsDamaging {get; set;}
    [field: SerializeField] public float Radius {get; set;} = 1.0f;
    [field: SerializeField] public bool Active {get; set;} 
    [field: SerializeField] public bool Linked {get; set;}
    [field: SerializeField] private HashSet<LinkedObject> CurrentlyLinked = new HashSet<LinkedObject>(); 
    [field: SerializeField] private HashSet<LinkedObject> InRangeObjects = new HashSet<LinkedObject>();
    [field: SerializeField] private HashSet<LinkedObject> LinkedObjectsToCull = new HashSet<LinkedObject>();
    [field: SerializeField] private Dictionary<LinkedObject, GameObject> LineRenderers = new Dictionary<LinkedObject, GameObject>();
    [field: SerializeField] public GameObject ParticleSystem {get; set;}
    private bool blocked = false;
    public Ray LinkDirection {get; set;}

    void Start()
    {
        InvokeRepeating(nameof(AttemptLinkObjects), 0, 0.2f);
        if(IsDamaging) InvokeRepeating(nameof(ProcessDamage), 0, 0.2f);
    }

    private void ProcessDamage()
    {
        foreach(LinkedObject linkedObject in CurrentlyLinked){
            HealthManager health;
            linkedObject.TryGetComponent<HealthManager>(out health);
            
            if(health != null) health.Damage();
        }
    }

    public void AttemptLinkObjects(){
        InRangeObjects.Clear();
        UpdateParticles();

        if(!Active) return;

        // Find all objects in range, Clear any out of range from currently linked if no longer linked
        RetrieveLinkedObjectsInRange();
        CullLinkedObjects();


        if(CurrentlyLinked.Count >= 1 && Amount == LINKED_OBJECT_AMOUNT.SINGULAR) return;  // if connected and only allowed one linked object, continue 
        foreach(LinkedObject linkedObject in InRangeObjects){
            if(linkedObject.Amount == LINKED_OBJECT_AMOUNT.SINGULAR && linkedObject.Linked) continue; // if other object ^^^^
            if(!(linkedObject.Type == this.Type)) continue;  // skip different type of object
            if(IsBlocked(linkedObject)) continue;
            if(CurrentlyLinked.Count >= 1 && Amount == LINKED_OBJECT_AMOUNT.SINGULAR) break;  // if connected and only allowed one linked object, exit early because we've found a linked object
            
            Block(linkedObject);
            Link(linkedObject);
            Unblock(linkedObject);
        }
        
    }

    private void CullLinkedObjects()
    {
        LinkedObjectsToCull.Clear();
        CurrentlyLinked.RemoveWhere(obj => !InRangeObjects.Contains(obj));
        if(CurrentlyLinked.Count == 0) Linked = false;

        if(BlockType == LINKED_OBJECT_BLOCK_TYPE.NONE) return; // dont remove blocked objects

        // cull blocked objects
        foreach(LinkedObject linkedObject in CurrentlyLinked){
            SetLinkDirection(linkedObject);
            var hits = GetSortedRaycastHits(LinkDirection, Radius);
            
            // This could use a rewrite -- I think we can get away with just checking [0, 1, 2] in the raycast hits
            bool targetHit = false;
            foreach(var hit in hits){
                // dont block on self or target
                if(hit.transform.gameObject == this) continue;
                if(hit.transform.gameObject == linkedObject.transform.gameObject) {
                    targetHit = true;
                    break;
                }

                if(!targetHit){
                    LinkedObjectsToCull.Add(linkedObject);
                    break;
                }
            }
        }

        CurrentlyLinked.RemoveWhere(obj => LinkedObjectsToCull.Contains(obj));
    }

    private Vector3 DirectionToObject(Transform src, Transform dest){
        return (dest.position - src.position).normalized;
    }
    
    private void SetLinkDirection(LinkedObject linkedObject){
        LinkDirection = new Ray(this.transform.position, DirectionToObject(this.transform, linkedObject.transform));
    }

    private RaycastHit[] GetSortedRaycastHits(Ray direction, float distance){
        RaycastHit[] hitColliders = Physics.RaycastAll(direction, distance);
        System.Array.Sort(hitColliders, (a, b) => a.distance.CompareTo(b.distance));

        return hitColliders;

    }

    private void UpdateParticles()
    {
        ParticleSystem.gameObject.SetActive(Linked);
        
        foreach(LinkedObject linkedObject in CurrentlyLinked){
            ParticleSystem.transform.LookAt(linkedObject.transform);
            linkedObject.ParticleSystem.GetComponent<Lightning>().SetDistances(this.transform, linkedObject.transform);
        }
    }

    private void RetrieveLinkedObjectsInRange(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Radius, ~0);  // check everything  -- consider a mask here
        foreach (Collider collider in hitColliders)
        {
            if(collider.gameObject == this.gameObject) continue;  // skip this object in the collision detection;

            LinkedObject linkedObject;
            collider.gameObject.TryGetComponent<LinkedObject>(out linkedObject);
            if(!linkedObject) continue;   //missing linked object component
            if(!linkedObject.Active) continue; // skip inactive linkedobjects
            InRangeObjects.Add(linkedObject);
        }
    }

    private bool IsBlocked(LinkedObject linkedObject)
    {
        return linkedObject.blocked || this.blocked;
    }

    public void Link(LinkedObject objectToLink){

        if(CurrentlyLinked.Contains(objectToLink)) return; // dont duplicate

        Debug.Log($"Linked {this.name} to {objectToLink.name}");
        CurrentlyLinked.Add(objectToLink);
        Linked = true;
        objectToLink.Link(this);
        ParticleSystem.transform.LookAt(objectToLink.transform);
    }

    public void Block(LinkedObject linkedObject){
        this.blocked = true;
        linkedObject.blocked = true;
    }

    private void Unblock(LinkedObject linkedObject){
        this.blocked = false;
        linkedObject.blocked = false;
    }
}