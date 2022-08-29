using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    private GameObject player;
    public float distanceToPlayer;
    private float rotationSpeed = 0.004f;
    private bool between = true;
    private float point1 = 0;
    private float point2 = 360;
    private float toRotate;
    public float shootRange = 20;
    public float shootOffset = 5;
    public GameObject bulletPrefab;
    public GameObject lazerPrefab;
    public float shootDelaySecondsMain = 2;
    public float shootDelaySecondsSub = 10;
    private float shootDelay;
    public bool main = true;

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

        // Checks if the player is within firing range. If so the the gun is pointed at the player and once pointed it fires
        if (distanceToPlayer <= shootRange)
        {
            Vector3 playerDirection = transform.position - player.transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
            toRotate = desiredRotation.eulerAngles.z;
            if ((toRotate >= point1 && toRotate <= point2) == between)
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

            if ((transform.rotation.eulerAngles.z + shootOffset > toRotate) && (transform.rotation.eulerAngles.z - shootOffset < toRotate) && shootDelay < 0)
            {
                // Main gun shoots lazers and sub-gun shoots bullets
                if (main)
                {
                    Shoot(lazerPrefab);
                    shootDelay = shootDelaySecondsMain;
                }else
                {
                    Shoot(bulletPrefab);
                    shootDelay = shootDelaySecondsSub;
                }
            }
        }
        shootDelay -= Time.deltaTime;
    }

    // Sub gun spawning
    public void Spawned(float startingAngleRad, float startingAngleDeg, Vector3 enemyPos)
    {
        // Sets position and rotation restrictions based on starting rotation
        transform.Rotate(0, 0, startingAngleDeg);
        transform.position = new Vector3 (enemyPos.x + 1.5f * Mathf.Cos(startingAngleRad), enemyPos.y + 1.5f * Mathf.Sin(startingAngleRad), 0);

        point1 = startingAngleDeg - 100;
        if (point1 < 0)
            point1 += 360;
        point2 = startingAngleDeg + 100;
        if (point2 > 360)
            point2 -= 360;

        if (point1 > point2)
        {
            float temp = point1;
            point1 = point2;
            point2 = temp;
            between = false;
        }
        
    }

    public void Shoot(GameObject projectile)
    {
        // Creates a new bullet and sets its rotation and position to the guns
        GameObject newBullet = Instantiate(projectile);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.localRotation;
        if(main)
            newBullet.GetComponent<Bullets>().damage = 5;
        else
            newBullet.GetComponent<Bullets>().damage = 1;
    }
}
