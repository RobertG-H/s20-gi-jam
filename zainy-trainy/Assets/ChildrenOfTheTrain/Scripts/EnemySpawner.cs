using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawn_period_max = 1.25f;
    public float spawn_period_min = 0.75f;
    public Transform enemy_prefab;
    public CottManager manager;

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
        manager.DecreaseSpawns();
        SpawnEnemy(Random.Range(0, num_children));
        float wait = Random.Range(spawn_period_min, spawn_period_max);
        yield return new WaitForSeconds(wait);

        if (manager.SpawnsRemaining())
            StartCoroutine(SpawnManager());
    }

    private void SpawnEnemy(int column_number)
    {
        Transform column = transform.GetChild(column_number);
        EnemyController enemy = Instantiate(enemy_prefab, column).gameObject.GetComponent<EnemyController>();
        enemy.manager = manager;
    }
}
