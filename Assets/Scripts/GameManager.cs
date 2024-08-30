using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int health;
    private int collectedCoins;
    private float score;
    private float elapsedTime;

    private float worldSpeedModifier;
    private float playerSpeedModifier;
    [SerializeField] private float maxWorldSpeed = 2.5f;
    [SerializeField] private float maxPlayerSpeed = 2;
    public float GetWorldSpeedModifier() => worldSpeedModifier;
    public float GetPlayerSpeedModifier() => playerSpeedModifier;

    public int GetHealth() => health;
    public void DecreaseHealth(int amount) => health -= amount;
    public void IncreaseHealth(int amount) => health += amount;


    public int GetCollectedCoins() => collectedCoins;
    public void DecreaseCollectedCoins(int amount) => collectedCoins -= amount;
    public void IncreaseCollectedCoins(int amount) => collectedCoins += amount;

    public float GetScore() => score;
    private void IncreaseScore(float amount) => score += amount;

    [SerializeField] private GameObject scoreUI;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject pauseScreen;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] playerPrefabs;
    private GameObject selectedPlayerSkin;


    private SaveData saveData;
    private bool isGameOver = false;
    private bool isPaused = false;

    void Awake()
    {
        Time.timeScale = 1;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        Cursor.visible = false;

        instance.scoreUI.SetActive(true);
        health = 1;
        instance.collectedCoins = 0;
        instance.score = 0;
        elapsedTime = 0;
        
        saveData = new SaveData();
        saveData.Load();
        
        selectedPlayerSkin = Instantiate(playerPrefabs[saveData.selectedSkin], player.transform.position, Quaternion.identity, player.transform);
        
        collectedCoins = 0;
        score = 0;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        worldSpeedModifier = 1;
        playerSpeedModifier = 1;
        instance.worldSpeedModifier = 1;
        instance.playerSpeedModifier = 1;
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        scoreUI.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && !isPaused)
        {
            HandleGameOver();
            elapsedTime += Time.deltaTime;
            if (worldSpeedModifier < maxWorldSpeed)
            {
                worldSpeedModifier += 0.00001f * elapsedTime;
            }

            if (playerSpeedModifier < maxPlayerSpeed)
            {
                playerSpeedModifier += 0.000008f * elapsedTime;
            }

            IncreaseScore(1 * worldSpeedModifier);

            if (scoreText != null)
            {
                scoreText.text = Mathf.Floor(score).ToString();
            }

            if (coinsText != null)
            {
                coinsText.text = "Dots: " + collectedCoins + " \u25cf";
            }
        }
    }

    private void HandleGameOver()
    {
        
        if (health <= 0)
        {
            saveData.Load();
            
            isGameOver = true;
            Cursor.visible = true;
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
            }

            elapsedTime = 0;
            worldSpeedModifier = 0;

            saveData.totalCoins += collectedCoins;
            if (score > saveData.highScore)
            {
                saveData.highScore = score;
            }

            saveData.selectedSkin = saveData.selectedSkin;
            
            saveData.Save();
            Time.timeScale = 0;
        }
    }

    public void ResetGame()
    {
        isGameOver = false;
        health = 1;
        collectedCoins = 0;
        score = 0;
        elapsedTime = 0;
        worldSpeedModifier = 1;
        playerSpeedModifier = 1;
        gameOverScreen.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        isGameOver = false;
        health = 1;
        collectedCoins = 0;
        score = 0;
        elapsedTime = 0;
        worldSpeedModifier = 1;
        playerSpeedModifier = 1;
        gameOverScreen.SetActive(false);
        
        scoreUI.SetActive(false);
        ResumeGame();
        // ResetGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

    public void PauseGame()
    {
        if (!isGameOver)
        {
            if (!isPaused)
            {
                pauseScreen.SetActive(true);
                Cursor.visible = true;
                isPaused = true;
                Time.timeScale = 0;
            }
            else
            {
                ResumeGame();
            }
        } 
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        pauseScreen.SetActive(false);
        isPaused = false;
    }
}