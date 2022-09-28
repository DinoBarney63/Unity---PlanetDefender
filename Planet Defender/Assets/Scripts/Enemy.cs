using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    public int health = 25;
    public GameObject enemyGunPrefab;
    private int guns;
    private float spreadDeg = 0;
    private float gunAngleDeg = 90;
    public GameObject orbit;
    public GameObject partsPrefab;
    public int enemyPower = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void Damage(int damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            SpawnParts(Mathf.RoundToInt(guns * gameManager.GetComponent<GameManager>().difficultyToLeveling * 50));
            Destroy(orbit);
            int score = Mathf.RoundToInt(guns * 1.5f);
            gameManager.GetComponent<GameManager>().UpdatePlayerScore(score);

        }
    }

    public void SetUp(float orbitDistance, int gunsCount)
    {
        guns = gunsCount;
        int gunCount = gunsCount;
        while (gunCount > 11)
        {
            enemyPower += 1;
            gunCount -= 8;
        }
        health = (enemyPower * 10) + 5;
        // Spawns in sub-guns based on gunCount
        spreadDeg = 360 / gunCount;
        for (int i = 0; i < gunCount; i++)
        {
            gunAngleDeg += spreadDeg;
            GameObject newGun = Instantiate(enemyGunPrefab);
            newGun.transform.parent = transform;
            newGun.GetComponent<EnemyGun>().Spawned(gunAngleDeg, transform.position);
            newGun.GetComponent<EnemyGun>().main = false;
            newGun.GetComponent<EnemyGun>().enemyPower = enemyPower;
        }
        float size = (enemyPower * 0.1f) + 0.8f;
        transform.localScale = new Vector3(size, size, size);
        transform.position = orbit.transform.position + new Vector3(0, orbitDistance, 0);
    }

    public void SpeedUp(float speeding)
    {
        orbit.GetComponent<Orbit>().SpeedUp(speeding);
    }

    private void SpawnParts(int points)
    {
        bool loop = true;
        int spawning = points;
        while (loop)
        {
            if (spawning > 63)
            {
                SpawnLevelPart(spawning);
                spawning = 0;
            }
            else if (spawning >= 29)
            {
                SpawnLevelPart(21);
                spawning -= 21;
            }
            else if (spawning >= 13)
            {
                SpawnLevelPart(13);
                spawning -= 13;
            }
            else if (spawning >= 5)
            {
                SpawnLevelPart(5);
                spawning -= 5;
            }
            else if (spawning >= 1)
            {
                SpawnLevelPart(1);
                spawning -= 1;
            }
            else if (spawning <= 0)
            {
                loop = false;
            }
        }
    }

    private void SpawnLevelPart(int level)
    {
        GameObject newPart = Instantiate(partsPrefab);
        newPart.GetComponent<Parts>().AddValue(level, false);
        float positionx = transform.position.x + (Random.Range(-20, 20 + 1) / 10);
        float positiony = transform.position.y + (Random.Range(-20, 20 + 1) / 10);
        newPart.transform.position = new Vector3(positionx, positiony, 0);
    }
}
