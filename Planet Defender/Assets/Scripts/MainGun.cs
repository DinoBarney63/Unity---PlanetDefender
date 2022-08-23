using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGun : MonoBehaviour
{
    public Camera mainCamera;
    public bool isActive = true;
    public GameObject bulletPrefab;
    public Enemy[] enemyList;
    public float distanceToEnemy;
    public float distanceToClosestEnemy;
    public Enemy nearestEnemy;
    private float rotationSpeed = 0.004f;
    private float toRotate;
    public float shootRange = 30;
    public float shootOffset = 5;
    public float shootDelaySeconds = 20;
    private float shootDelay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyList = FindObjectsOfType<Enemy>();

        distanceToEnemy = 1000;
        distanceToClosestEnemy = 1000;
        nearestEnemy = null;

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

        if (isActive)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 perpendicular = transform.position - mousePos;
            Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);

            if (Input.GetMouseButton(0) && shootDelay < 0)
            {
                Shoot();
                shootDelay = shootDelaySeconds;
            }
        }else
        {
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
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.localRotation;
        newBullet.GetComponent<Bullets>().damage = 10;
    }

    public void Toggle(bool OnOff)
    {
        isActive = OnOff;
    }
}