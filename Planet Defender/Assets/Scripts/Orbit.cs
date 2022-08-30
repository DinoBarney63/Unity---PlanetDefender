using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject player;
    public bool rotating = false;
    public float speed = 1.0f;
    public float speedySpeed = 10.0f;
    public bool clockwise = true;
    private int direction;
    public float startingAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        Vector3 playerDirection = transform.position - player.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 1);
        startingAngle = transform.rotation.eulerAngles.z;
        double speedOffset = Random.Range(-10, 10) * 0.1;
        speed = 1.5f + (float)speedOffset;
        clockwise = Random.value > 0.5f;
        rotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(rotating)
        {
            if (clockwise)
                direction = -1;
            else
                direction = 1;

            transform.Rotate(0, 0, speed * direction * Time.deltaTime);
        }
    }

    public void SpeedUp()
    {
       transform.Rotate(0, 0, speedySpeed * direction * Time.deltaTime);
    }
}
