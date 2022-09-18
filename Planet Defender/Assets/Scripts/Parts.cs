using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    public GameObject circle;
    public GameObject square;
    public GameObject health;
    public int partValue;
    public int healthAmount;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");

        if (Random.value > 0.5f)
        {
            circle.SetActive(true);
            square.SetActive(false);
            health.SetActive(false);
        }
        else
        {
            circle.SetActive(false);
            square.SetActive(true);
            health.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPostion = player.transform.position;
        transform.Translate(Vector3.Normalize(playerPostion - transform.position) * 0.05f);
    }

    public void AddValue(int value)
    {
        partValue = value;
        float size = partValue * 0.1f;
        gameObject.transform.localScale = new Vector3(size, size, size);
        
        if(Random.value > 0.6f)
        {
            healthAmount = partValue;
            circle.SetActive(false);
            square.SetActive(false);
            health.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.GetComponent<GameManager>().UpdatePlayerLeveling(partValue);
            player.GetComponent<Player>().Damage(-healthAmount);
            Destroy(gameObject);
        }
    }
}
