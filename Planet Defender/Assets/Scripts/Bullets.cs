using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Rigidbody bulletRb;
    private float bulletSpeed = 10.0f;
    private GameObject player;
    public float age;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        bulletRb.AddForce(transform.up * -1 * bulletSpeed);
        if (Mathf.Abs(transform.position.x) > player.transform.position.x + 40)
        {
            Destroy(gameObject);
        }else if (Mathf.Abs(transform.position.y) > player.transform.position.y + 40)
        {
            Destroy(gameObject);
        }

        age += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (age > 0.05)
        {
            if (other.tag == "Player")
            {
                player.GetComponent<Player>().Damage(1);
            }else if (other.tag == "Enemy")
            {
                GameObject enemy = other.gameObject;
                enemy.GetComponent<Enemy>().Damage(1);
            }
            Destroy(gameObject);
        }
    }
}
