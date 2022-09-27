using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : MonoBehaviour
{
    private GameManager gameManager;
    public int health = 15;
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
            SpawnLevelPart();
            Destroy(orbit);
            gameManager.GetComponent<GameManager>().UpdatePlayerScore(3);
        }
    }

    public void SetUp(float orbitDistance)
    {
        transform.position = orbit.transform.position + new Vector3(0, orbitDistance, 0);
    }

    public void SpeedUp(float speeding)
    {
        orbit.GetComponent<Orbit>().SpeedUp(speeding);
    }

    private void SpawnLevelPart()
    {
        GameObject newPart = Instantiate(partsPrefab);
        newPart.GetComponent<Parts>().AddValue(5, true);
        float positionx = transform.position.x + (Random.Range(-20, 20 + 1) / 10);
        float positiony = transform.position.y + (Random.Range(-20, 20 + 1) / 10);
        newPart.transform.position = new Vector3(positionx, positiony, 0);
    }
}
