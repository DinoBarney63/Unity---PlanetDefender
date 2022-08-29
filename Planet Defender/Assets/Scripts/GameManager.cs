using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public int difficultyPercentage = 25;
    public GameObject enemyInOrbitPrefab;
    public int enemyCount;
    public bool spawn = false;
    public int enemyCountMax = 10;
    private bool playing = false;
    public bool startGame = false;
    private bool breaker = true;
    public TextMeshProUGUI titleText;
    public Button startButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        titleText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        playerHealthText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if ((spawn || enemyCount < enemyCountMax) && playing)
        {
            spawn = false;
            SpawnEnemy();
        }

        if (startGame && breaker)
        {
            breaker = false;
            BeguinGame();
        }
    }

    public void SpawnEnemy()
    {
        GameObject NewEnemyOrbit = Instantiate(enemyInOrbitPrefab);
        Vector3 offset = randomSpawnPos(50, 70);
        float distanceToPlayer = Mathf.Sqrt(Mathf.Pow(offset.x, 2) + Mathf.Pow(offset.y, 2));
        NewEnemyOrbit.transform.position = player.transform.position + offset;
        NewEnemyOrbit.GetComponentInChildren<Enemy>().SetUp(distanceToPlayer, 3);
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

    public void BeguinGame()
    {
        playing = true;
        player.GetComponent<Player>().BeguinGame();
        titleText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
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
}
