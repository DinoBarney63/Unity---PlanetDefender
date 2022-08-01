using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGun : MonoBehaviour
{
    public Camera mainCamera;
    public bool isActive = true;
    public float rotation;
    public GameObject playerBulletPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 perpendicular = transform.position - mousePos;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);

            if (transform.localRotation.eulerAngles.z <= 180f)
            {
                rotation = transform.localRotation.eulerAngles.z;
            }
            else
            {
                rotation = transform.localRotation.eulerAngles.z - 360f;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
        
    }

    public void Shoot()
    {
        GameObject newBullet = Instantiate(playerBulletPrefab);
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = transform.localRotation;
    }
}