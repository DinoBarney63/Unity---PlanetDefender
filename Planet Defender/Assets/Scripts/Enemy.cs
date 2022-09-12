using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    public int health = 25;
    public GameObject enemyGunPrefab;
    private int guns;
    private float spreadRad = 0;
    private float spreadDeg = 0;
    private float gunAngleRad = 0;
    private float gunAngleDeg = 90;
    public GameObject orbit;
    public GameObject partsPrefab;

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
            SpawnParts(guns);
            Destroy(orbit);
            int score = Mathf.RoundToInt(guns * 1.5f);
            gameManager.GetComponent<GameManager>().UpdatePlayerScore(score, guns);

        }
    }

    public void SetUp(float orbitDistance, int gunCount)
    {
        // Spawns in sub-guns based on gunCount
        spreadRad = Mathf.PI * 2 / gunCount;
        spreadDeg = 360 / gunCount;
        guns = gunCount;
        for (int i = 0; i < gunCount; i++)
        {
            gunAngleRad += spreadRad;
            gunAngleDeg += spreadDeg;
            GameObject newGun = Instantiate(enemyGunPrefab);
            newGun.transform.parent = transform;
            newGun.GetComponent<EnemyGun>().Spawned(gunAngleRad, gunAngleDeg, transform.position);
            newGun.GetComponent<EnemyGun>().main = false;
        }
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
            if (spawning >= 5)
            {
                SpawnLevelPart(5);
                spawning -= 5;
            }
            else if (spawning >= 3)
            {
                SpawnLevelPart(3);
                spawning -= 3;
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
        newPart.GetComponent<Parts>().AddValue(level);
        float positionx = transform.position.x + (Random.Range(-20, 20) / 10);
        float positiony = transform.position.y + (Random.Range(-20, 20) / 10);
        newPart.transform.position = new Vector3(positionx, positiony, 0);
    }
}
