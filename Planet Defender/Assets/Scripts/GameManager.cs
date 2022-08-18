using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public int difficultyPercentage = 25;
    public GameObject enemyInOrbitPrefab;
    public int enemyCount;
    public bool spawn = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn || enemyCount < 5)
        {
            spawn = false;
            SpawnEnemy();
        }

        enemyCount = FindObjectsOfType<Enemy>().Length;
    }

    public void SpawnEnemy()
    {
        GameObject NewEnemyOrbit = Instantiate(enemyInOrbitPrefab);
        Vector3 offset = randomSpawnPos(30, 50);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(offset.x, 2) + Mathf.Pow(offset.y, 2));
        NewEnemyOrbit.transform.position = player.transform.position + offset;
        NewEnemyOrbit.GetComponentInChildren<Enemy>().SetUp(distanceToPlayer, 3);
    }

    public Vector3 randomSpawnPos(int low, int high)
    {
        int numberA = Random.Range(low, high);
        if (Random.value > 0.5f)
            numberA *= -1;
        int numberB = Random.Range(-high, high);
        int x;
        int y;

        if(Random.value > 0.5f)
        {
            x = numberA;
            y = numberB;
        }else
        {
            x = numberB;
            y = numberA;
        }

        Vector3 position = new (x, y, 0);
        return position;
    }
}
