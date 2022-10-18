using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : MonoBehaviour
{
    private GameManager gameManager;
    public int health = 15;
    public GameObject orbit;
    public GameObject partsPrefab;
    private int neutralValue = 15;
    public List<GameObject> types;
    public int type = 0;
    public float speed;
    private int direction;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // Set the planets rotation speed and direction
        double speedOffset = Random.Range(-10, 10 + 1) * 0.1;
        speed = 0.5f + (float)speedOffset;
        if (Random.value > 0.5f)
            direction = -1;
        else
            direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Slowly rotates itself
        transform.Rotate(0, 0, speed * direction * Time.deltaTime);
    }

    public void Damage(int damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            // When the neutral is destroyed, it spawns parts, updates the score and deletes itself
            SpawnParts(neutralValue / 3);
            Destroy(orbit);
            gameManager.GetComponent<GameManager>().UpdatePlayerScore(3);
        }
    }

    public void SetUp(float orbitDistance, int points)
    {
        // Sets the orbiting distance, health, value, and type
        transform.position = orbit.transform.position + new Vector3(0, orbitDistance, 0);
        health = points;
        neutralValue = points;
        type = Random.Range(0, types.Count);
        types[type].SetActive(true);
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
            // If the amount to spawn is above 500 it is put into one part to reduce lag in the later stages of the game
            // Otherwise smaller parts are spawned with their maxes at 50
            // Once all parts have been spawned the loop is ended
            if (spawning > 500)
            {
                SpawnLevelPart(spawning);
                spawning = 0;
            }
            else if (spawning > 0)
            {
                if (spawning >= 25)
                {
                    SpawnLevelPart(25);
                    spawning -= 25;
                }
                else
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
        newPart.GetComponent<Parts>().AddValue(level, true);
        float positionx = transform.position.x + (Random.Range(-20, 20 + 1) / 10);
        float positiony = transform.position.y + (Random.Range(-20, 20 + 1) / 10);
        newPart.transform.position = new Vector3(positionx, positiony, 0);
    }
}
