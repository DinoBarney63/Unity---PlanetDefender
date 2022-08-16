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
    public GameObject orbit;
    public float distanceFromOrbit = 25.0f;

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
        }
    }

    public void SetUp(float orbitDistance, int gunCount)
    {
        spreadRad = Mathf.PI * 2 / gunCount;
        spreadDeg = 360 / gunCount;
        for (int i = 0; i < gunCount; i++)
        {
            gunAngleRad += spreadRad;
            gunAngleDeg += spreadDeg;
            GameObject newGun = Instantiate(enemyGunPrefab);
            newGun.transform.parent = transform;
            newGun.GetComponent<EnemyGun>().Spawned(gunAngleRad, gunAngleDeg, transform.position);
        }
        transform.position = orbit.transform.position + new Vector3(0, orbitDistance + Random.Range(-25, 25), 0);
    }
}
