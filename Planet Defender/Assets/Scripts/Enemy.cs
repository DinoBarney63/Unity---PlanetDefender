using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    public int powerPercentage;
    public int health = 50;
    public GameObject enemyGunPrefab;
    public int gunCount = 3;
    public float gunAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int difficulty = gameManager.difficultyWhat();


        GameObject newGun = Instantiate(enemyGunPrefab);
        
        for(int x = 0; x >= gunCount; x++)
        {
            float spread = 360 / gunCount;
            newGun.GetComponent<EnemyGun>().Spawned(gunAngle);
            // Set position
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
