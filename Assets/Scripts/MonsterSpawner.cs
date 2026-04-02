using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monster[] prefabs;

    public Transform[] spawnPoints;

    private List<Monster> monsters = new List<Monster>();

    private int wave = 0;

    private void Update()
    {
        if (monsters.Count == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        wave++;
        int count = Mathf.RoundToInt(wave * 1.5f);
        for (int i = 0; i < count; i++)
        {
            CreateZombie();
        }
    }

    private void CreateZombie()
    {
        var point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var monster = Instantiate(prefabs[Random.Range(0, prefabs.Length)], point.position, point.rotation);
        monsters.Add(monster);

        monster.onDead.AddListener(() => monsters.Remove(monster));
        monster.onDead.AddListener(() => Destroy(monster.gameObject, 5f));
    }
}
