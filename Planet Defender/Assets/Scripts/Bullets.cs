using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Rigidbody bulletRb;
    private float bulletSpeed = 25.0f;
    private GameObject player;
    private float age;
    public int damage = 1;
    public bool playerBullet;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        if (playerBullet)
            bulletSpeed = player.GetComponent<Player>().rangeLevel * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Moves forward until far distanced from the player
        bulletRb.AddForce(force: -1 * bulletSpeed * transform.up);
        if (Mathf.Abs(transform.position.x) > player.transform.position.x + 60 + (player.GetComponent<Player>().rangeLevel * 5))
        {
            Destroy(gameObject);
        }else if (Mathf.Abs(transform.position.y) > player.transform.position.y + 60 + (player.GetComponent<Player>().rangeLevel * 5))
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
            // When collides with a player, a enemy or a neutral it deals damage to them
            if (other.CompareTag("Player"))
            {
                player.GetComponent<Player>().Damage(damage);
            }else if (other.CompareTag("Enemy"))
            {
                GameObject enemy = other.gameObject;
                enemy.GetComponent<Enemy>().Damage(damage);
            } else if (other.CompareTag("Neutral"))
            {
                GameObject neutral = other.gameObject;
                neutral.GetComponent<Neutral>().Damage(damage);
            }

            // If the bullet collides with a bullet shot by the same charcter nothing happens
            if (other.CompareTag("Bullet"))
            {
                if(playerBullet != other.GetComponent<Bullets>().playerBullet)
                    Destroy(gameObject);
            }
            else
            {
                if (!other.CompareTag("Part"))
                    Destroy(gameObject);
            }
        }
    }
}
