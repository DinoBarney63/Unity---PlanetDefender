using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private GameManager gameManager;
    public int playerMaxHealth = 100;
    public int playerHealth;
    private bool regenCountingDown = true;
    private float regenCountdown;
    private float regenCountdownMax = 5;
    private float regenTime;
    private float regenTimeMax = 2;
    public GameObject mainGun;
    public GameObject[] subGuns;
    private bool playing = false;
    public bool alive = true;
    private bool mainGunActive = false;
    private bool subGunActive = false;
    public Button mainGunButton;
    public TextMeshProUGUI mainGunText;
    public Button subGunButton;
    public TextMeshProUGUI subGunText;
    public float distanceToClosestEnemy;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerHealth = playerMaxHealth;
        alive = true;

        //Disables guns until the game beguins
        mainGunActive = false;
        subGunActive = false;
        mainGun.GetComponent<MainGun>().Toggle(false);
        foreach (GameObject i in subGuns)
            i.GetComponent<SubGun>().Toggle(false);

        mainGunText.text = "Main: Disabled";
        mainGunText.color = Color.red;
        subGunText.text = "Sub: Disabled";
        subGunText.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
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
                if (distanceToEnemy > 55)
                {
                    i.GetComponent<Enemy>().SpeedUp(10);
                }
            }

            // If the closest enemy is further than 55 its rotation speed is increased until in range 
            if (distanceToClosestEnemy > 55)
            {
                nearestEnemy.GetComponent<Enemy>().SpeedUp(30);
            }

            // Health regen timer
            // If the inital timer is counting down...
            if (regenCountingDown && playerHealth != playerMaxHealth)
            {
                // If the timer is above 0 then the timer is decreased by time
                if (regenCountdown > 0)
                    regenCountdown -= Time.deltaTime;
                else
                {
                    // Otherwise the timer is set to 0 and the inital timer is deactivated
                    regenCountdown = regenCountdownMax;
                    regenTime = regenTimeMax;
                    regenCountingDown = false;
                }
            }
            else
            {
                // Since the initial timer is deactivated then the second timer can run
                // If this timer is above 0 then the timer is decreased by time
                if (regenTime > 0)
                    regenTime -= Time.deltaTime;
                else
                {
                    // Once the timer is up then the timer is reset and the health is increased by 1
                    regenTime = regenTimeMax;
                    if (playerHealth < playerMaxHealth)
                        playerHealth += 1;
                    // Player health is displayed
                    gameManager.GetComponent<GameManager>().DisplayPlayerHealth(playerHealth);
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
                ToggleMainGun();
            if (Input.GetKeyDown(KeyCode.H))
                ToggleSubGuns();
        }
    }

    public void Damage(int damageTaken)
    {
        // Reduce players health and if below or equal to 0 then the player is dead
        playerHealth -= damageTaken;
        regenCountingDown = true;
        if (playerHealth <= 0)
        {
            alive = false;
            playing = false;
            gameObject.SetActive(false);
            gameManager.GetComponent<GameManager>().GameOver();
            playerHealth = 0;
        }
        gameManager.GetComponent<GameManager>().DisplayPlayerHealth(playerHealth);
    }

    public void ToggleMainGun()
    {
        if (playing)
        {
            if(!mainGunActive)
            {
                mainGun.GetComponent<MainGun>().Toggle(true);
                mainGunActive = true;
                mainGunText.text = "Main: Manual";
                mainGunText.color = Color.green;
            }else
            {
                mainGun.GetComponent<MainGun>().Toggle(false);
                mainGunActive = false;
                mainGunText.text = "Main: Auto";
                mainGunText.color = Color.red;
            }
        }
    }

    public void ToggleSubGuns()
    {
        if (playing)
        {
           if (!subGunActive)
            {
                foreach (GameObject i in subGuns)
                    i.GetComponent<SubGun>().Toggle(true);
                subGunActive = true;
                subGunText.text = "Sub: Manual";
                subGunText.color = Color.green;
            }
            else
            {
                foreach (GameObject i in subGuns)
                    i.GetComponent<SubGun>().Toggle(false);
                subGunActive = false;
                subGunText.text = "Sub: Auto";
                subGunText.color = Color.red;
            }
        }
    }

    public void BeguinGame()
    {
        playing = true;
        ToggleMainGun();
        ToggleSubGuns();
    }
}
