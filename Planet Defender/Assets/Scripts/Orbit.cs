using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    private GameObject player;
    public bool rotating = false;
    public float speed;
    public bool clockwise = true;
    private int direction;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        // Points away from the player and decides the rotation speed and direction and then beguins rotating
        Vector3 playerDirection = transform.position - player.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 1);
        double speedOffset = Random.Range(-10, 10 + 1) * 0.1;
        speed = 1.5f + (float)speedOffset;
        clockwise = Random.value > 0.5f;
        rotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotates
        if(rotating)
        {
            if (clockwise)
                direction = -1;
            else
                direction = 1;

            transform.Rotate(0, 0, speed * direction * Time.deltaTime);
        }
    }

    public void SpeedUp(float speeding)
    {
       transform.Rotate(0, 0, speeding * direction * Time.deltaTime);
    }
}
