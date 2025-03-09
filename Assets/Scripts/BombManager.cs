using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombManager : MonoBehaviour
{
    public GameObject bombPrefab;
    public RectTransform spawnArea;
    public int totalBombs = 25;
    public float gameTime = 50f;
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 2f;

    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject battlePanel;
    public Text timerText; // Legacy UI Text
    public Text readyText; // "Ready?" Text (Legacy UI)
    public Button readyButton; // Ready Button Reference

    public TapButton tapButton;

    private int bombsSpawned = 0;
    private int bombsClicked = 0;
    private float timeLeft;
    private bool gameActive = false;

    void Start()
    {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        battlePanel.SetActive(false);
        spawnArea.gameObject.SetActive(false); // SpawnArea starts inactive

        readyButton.gameObject.SetActive(false);
        readyText.gameObject.SetActive(false);

        // Ensure the Ready Button works
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(StartBattle);
    }

    public void OpenBattlePanel()
    {
        battlePanel.SetActive(true);
        readyText.text = "Ready?";
        readyText.gameObject.SetActive(true);
        readyButton.gameObject.SetActive(true);
        spawnArea.gameObject.SetActive(false); // Keep SpawnArea inactive until battle starts
    }

    public void StartBattle()
    {
        Debug.Log("Ready button clicked! Battle started."); // Debugging

        // Hide the ready button and text
        readyText.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(false);

        // Activate the Spawn Area
        spawnArea.gameObject.SetActive(true);

        StartGame(); // Start the battle
    }

    void StartGame()
    {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        bombsSpawned = 0;
        bombsClicked = 0;
        timeLeft = gameTime;
        gameActive = true;

        StartCoroutine(TimerCountdown());
        StartCoroutine(SpawnBombs());
    }

    IEnumerator SpawnBombs()
    {
        while (bombsSpawned < totalBombs && gameActive)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            if (!gameActive) yield break;

            SpawnBomb();
        }
    }

    void SpawnBomb()
    {
        Vector3 randomPos = GetRandomPosition();
        GameObject newBomb = Instantiate(bombPrefab, spawnArea);
        newBomb.GetComponent<RectTransform>().anchoredPosition = randomPos;
        newBomb.GetComponent<Bomb>().bombManager = this;
        bombsSpawned++;
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(spawnArea.rect.xMin, spawnArea.rect.xMax);
        float y = Random.Range(spawnArea.rect.yMin, spawnArea.rect.yMax);
        return new Vector3(x, y, 0);
    }

    public void BombClicked()
    {
        bombsClicked++;
        if (bombsClicked >= totalBombs)
        {
            WinGame();
        }
    }

    IEnumerator TimerCountdown()
    {
        while (timeLeft > 0 && gameActive)
        {
            timerText.text = "Time: " + Mathf.Round(timeLeft); // Update timer
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        if (gameActive)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameActive = false;
        gameOverPanel.SetActive(true);
        if (tapButton.coins >= 100)
        {
            tapButton.coins -= 100;
        }
    }

    void WinGame()
    {
        gameActive = false;
        winPanel.SetActive(true);
        tapButton.coins += 200;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
