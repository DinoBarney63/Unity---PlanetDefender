using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    private GameObject player;
    public bool rotating = false;
    public float speed = 1.0f;
    public bool clockwise = true;
    private int direction;
    public float startingAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
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

    public void Beguin()
    {
        Vector3 playerDirection = transform.position - player.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 1);
        startingAngle = transform.rotation.eulerAngles.z;
        rotating = true;
    }
}
