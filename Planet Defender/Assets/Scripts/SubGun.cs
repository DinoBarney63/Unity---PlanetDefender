using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGun : MonoBehaviour
{
    private GameObject player;
    public Camera mainCamera;
    public bool isActive = false;
    public GameObject bulletPrefab;
    public float distanceToClosestEnemy;
    public float distanceToClosestNeutral;
    private float rotationSpeed = 0.004f;
    private bool between = true;
    private float point1 = 0;
    private float point2 = 360;
    private float toRotate;
    private float shootRange = 25;
    private float shootOffset = 5;
    private float shootDelaySeconds = 0.75f;
    private float shootDelay;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        // Sets the gun restrictions based on the starting rotation
        float startingAngleDeg = transform.rotation.eulerAngles.z;

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

    // Update is called once per frame
    void Update()
    {
        // If enabled it finds the mouse location and trys to point towards it, if it can then the gun will rotate towards the mouse.
        if (isActive)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 perpendicular = transform.position - mousePos;
            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
            toRotate = desiredRotation.eulerAngles.z;
            if ((toRotate >= point1 && toRotate <= point2) == between)
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

            // If the gun is pointed close to where the mouse is and the mouse is clicked then the gun fires.
            if ((transform.rotation.eulerAngles.z + shootOffset > toRotate) && (transform.rotation.eulerAngles.z - shootOffset < toRotate) && Input.GetMouseButton(0) && shootDelay < 0)
            {
                Shoot();
                shootDelay = shootDelaySeconds;
            }
        }
        else
        {
            // Calculates which neutral is the closest to the player
            Neutral[] neutralList = FindObjectsOfType<Neutral>();
            distanceToClosestNeutral = 1000;
            Neutral nearestNeutral = null;
            foreach (Neutral i in neutralList)
            {
                float distanceToNeutral = player.GetComponent<Player>().DistanceToNeutral(i);
                if (distanceToNeutral < distanceToClosestNeutral)
                {
                    distanceToClosestNeutral = distanceToNeutral;
                    nearestNeutral = i;
                }
            }

            // Calculates which enemy is the closest to the player
            Enemy[] enemyList = FindObjectsOfType<Enemy>();
            distanceToClosestEnemy = 1000;
            Enemy nearestEnemy = null;
            foreach (Enemy i in enemyList)
            {
                float distanceToEnemy = player.GetComponent<Player>().DistanceToEnemy(i);
                if (distanceToEnemy < distanceToClosestEnemy)
                {
                    distanceToClosestEnemy = distanceToEnemy;
                    nearestEnemy = i;
                }
            }

            // If the gun is disabled it checks if the closest enemy is inside it's shooting range
            // If so it rotates to point it, if it can. Once pointing in the right direction it fires
            if (distanceToClosestEnemy <= shootRange)
            {
                Vector3 playerDirection = transform.position - nearestEnemy.transform.position;
                Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
                toRotate = desiredRotation.eulerAngles.z;
                if ((toRotate >= point1 && toRotate <= point2) == between)
                    transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

                if ((transform.rotation.eulerAngles.z + shootOffset > toRotate) && (transform.rotation.eulerAngles.z - shootOffset < toRotate) && shootDelay < 0)
                {
                    Shoot();
                    shootDelay = shootDelaySeconds;
                }
            }
            else if (distanceToClosestNeutral <= shootRange)
            {
                // If the closest enemy is out of range it checks if the closest neutral is inside it's shooting range
                // If so it rotates to point it, if it can. Once pointing in the right direction it fires
                Vector3 playerDirection = transform.position - nearestNeutral.transform.position;
                Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
                toRotate = desiredRotation.eulerAngles.z;
                if ((toRotate >= point1 && toRotate <= point2) == between)
                    transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

                if ((transform.rotation.eulerAngles.z + shootOffset > toRotate) && (transform.rotation.eulerAngles.z - shootOffset < toRotate) && shootDelay < 0)
                {
                    Shoot();
                    shootDelay = shootDelaySeconds;
                }
            }
        }

        shootDelay -= Time.deltaTime;
    }

    public void Shoot()
    {
        // Creates a new bullet and sets its rotation and position to the guns
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.localRotation;
        newBullet.GetComponent<Bullets>().damage = 1;
    }

    public void Toggle(bool OnOff)
    {
        isActive = OnOff;
    }
}
