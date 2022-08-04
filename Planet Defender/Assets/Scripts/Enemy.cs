using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    public int powerPercentage;
    public int health = 50;
    public List<GameObject> guns;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int difficulty = gameManager.difficultyWhat();
        foreach(GameObject gun in guns)
        {
            int num = Random.Range(1, 100);
            if (num <= difficulty)
                gun.SetActive(true);
            else
                gun.SetActive(false);
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
