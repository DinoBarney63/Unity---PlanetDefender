using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = playerMaxHealth;
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
            }
        }

        if (switchGuns)
        {
            switchGuns = false;
            SwitchGuns();
        }
    }

    public void Damage(int damageTaken)
    {
        playerHealth -= damageTaken;
        regenCountingDown = true;
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SwitchGuns()
    {
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
}
