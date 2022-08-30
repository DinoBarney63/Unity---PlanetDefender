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
    public bool mainGunEnabled = true;
    public GameObject[] subGuns;
    public bool switchGuns;
    private bool playing = false;
    public bool alive = true;
    public Button mainGunButton;
    public TextMeshProUGUI mainGunText;
    public Button subGunButton;
    public TextMeshProUGUI subGunText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerHealth = playerMaxHealth;
        alive = true;

        //Disables guns until the game beguins
        mainGun.GetComponent<MainGun>().Toggle(false);
        foreach (GameObject i in subGuns)
            i.GetComponent<SubGun>().Toggle(false);
    }

    // Update is called once per frame
    void Update()
    {
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

        // Temprary way to switch player's guns
        if (switchGuns && playing)
        {
            switchGuns = false;
            SwitchGuns();
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
            gameObject.SetActive(false);
            gameManager.GetComponent<GameManager>().GameOver();
            playerHealth = 0;
        }
        gameManager.GetComponent<GameManager>().DisplayPlayerHealth(playerHealth);
    }

    public void SwitchGuns()
    {
        // Switches between main gun and sub-guns
        if (mainGunEnabled)
        {
            mainGunEnabled = false;
            mainGun.GetComponent<MainGun>().Toggle(false);
            foreach(GameObject i in subGuns)
                i.GetComponent<SubGun>().Toggle(true);
        }else
        {
            mainGunEnabled = true;
            mainGun.GetComponent<MainGun>().Toggle(true);
            foreach (GameObject i in subGuns)
                i.GetComponent<SubGun>().Toggle(false);
        }
            
    }

    public void BeguinGame()
    {
        playing = true;
        mainGun.GetComponent<MainGun>().Toggle(true);
    }
}
