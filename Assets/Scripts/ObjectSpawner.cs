using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> PrefabsToSpawn = new List<GameObject>();
    public GameObject objectContainer;
    public float spawnEveryXSeconds = 1.0f;
    public float spawnOffsetTime = 0f;
    public bool canStart = false;
    public bool hasStarted = false;

    void Start()
    {
        if (canStart)
        {
            InvokeRepeating(nameof(SpawnObject), 0.0f, spawnEveryXSeconds);
            hasStarted = true;
        }
    }

    private void Update()
    {
        if (spawnOffsetTime > 0f)
        {
            spawnOffsetTime = spawnOffsetTime - Time.deltaTime;
        }

        if (spawnOffsetTime <= 0f && !hasStarted)
        {
            canStart = true;
            Start();
        }
    }


    private void SpawnObject(){
        if(PrefabsToSpawn.Count > 0){
            int index = UnityEngine.Random.Range(0, PrefabsToSpawn.Count);
            var a = Instantiate(PrefabsToSpawn[index], objectContainer.transform);
            a.transform.position = this.transform.position;
        }
    }
}
