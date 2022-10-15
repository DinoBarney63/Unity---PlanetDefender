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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPostion = player.transform.position;
        transform.Translate(Vector3.Normalize(playerPostion - transform.position) * 0.05f);
    }

    public void AddValue(int value, bool heal)
    {
        partValue = value;
        float size = 0.5f + (value / 50);
        if (size > 1)
            size = 1;
        gameObject.transform.localScale = new Vector3(size, size, size);

        if (heal)
        {
            healthAmount = partValue;
            health.SetActive(true);
        } else if (Random.value > 0.6f)
        {
            healthAmount = partValue;
            health.SetActive(true);
        }
        else if (Random.value > 0.5f)
        {
            circle.SetActive(true);
        }
        else
        {
            square.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.GetComponent<GameManager>().UpdatePlayerLeveling(partValue);
            player.GetComponent<Player>().Damage(-healthAmount);
            Destroy(gameObject);
        }
    }
}
