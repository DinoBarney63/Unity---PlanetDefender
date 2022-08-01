using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject player;
    public float distanceToPlayer;
    private float rotationSpeed = 0.004f;
    public bool between = true;
    public float point1 = 0;
    public float point2 = 360;
    public float toRotate;
    public float shootOffset = 5;
    public GameObject enemyBulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayerx = player.transform.position.x - transform.position.x;
        float distanceToPlayery = player.transform.position.y - transform.position.y;
        distanceToPlayer = Mathf.Sqrt(Mathf.Pow(distanceToPlayerx, 2f) + Mathf.Pow(distanceToPlayery, 2f));

        if (distanceToPlayer <= 10)
        {
            Vector3 playerDirection = transform.position - player.transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
            toRotate = desiredRotation.eulerAngles.z;
            if ((toRotate >= point1 && toRotate <= point2) == between)
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

            if (transform.rotation.eulerAngles.z == toRotate)
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        GameObject newBullet = Instantiate(enemyBulletPrefab);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.localRotation;
    }
}
