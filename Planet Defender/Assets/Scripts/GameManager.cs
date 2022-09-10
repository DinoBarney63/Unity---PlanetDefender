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
    public bool spawn = false;
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
    public Slider progressBar;
    public TextMeshProUGUI progressBarText;
    public float levelingCount = 0.0f;
    public float levelingMax = 100.0f;

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
        levelingMax = 100;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        neutralCount = FindObjectsOfType<Neutral>().Length;

        if ((spawn || enemyCount < Mathf.FloorToInt(playerDifficulty / 10)) && playing)
        {
            spawn = false;
            SpawnEnemy();
        }

        if ((neutralCount < Mathf.FloorToInt(playerDifficulty / 50)) && playing)
        {
            SpawnNeutral();
        }
    }

    public void SpawnEnemy()
    {
        GameObject NewEnemyOrbit = Instantiate(enemyInOrbitPrefab);
        Vector3 offset = randomSpawnPos(50, 70);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(offset.x, 2) + Mathf.Pow(offset.y, 2));
        NewEnemyOrbit.transform.position = player.transform.position + offset;
        NewEnemyOrbit.GetComponentInChildren<Enemy>().SetUp(distanceToPlayer, Mathf.FloorToInt(playerDifficulty / 10));
    }

    public void SpawnNeutral()
    {
        GameObject NewNeutralOrbit = Instantiate(neutralInOrbitPrefab);
        Vector3 offset = randomSpawnPos(50, 70);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(offset.x, 2) + Mathf.Pow(offset.y, 2));
        NewNeutralOrbit.transform.position = player.transform.position + offset;
        NewNeutralOrbit.GetComponentInChildren<Neutral>().SetUp(distanceToPlayer);
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
        playerDifficulty = Random.Range(difficulty + 2, difficulty * Random.Range(1, difficulty + 2)) + 3 * (difficulty + 2) + 25;
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
        progressBar.value = levelingCount / levelingMax;
        progressBarText.text = (progressBar.value * 100).ToString("0.0") + "%";
    }
}
