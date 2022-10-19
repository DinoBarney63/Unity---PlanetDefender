using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public GameObject enemyPrefab;
    public GameObject neutralPrefab;
    public int enemyCount;
    public int neutralCount;
    private bool playing = false;
    public int playerScore;
    public TextMeshProUGUI titleText;
    public GameObject startButtons;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public int playerDifficulty;
    public int initalPlayerDifficulty;
    private int divideDifficultyToGunCount = 20;
    public Slider progressBar;
    public TextMeshProUGUI progressBarText;
    public float levelingCount;
    public float levelingMax;
    public int playerLevel = 1;
    public float difficultyToLeveling;
    public List<Button> upgradeButtons;
    public bool waitingForUpgrade = false;
    public int option1 = 0;
    public int option2 = 0;
    public int option3 = 0;
    public int level1 = 0;
    public int level2 = 0;
    public int level3 = 0;
    public GameObject info;
    public List<Button> infoButtons;
    public GameObject panel;
    public GameObject controlsText;
    public GameObject neutralInfo;
    public GameObject enemyInfo;
    public GameObject verson;
    public GameObject MyName;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        titleText.gameObject.SetActive(true);
        startButtons.SetActive(true);
        scoreText.gameObject.SetActive(true);
        playerHealthText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        foreach (Button i in upgradeButtons)
            i.gameObject.SetActive(false);
        info.SetActive(true);
        panel.SetActive(false);
        controlsText.SetActive(false);
        neutralInfo.SetActive(false);
        enemyInfo.SetActive(false);
        verson.SetActive(true);
        MyName.SetActive(true);

        levelingCount = 0;
        levelingMax = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks the enemy and neurtal count if below the required amount new enemies and/or neutrals are spawned
        enemyCount = FindObjectsOfType<Enemy>().Length;
        neutralCount = FindObjectsOfType<Neutral>().Length;

        if ((enemyCount < Mathf.FloorToInt(playerDifficulty / divideDifficultyToGunCount)) && playing)
            SpawnEnemy();
        if ((neutralCount < Mathf.FloorToInt(playerDifficulty / (divideDifficultyToGunCount * 4))) && playing)
            SpawnNeutral();

        // When upgrades are avalable hotkeys can be used to select the upgrade
        if(waitingForUpgrade)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                UpgradeSelected(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                UpgradeSelected(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                UpgradeSelected(3);
        }
    }

    public void SpawnEnemy()
    {
        // Spawns the enemy orbit gives it a position and calculates the distance the enemy should be from the centre
        // Then calculates the strenght of the enemy and give the info the enemy
        GameObject NewEnemyOrbit = Instantiate(enemyPrefab);
        Vector3 spawnOffset = RandomSpawnPos(50, 70);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(spawnOffset.x, 2) + Mathf.Pow(spawnOffset.y, 2));
        float offset = Random.Range(500, 2000 + 1) / 100;
        if (Random.value > 0.5f)
            offset *= -1;
        float distanceToOrbit = distanceToPlayer + offset;
        NewEnemyOrbit.transform.position = player.transform.position + spawnOffset;
        int enemyStrenght = Mathf.FloorToInt(playerDifficulty / divideDifficultyToGunCount);
        NewEnemyOrbit.GetComponentInChildren<Enemy>().SetUp(distanceToOrbit, enemyStrenght);
    }

    public void SpawnNeutral()
    {
        // Spawns the neutral orbit gives it a position and calculates the distance the neutral should be from the centre
        // Then calculates the value of the neutral and give the info the neutral
        GameObject NewNeutralOrbit = Instantiate(neutralPrefab);
        Vector3 spawnOffset = RandomSpawnPos(50, 70);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(spawnOffset.x, 2) + Mathf.Pow(spawnOffset.y, 2));
        float offset = Random.Range(500, 2000 + 1) / 100;
        if (Random.value > 0.5f)
            offset *= -1;
        float distanceToOrbit = distanceToPlayer + offset;
        NewNeutralOrbit.transform.position = player.transform.position + spawnOffset;
        int value = Mathf.FloorToInt(playerDifficulty / divideDifficultyToGunCount);
        NewNeutralOrbit.GetComponentInChildren<Neutral>().SetUp(distanceToOrbit, value);
    }

    public Vector3 RandomSpawnPos(int low, int high)
    {
        // Calculates a random spawn position of the enemies and neutrals in a square perimeter []
        int A = Random.Range(low, high + 1);
        if (Random.value > 0.5f)
            A *= -1;
        int B = Random.Range(-high, high + 1);
        int x;
        int y;

        if(Random.value > 0.5f) {
            x = A;
            y = B;
        }else {
            x = B;
            y = A;
        }

        Vector3 position = new (x, y, 0);
        return position;
    }

    public void BeguinGame(int difficulty)
    {
        // Starts the game, hides the title text and buttons and sets the player's difficulty based on the difficulty selected
        playing = true;
        player.GetComponent<Player>().BeguinGame();
        titleText.gameObject.SetActive(false);
        startButtons.SetActive(false);
        info.SetActive(false);
        verson.SetActive(false);
        MyName.SetActive(false);
        initalPlayerDifficulty = difficulty;
        playerDifficulty = (divideDifficultyToGunCount * (2 * difficulty + 2)) + Random.Range(difficulty, (difficulty * difficulty) + 1);
        difficultyToLeveling = difficulty * 0.02f;
        levelingMax = 0.5f * difficultyToLeveling * Mathf.Pow(playerDifficulty, 2);
        playerLevel = 1;
    }

    public void GameOver()
    {
        // Stops the game and brings up the game over text and retry button
        playing = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        verson.SetActive(true);
        MyName.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisplayPlayerHealth(int playerHealth)
    {
        playerHealthText.text = "Health: " + playerHealth;
    }

    public void UpdatePlayerScore(int scoreToAdd)
    {
        // Adds the score and adds to the player's difficulty
        playerScore += scoreToAdd;
        scoreText.text = "Score: " + playerScore;
        playerDifficulty += Random.Range(initalPlayerDifficulty, (divideDifficultyToGunCount / 5) + Random.Range(0, initalPlayerDifficulty) - 1);
    }

    public void UpdatePlayerLeveling(int leveingAmount)
    {
        // Updates the leveing of the player, if the leveing is over the amount required the player levels up and the amount is reset
        // The levels required for the next level is set to a new higher amount base on the player's difficulty
        // Then the bar is updated along with its text
        levelingCount += leveingAmount;
        if (levelingCount >= levelingMax)
        {
            levelingCount -= levelingMax;
            levelingMax = 0.5f * difficultyToLeveling * Mathf.Pow(playerDifficulty, 2);
            playerLevel += 1;
            LevelingUp(true);
        }
        progressBar.value = levelingCount / levelingMax;
        progressBarText.text = (progressBar.value * 100).ToString("0.00") + "%";
    }

    public void LevelingUp(bool active)
    {
        // If the player is going to be leveling up
        if (active)
        {
            // Creates three random upgrades if the upgrade is the same compared to the others it is re-rolled
            // The level is also random, but dose not matter if identical to another upgrade
            // Upgrade 6 (Everything) is rare, so it is re-rolled a seconde time making it harder to get
            int upgrade = 0;
            string upgradeName;
            int option = 0;
            int level;
            
            foreach (Button i in upgradeButtons)
            {
                option += 1;
                if (option == 1)
                {
                    upgrade = Random.Range(1, 6 + 1);
                    if (upgrade == 6)
                        upgrade = Random.Range(1, 6 + 1);
                    option1 = upgrade;
                    level = Random.Range(1, 3 + 1);
                    level1 = level;
                }
                else if (option == 2)
                {
                    while (upgrade == option1)
                    {
                        upgrade = Random.Range(1, 6 + 1);
                        if (upgrade == 6)
                            upgrade = Random.Range(1, 6 + 1);
                    }
                    option2 = upgrade;
                    level = Random.Range(1, 3 + 1);
                    level2 = level;
                }
                else
                {
                    while (upgrade == option2 || upgrade == option1)
                    {
                        upgrade = Random.Range(1, 6 + 1);
                        if (upgrade == 6)
                            upgrade = Random.Range(1, 6 + 1);
                    }
                    option3 = upgrade;
                    level = Random.Range(1, 3 + 1);
                    level3 = level;
                }

                // The numbers generated for the upgrades are then turned into text to be displayed on their upgrade buttons
                if (upgrade == 1)
                    upgradeName = "Health";
                else if (upgrade == 2)
                    upgradeName = "Regeneration";
                else if (upgrade == 3)
                    upgradeName = "Range";
                else if (upgrade == 4)
                    upgradeName = "Attack Speed";
                else if (upgrade == 5)
                    upgradeName = "Damage";
                else
                    upgradeName = "Everything";
                i.GetComponentInChildren<TextMeshProUGUI>().text = upgradeName + ": " + level;
            }
        }
        // Enables or disables the buttons and the waiting for upgrade variable depending if it is the beguining or end of leveling up
        foreach (Button i in upgradeButtons)
            i.gameObject.SetActive(active);

        waitingForUpgrade = active;
    }

    public void UpgradeSelected(int chosenUpgrade)
    {
        // Once an upgrade is selected the leveing up sequence is ended
        // The upgrade selected is applied and the rest is discarded
        LevelingUp(false);
        if (chosenUpgrade == 1)
            player.GetComponent<Player>().LevelUp(option1, level1);
        else if (chosenUpgrade == 2)
            player.GetComponent<Player>().LevelUp(option2, level2);
        else
            player.GetComponent<Player>().LevelUp(option3, level3);

        option1 = 0;
        option2 = 0;
        option3 = 0;
        level1 = 0;
        level2 = 0;
        level3 = 0;
    }

    public void Infomation(int type)
    {
        titleText.gameObject.SetActive(false);
        startButtons.SetActive(false);

        foreach(Button i in infoButtons)
        {
            //i.GetComponent<Button>().colors = ColorBlock.defaultColorBlock;
            i.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }

        infoButtons[type - 1].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        controlsText.SetActive(false);
        neutralInfo.SetActive(false);
        enemyInfo.SetActive(false);

        panel.SetActive(true);

        if (type == 1)
        {
            controlsText.SetActive(true);
        }
        else if (type == 2)
        {
            neutralInfo.SetActive(true);
        }
        else if (type == 3)
        {
            enemyInfo.SetActive(true);
        }
    }
}
