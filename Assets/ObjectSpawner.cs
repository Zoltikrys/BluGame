using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> PrefabsToSpawn = new List<GameObject>();
    public GameObject objectContainer;
    public float spawnEveryXSeconds = 1.0f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 0.0f, spawnEveryXSeconds);
    }

    private void SpawnObject(){
        if(PrefabsToSpawn.Count > 0){
            int index = UnityEngine.Random.Range(0, PrefabsToSpawn.Count);
            var a = Instantiate(PrefabsToSpawn[index], objectContainer.transform);
            a.transform.position = this.transform.position;
        }
    }
}
