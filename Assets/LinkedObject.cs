using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to link two or more objects together. An example of this would be the electrical pylons.
/// </summary>
public class LinkedObject : MonoBehaviour
{
    [field: SerializeField] public LINKED_OBJECT_TYPE Type {get; set;}
    [field: SerializeField] public LINKED_OBJECT_AMOUNT Amount{ get; set;}
    [field: SerializeField] public float Radius {get; set;} = 1.0f;
    [field: SerializeField] public bool Active {get; set;} 
    [field: SerializeField] public bool Linked {get; set;}
    [field: SerializeField] public HashSet<LinkedObject> CurrentlyLinked = new HashSet<LinkedObject>(); 
    [field: SerializeField] public HashSet<LinkedObject> InRangeObjects = new HashSet<LinkedObject>();
    [field: SerializeField] public GameObject ParticleSystem {get; set;}
    private bool blocked = false;

    void Start()
    {
        InvokeRepeating(nameof(AttemptLinkObjects), 0, 0.2f);
    }

    public void AttemptLinkObjects(){
        InRangeObjects.Clear();
        UpdateParticles();

        if(!Active) return;

        // Find all objects in range, Clear any out of range from currently linked
        RetrieveLinkedObjectsInRange();
        CurrentlyLinked.RemoveWhere(obj => !InRangeObjects.Contains(obj));
        if(CurrentlyLinked.Count == 0) Linked = false;


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

    private void UpdateParticles()
    {
        ParticleSystem.gameObject.SetActive(Linked);
        
        foreach(LinkedObject linkedObject in CurrentlyLinked){
            ParticleSystem.transform.LookAt(linkedObject.transform);
            linkedObject.ParticleSystem.GetComponent<Lightning>().SetDistances(this.transform, linkedObject.transform);
        }
    }

    private void RetrieveLinkedObjectsInRange(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Radius, ~0);  // check everything
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