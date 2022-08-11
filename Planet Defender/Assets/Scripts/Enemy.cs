using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    public int health = 50;
    public GameObject enemyGunPrefab;
    public int gunCount = 3;
    private float spreadRad = 0;
    private float spreadDeg = 0;
    private float gunAngleRad = 0;
    private float gunAngleDeg = 90;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //int difficulty = gameManager.difficultyWhat();


        
        spreadRad = Mathf.PI * 2 / gunCount;
        spreadDeg = 360 / gunCount;
        for(int i = 0; i < gunCount; i++)
        {
            gunAngleRad += spreadRad;
            gunAngleDeg += spreadDeg;
            GameObject newGun = Instantiate(enemyGunPrefab);
            newGun.transform.parent = transform;
            newGun.GetComponent<EnemyGun>().Spawned(gunAngleRad, gunAngleDeg, transform.position);
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
