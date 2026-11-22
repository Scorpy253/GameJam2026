using UnityEngine;
using System.Collections.Generic;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 10f;
    private float baseMulti = 1f;
    private float TotalTime;
    private float timer;
    void Start()
    { 

    }
    void Update()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;
        timer += Time.deltaTime;
        TotalTime += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
        if(TotalTime%120 == 0)
        {
            spawnInterval -= 0.5f;
        }
    }
    void SpawnEnemy()
    {
        int enemyType = Random.Range(0, 2);
        if(TotalTime < 60)
        {
            enemyType = 0;
        }
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject obj = Instantiate(enemyPrefab[enemyType], point.position, Quaternion.identity);
        obj.GetComponent<EnemyAI>().spdMulti = baseMulti * Mathf.Pow(1.10f, TotalTime / 60f);
        obj.GetComponent<EnemyAI>().dmgMulti = baseMulti * Mathf.Pow(1.10f, TotalTime / 60f);
        obj.GetComponent<EnemyAI>().hpMulti = baseMulti * Mathf.Pow(1.10f, TotalTime / 60f);
        obj.GetComponent<EnemyAI>().EnemyElement = "working on it";
    }
}