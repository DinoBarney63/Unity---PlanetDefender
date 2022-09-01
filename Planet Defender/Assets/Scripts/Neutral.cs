using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutral : MonoBehaviour
{
    private GameManager gameManager;
    public int health = 15;
    public GameObject orbit;

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
            Destroy(orbit);
            gameManager.GetComponent<GameManager>().UpdatePlayerScore(2);
        }
    }

    public void SetUp(float orbitDistance)
    {
        transform.position = orbit.transform.position + new Vector3(0, orbitDistance + Random.Range(-20, 20), 0);
    }
}
