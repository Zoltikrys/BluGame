using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnDelay = 3f;

    [SerializeField] private float nextSpawnTime;

    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        nextSpawnTime += Time.deltaTime;

        if(nextSpawnTime > spawnDelay)
        {
            anim.Play("spawnerOpen", 0, 0.0f);
            //SpawnEnemy();
            nextSpawnTime = 0f;
            //anim.Play("spawnerClose", 0, 0.0f);
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        if(spawnPoints.Length != 0) {
            Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        }
    }

    void CloseSpawner()
    {
        anim.Play("spawnerClose");
    }
}
