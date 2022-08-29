using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Rigidbody bulletRb;
    private float bulletSpeed = 10.0f;
    private GameObject player;
    private float age;
    public int damage = 1;
    public bool playerBullet;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Moves forward until far distanced from the player
        bulletRb.AddForce(transform.up * -1 * bulletSpeed);
        if (Mathf.Abs(transform.position.x) > player.transform.position.x + 60)
        {
            Destroy(gameObject);
        }else if (Mathf.Abs(transform.position.y) > player.transform.position.y + 60)
        {
            Destroy(gameObject);
        }

        age += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Waits a small delay so it dosent collide with it's inital shooter
        if (age > 0.05)
        {
            // When collides with a player or a enemy it deals damage
            if (other.tag == "Player")
            {
                player.GetComponent<Player>().Damage(damage);
            }else if (other.tag == "Enemy")
            {
                GameObject enemy = other.gameObject;
                enemy.GetComponent<Enemy>().Damage(damage);
            }

            // If the bullet collides with a bullet shot by the same charcter nothing happens
            if (other.tag == "Bullet")
            {
                if(playerBullet == other.GetComponent<Bullets>().playerBullet)
                {

                }else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
