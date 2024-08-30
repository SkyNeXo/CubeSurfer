using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Menu : MonoBehaviour
{
    
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject highscoreText;
    [SerializeField] private GameObject dotsText;
    [SerializeField] private GameObject titleText;
    
    private float amplitude = 6f;
    private float frequency = 2.0f;
    private Vector3 startPosition;
    private int playerBalance;
    
    private SaveData saveData;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        startPosition = titleText.transform.position;
        shop.SetActive(false);
        saveData = new SaveData();
        saveData.Load();
        playerBalance = saveData.totalCoins;
        highscoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + Mathf.Floor(saveData.highScore);
        dotsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Dots: " + playerBalance + " \u25cf";
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        titleText.transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
    
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    
    public void CloseGame()
    {
        Application.Quit();
    }
    
    public void OpenShop()
    {
        saveData.Load();
        mainMenu.SetActive(false);
        shop.SetActive(true);
    }
    
    public void CloseShop()
    {
        saveData.Load();
        playerBalance = saveData.totalCoins;
        dotsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Dots: " + playerBalance + " \u25cf";
        mainMenu.SetActive(true);
        shop.SetActive(false);
    }
}
