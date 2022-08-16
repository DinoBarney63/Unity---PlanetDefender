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
        if(spawn)
        {
            spawn = false;
            SpawnEnemy();
        }   
    }

    public void SpawnEnemy()
    {
        GameObject NewEnemyOrbit = Instantiate(enemyInOrbitPrefab);
        float xOffset = Random.Range(-50, 50);
        float yOffset = Random.Range(-50, 50);
        Vector3 offset = new Vector3(xOffset, yOffset, 0);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(xOffset, 2) + Mathf.Pow(yOffset, 2));
        NewEnemyOrbit.transform.position = player.transform.position + offset;
        NewEnemyOrbit.GetComponentInChildren<Enemy>().SetUp(distanceToPlayer, 3);
    }
}
