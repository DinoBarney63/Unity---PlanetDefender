using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGun : MonoBehaviour
{
    public Camera mainCamera;
    public bool isActive = false;
    public GameObject bulletPrefab;
    public Enemy[] enemyList;
    public float distanceToEnemy;
    public float distanceToClosestEnemy;
    public Enemy nearestEnemy;
    private float rotationSpeed = 0.004f;
    private bool between = true;
    private float point1 = 0;
    private float point2 = 360;
    private float toRotate;
    public float shootRange = 20;
    public float shootOffset = 5;
    public float shootDelaySeconds = 2;
    private float shootDelay;
    public bool falseActive;


    // Start is called before the first frame update
    void Start()
    {
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
            toRotate = desiredRotation.eulerAngles.z;
            if ((toRotate >= point1 && toRotate <= point2) == between)
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.time * rotationSpeed);


            if ((transform.rotation.eulerAngles.z + shootOffset > toRotate) && (transform.rotation.eulerAngles.z - shootOffset < toRotate) && Input.GetMouseButton(0) && shootDelay < 0)
            {
                Shoot();
                shootDelay = shootDelaySeconds;
            }
        }
        else
        {
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
        }

        shootDelay -= Time.deltaTime;
    }

    public void Shoot()
    {
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
