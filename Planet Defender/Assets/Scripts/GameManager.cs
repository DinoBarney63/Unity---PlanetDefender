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
    public Button startButtonEasy;
    public Button startButtonMedium;
    public Button startButtonHard;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public int playerDifficulty;
    private int divideDifficultyToGunCount = 15;
    public Slider progressBar;
    public TextMeshProUGUI progressBarText;
    public float levelingCount;
    public float levelingMax;
    public int playerLevel = 1;
    private float difficultyToLeveling = 1.1f;
    public Button upgrade1;
    public Button upgrade2;
    public Button upgrade3;
    public bool waitingForUpgrade = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        titleText.gameObject.SetActive(true);
        startButtonEasy.gameObject.SetActive(true);
        startButtonMedium.gameObject.SetActive(true);
        startButtonHard.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        playerHealthText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

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
                Upgrade1();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                Upgrade2();
            if (Input.GetKeyDown(KeyCode.Alpha3))
                Upgrade3();
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
        int numberA = Random.Range(low, high);
        if (Random.value > 0.5f)
            numberA *= -1;
        int numberB = Random.Range(-high, high);
        int x;
        int y;

        if(Random.value > 0.5f)
        {
            x = numberA;
            y = numberB;
        }else
        {
            x = numberB;
            y = numberA;
        }

        Vector3 position = new (x, y, 0);
        return position;
    }

    public void BeguinGame(int difficulty)
    {
        playing = true;
        player.GetComponent<Player>().BeguinGame();
        titleText.gameObject.SetActive(false);
        startButtonEasy.gameObject.SetActive(false);
        startButtonMedium.gameObject.SetActive(false);
        startButtonHard.gameObject.SetActive(false);
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
        upgrade1.gameObject.SetActive(active);
        upgrade2.gameObject.SetActive(active);
        upgrade3.gameObject.SetActive(active);
        waitingForUpgrade = active;
    }

    public void Upgrade1()
    {
        LevelingUp(false);
    }

    public void Upgrade2()
    {
        LevelingUp(false);
    }

    public void Upgrade3()
    {
        LevelingUp(false);
    }
}
