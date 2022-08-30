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
    public int enemyCount;
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
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if ((spawn || enemyCount < playerDifficulty) && playing)
        {
            spawn = false;
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        GameObject NewEnemyOrbit = Instantiate(enemyInOrbitPrefab);
        Vector3 offset = randomSpawnPos(50, 70);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(offset.x, 2) + Mathf.Pow(offset.y, 2));
        NewEnemyOrbit.transform.position = player.transform.position + offset;
        NewEnemyOrbit.GetComponentInChildren<Enemy>().SetUp(distanceToPlayer, Random.Range(playerDifficulty - 1, playerDifficulty + 1));
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
        playerDifficulty = Random.Range(0, difficulty) + Random.Range(0, difficulty) + difficulty;
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

    public void UpdatePlayerScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.text = "Score: " + playerScore;
        playerDifficulty += Random.Range(0, 1);
    }
}
