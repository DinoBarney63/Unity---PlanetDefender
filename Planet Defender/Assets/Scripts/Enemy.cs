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
    public int type = 0;
    public List<GameObject> types;
    public List<int> healthMultiplier;
    public List<int> damageMultiplier;
    public List<int> rangeMultiplier;
    public List<Color> gunColours;

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
        // Decides enemy power based on excess guns that are over the limit
        guns = gunsCount;
        int gunCount = gunsCount;
        while (gunCount > 11)
        {
            enemyPower += 1;
            gunCount -= 8;
        }

        // initiates multipliers (0 means no change)
        // Decides enemy type  (33% chance for special type)
        if (Random.value > 0.6f)
        {
            type = Random.Range(1, 4); // 1 to 3 (4 is excluded in random.range)
            
        }

        health = (enemyPower * 10) + 5 + (healthMultiplier[type] * 5);
        int enemyDamage = 1 + damageMultiplier[type];
        float enemyRange = 15 + (rangeMultiplier[type] * 5);

        types[type].SetActive(true);

        // Sets the main gun
        GetComponentInChildren<EnemyGun>().enemyPower = enemyPower;
        GetComponentInChildren<EnemyGun>().damage = enemyDamage;
        GetComponentInChildren<EnemyGun>().shootRange = enemyRange;

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
            newGun.GetComponent<EnemyGun>().damage = enemyDamage;
            newGun.GetComponent<EnemyGun>().shootRange = enemyRange;
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
            if (spawning > 500)
            {
                SpawnLevelPart(spawning);
                spawning = 0;
            }
            else if (spawning > 0)
            {
                if (spawning >= 50)
                {
                    SpawnLevelPart(50);
                    spawning -= 50;
                }else
                {
                    SpawnLevelPart(spawning);
                    spawning = 0;
                }
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
