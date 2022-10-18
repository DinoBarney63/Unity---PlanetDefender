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
    private float shootOffset = 5;
    public GameObject bulletPrefab;
    public GameObject lazerPrefab;
    private float shootDelaySecondsMain = 10;
    private float shootDelaySecondsSub = 2;
    private float shootDelay;
    public bool main = true;
    public float shootRange = 20;
    public int damage = 1;
    public List<GameObject> barrelAndBody;
    public Color colour;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        // If the gun is a main it is given its main gun delay and coloured otherwise it is given the sub-gun delay
        if (main)
        {
            shootDelay = shootDelaySecondsMain;
            colour = GetComponentInParent<Enemy>().gunColours[GetComponentInParent<Enemy>().type];
            foreach (GameObject i in barrelAndBody)
            {
                i.GetComponent<SpriteRenderer>().color = colour;
            }
        }
        else
            shootDelay = shootDelaySecondsSub;
    }

    // Update is called once per frame
    void Update()
    {
        // Claculates the distance to the player
        float distanceToPlayerx = player.transform.position.x - transform.position.x;
        float distanceToPlayery = player.transform.position.y - transform.position.y;
        distanceToPlayer = Mathf.Sqrt(Mathf.Pow(distanceToPlayerx, 2f) + Mathf.Pow(distanceToPlayery, 2f));

        // Checks if the player is within firing range. If so the the gun is pointed towards the player and once pointed it fires one the delay is up
        // Also the shoot delay only counts down while in range of the player
        if (distanceToPlayer <= shootRange && player.GetComponent<Player>().alive)
        {
            Vector3 playerDirection = transform.position - player.transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
            toRotate = desiredRotation.eulerAngles.z;
            // Checks to see if the gun cna turn to the position required
            if ((toRotate >= point1 && toRotate <= point2) == between)
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

            if ((transform.rotation.eulerAngles.z + shootOffset > toRotate) && (transform.rotation.eulerAngles.z - shootOffset < toRotate) && shootDelay < 0 && player.GetComponent<Player>().alive)
            {
                // Main gun shoots lazers and sub-gun shoots bullets and then the delay is reset
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
            shootDelay -= Time.deltaTime;
        }
    }

    // Sub gun spawning
    public void Spawned(float startingAngleDeg, Vector3 enemyPos)
    {
        // Sets position and rotation restrictions based on starting rotation
        transform.Rotate(0, 0, startingAngleDeg);
        float startingAngleRad = (startingAngleDeg / 180 * Mathf.PI) - (Mathf.PI / 2);
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

        // Changes the colour of the sub-sun based on the parent
        colour = GetComponentInParent<Enemy>().gunColours[GetComponentInParent<Enemy>().type];
        foreach (GameObject i in barrelAndBody)
        {
            i.GetComponent<SpriteRenderer>().color = colour;
        }
    }

    public void Shoot(GameObject projectile)
    {
        // Creates a new bullet and sets its rotation and position to the guns
        GameObject newBullet = Instantiate(projectile);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.localRotation;
        if(main)
            newBullet.GetComponent<Bullets>().damage = 2 * damage;
        else
            newBullet.GetComponent<Bullets>().damage = damage;
    }
}
