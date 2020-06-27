using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawn_period_max = 1.25f;
    public float spawn_period_min = 0.75f;
    public Transform enemy_prefab;

    private int num_children;

    // Start is called before the first frame update
    void Start()
    {
        num_children = transform.childCount;
        StartCoroutine(SpawnManager());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnManager()
    {
        SpawnEnemy(Random.Range(0, num_children));
        float wait = Random.Range(spawn_period_min, spawn_period_max);
        yield return new WaitForSeconds(wait);
        StartCoroutine(SpawnManager());
    }

    private void SpawnEnemy(int column_number)
    {
        Transform column = transform.GetChild(column_number);
        Instantiate(enemy_prefab, column);
    }
}
