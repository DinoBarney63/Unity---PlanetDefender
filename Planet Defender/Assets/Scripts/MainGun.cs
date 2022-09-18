using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGun : MonoBehaviour
{
    private GameObject player;
    public Camera mainCamera;
    public bool isActive = true;
    public GameObject lazerPrefab;
    public float distanceToClosestEnemy;
    public float distanceToClosestNeutral;
    private float rotationSpeed = 0.004f;
    private float toRotate;
    private float shootRange = 25;
    private float shootOffset = 5;
    public float shootDelaySeconds = 5;
    private float shootDelay;
    public int damage = 10;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
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
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

            // If  the mouse is clicked then the gun fires.
            if (Input.GetMouseButton(1) && shootDelay < 0)
            {
                Shoot();
                shootDelay = shootDelaySeconds - (player.GetComponent<Player>().attackSpeedLevel * shootDelaySeconds * 0.1f);
            }
        }else
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
            // If so it rotates to point it. Once pointing in the right direction it fires
            if (distanceToClosestEnemy <= shootRange + ((player.GetComponent<Player>().rangeLevel - 1) * 5))
            {
                Vector3 playerDirection = transform.position - nearestEnemy.transform.position;
                Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
                toRotate = desiredRotation.eulerAngles.z;
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

                if ((transform.rotation.eulerAngles.z + shootOffset > toRotate) && (transform.rotation.eulerAngles.z - shootOffset < toRotate) && shootDelay < 0)
                {
                    Shoot();
                    shootDelay = shootDelaySeconds - (player.GetComponent<Player>().attackSpeedLevel * shootDelaySeconds * 0.1f);
                }
            }else if (distanceToClosestNeutral <= shootRange + ((player.GetComponent<Player>().rangeLevel - 1) * 5))
            {
                // If the closest enemy is out of range it checks if the closest neutral is inside it's shooting range
                // If so it rotates to point it. Once pointing in the right direction it fires
                Vector3 playerDirection = transform.position - nearestNeutral.transform.position;
                Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
                toRotate = desiredRotation.eulerAngles.z;
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

                if ((transform.rotation.eulerAngles.z + shootOffset > toRotate) && (transform.rotation.eulerAngles.z - shootOffset < toRotate) && shootDelay < 0)
                {
                    Shoot();
                    shootDelay = shootDelaySeconds - (player.GetComponent<Player>().attackSpeedLevel * shootDelaySeconds * 0.1f);
                }
            }
        }

        shootDelay -= Time.deltaTime;
    }

    public void Shoot()
    {
        // Creates a new bullet and sets its rotation and position to the guns
        GameObject newBullet = Instantiate(lazerPrefab);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.localRotation;
        newBullet.GetComponent<Bullets>().damage = damage * player.GetComponent<Player>().damageLevel;
    }

    public void Toggle(bool OnOff)
    {
        isActive = OnOff;
    }
}