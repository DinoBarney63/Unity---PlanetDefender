using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGun : MonoBehaviour
{
    public Camera mainCamera;
    public bool isActive = true;
    public GameObject lazerPrefab;
    public float distanceToClosestEnemy;
    private float rotationSpeed = 0.004f;
    private float toRotate;
    private float shootRange = 30;
    private float shootOffset = 5;
    private float shootDelaySeconds = 5;
    private float shootDelay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Gose through each enemy and figures out which enemy is the closest to the player
        Enemy[] enemyList = FindObjectsOfType<Enemy>();
        float distanceToEnemy = 1000;
        distanceToClosestEnemy = 1000;
        Enemy nearestEnemy = null;

        foreach (Enemy i in enemyList)
        {
            float distanceToEnemyx = i.transform.position.x - transform.position.x;
            float distanceToEnemyy = i.transform.position.y - transform.position.y;
            distanceToEnemy = Mathf.Sqrt(Mathf.Pow(distanceToEnemyx, 2f) + Mathf.Pow(distanceToEnemyy, 2f));
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                nearestEnemy = i;
            }
        }

        // If enabled it finds the mouse location and trys to point towards it, if it can then the gun will rotate towards the mouse.
        if (isActive)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 perpendicular = transform.position - mousePos;
            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

            // If  the mouse is clicked then the gun fires.
            if (Input.GetMouseButton(0) && shootDelay < 0)
            {
                Shoot();
                shootDelay = shootDelaySeconds;
            }
        }else
        {
            // If the gun is disabled it checks if the glosest enemy is inside it's shooting range
            // If so it rotates to point it. Once pointing in the right direction it can fire
            if (distanceToClosestEnemy <= shootRange)
            {
                Vector3 playerDirection = transform.position - nearestEnemy.transform.position;
                Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
                toRotate = desiredRotation.eulerAngles.z;
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
        GameObject newBullet = Instantiate(lazerPrefab);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.localRotation;
        newBullet.GetComponent<Bullets>().damage = 10;
    }

    public void Toggle(bool OnOff)
    {
        isActive = OnOff;
    }
}