using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public GameObject enemyInOrbitPrefab;
    public GameObject neutralInOrbitPrefab;
    public int enemyCount;
    public int neutralCount;
    private bool playing = false;
    public bool startGame = false;
    public int playerScore;
    public TextMeshProUGUI titleText;
    public List<Button> startButtons;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public int playerDifficulty;
    private int divideDifficultyToGunCount = 20;
    public Slider progressBar;
    public TextMeshProUGUI progressBarText;
    public float levelingCount;
    public float levelingMax;
    public int playerLevel = 1;
    public float difficultyToLeveling = 1.2f;
    public List<Button> upgradeButtons;
    public bool waitingForUpgrade = false;
    public int option1 = 0;
    public int option2 = 0;
    public int option3 = 0;
    public int level1 = 0;
    public int level2 = 0;
    public int level3 = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        titleText.gameObject.SetActive(true);
        foreach (Button i in startButtons)
            i.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        playerHealthText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        foreach (Button i in upgradeButtons)
            i.gameObject.SetActive(false);

        levelingCount = 0;
        levelingMax = 0;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        neutralCount = FindObjectsOfType<Neutral>().Length;

        if ((enemyCount < Mathf.FloorToInt(playerDifficulty / divideDifficultyToGunCount)) && playing)
        {
            SpawnEnemy();
        }

        if ((neutralCount < Mathf.FloorToInt(playerDifficulty / (divideDifficultyToGunCount * 4))) && playing)
        {
            SpawnNeutral();
        }

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
        GameObject NewEnemyOrbit = Instantiate(enemyInOrbitPrefab);
        Vector3 spawnOffset = randomSpawnPos(50, 70);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(spawnOffset.x, 2) + Mathf.Pow(spawnOffset.y, 2));
        float offset = Random.Range(500, 2000) / 100;
        if (Random.value > 0.5f)
            offset *= -1;
        float distanceToOrbit = distanceToPlayer + offset;
        NewEnemyOrbit.transform.position = player.transform.position + spawnOffset;
        NewEnemyOrbit.GetComponentInChildren<Enemy>().SetUp(distanceToOrbit, Mathf.FloorToInt(playerDifficulty / divideDifficultyToGunCount));
    }

    public void SpawnNeutral()
    {
        GameObject NewNeutralOrbit = Instantiate(neutralInOrbitPrefab);
        Vector3 spawnOffset = randomSpawnPos(50, 70);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(spawnOffset.x, 2) + Mathf.Pow(spawnOffset.y, 2));
        float offset = Random.Range(500, 2000) / 100;
        if (Random.value > 0.5f)
            offset *= -1;
        float distanceToOrbit = distanceToPlayer + offset;
        NewNeutralOrbit.transform.position = player.transform.position + spawnOffset;
        NewNeutralOrbit.GetComponentInChildren<Neutral>().SetUp(distanceToOrbit);
    }

    public Vector3 randomSpawnPos(int low, int high)
    {
        int A = Random.Range(low, high);
        if (Random.value > 0.5f)
            A *= -1;
        int B = Random.Range(-high, high);
        int x;
        int y;

        if(Random.value > 0.5f)
        {
            x = A;
            y = B;
        }else
        {
            x = B;
            y = A;
        }

        Vector3 position = new (x, y, 0);
        return position;
    }

    public void BeguinGame(int difficulty)
    {
        playing = true;
        player.GetComponent<Player>().BeguinGame();
        titleText.gameObject.SetActive(false);
        foreach (Button i in startButtons)
            i.gameObject.SetActive(false);
        playerDifficulty = (divideDifficultyToGunCount * (difficulty + 2)) + Random.Range(difficulty, difficulty * difficulty);
        levelingMax = difficultyToLeveling * playerDifficulty;
        playerLevel = 1;
    }

    public void GameOver()
    {
        playing = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisplayPlayerHealth(int playerHealth)
    {
        playerHealthText.text = "Health: " + playerHealth;
    }

    public void UpdatePlayerScore(int scoreToAdd, int enemyGunCount)
    {
        playerScore += scoreToAdd;
        scoreText.text = "Score: " + playerScore;
        playerDifficulty += Random.Range(1, enemyGunCount);

        
    }

    public void UpdatePlayerLeveling(int leveingAmount)
    {
        levelingCount += leveingAmount;
        if (levelingCount >= levelingMax)
        {
            levelingCount -= levelingMax;
            levelingMax = difficultyToLeveling * playerDifficulty;
            playerLevel += 1;
            LevelingUp(true);
        }
        progressBar.value = levelingCount / levelingMax;
        progressBarText.text = (progressBar.value * 100).ToString("0.00") + "%";
    }

    public void LevelingUp(bool active)
    {
        if (active)
        {
            int upgrade = 0;
            string upgradeName;
            int option = 0;
            int level;

            foreach (Button i in upgradeButtons)
            {
                option += 1;
                if (option == 1)
                {
                    upgrade = Random.Range(1, 5);
                    option1 = upgrade;
                    level = Random.Range(1, 3);
                    level1 = level;
                }
                else if (option == 2)
                {
                    while (upgrade == option1)
                    {
                        upgrade = Random.Range(1, 5);
                    }
                    option2 = upgrade;
                    level = Random.Range(1, 3);
                    level2 = level;
                }
                else
                {
                    while (upgrade == option2 || upgrade == option1)
                    {
                        upgrade = Random.Range(1, 5);
                    }
                    option3 = upgrade;
                    level = Random.Range(1, 3);
                    level3 = level;
                } 

                // Somehow options 3 and five are rarer than the others and five is the rarest
                if (upgrade == 1)
                    upgradeName = "Health";
                else if (upgrade == 2)
                    upgradeName = "Regeneration";
                else if (upgrade == 3)
                    upgradeName = "Range";
                else if (upgrade == 4)
                    upgradeName = "Attack Speed";
                else
                    upgradeName = "Damage";
                i.GetComponentInChildren<TextMeshProUGUI>().text = upgradeName + ": " + level;
            }
        }

        foreach (Button i in upgradeButtons)
            i.gameObject.SetActive(active);

        waitingForUpgrade = active;
    }

    public void UpgradeSelected(int chosenUpgrade)
    {
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
}
