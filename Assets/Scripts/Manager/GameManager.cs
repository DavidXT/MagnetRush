using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum States {Playing, GameOver, Pause, Win}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public States state;

    private int playerMagnet = 0;
    private float playerScore = 0;
    [SerializeField] private float scoreSpeed;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI endGameTitle;
    [SerializeField] private TextMeshProUGUI magnetText;
    [SerializeField] private GameObject endGamePanel;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        state = States.Playing;
    }

    private void Update()
    {
        if (state != States.Playing) return;
        playerScore += scoreSpeed * Time.deltaTime;
        UpdateScoreText();
    }
    public void UpdateScoreText()
    {
        if(scoreText != null)
        {
            scoreText.text = Mathf.RoundToInt(playerScore).ToString();
        }
    }

    public void UpdateMagnetText()
    {
        if (magnetText != null)
        {
            magnetText.text = playerMagnet.ToString();
        }
    }


    public void IncreaseMagnet(int i)
    {
        playerMagnet += i;
        playerScore += playerMagnet;
        UpdateMagnetText();
    }

    public void GameOver()
    {
        state = States.GameOver;
        //Show gameOverPanel + Stop score counting
        if(endGamePanel != null)
        {
            endGameTitle.text = "GAME OVER";
            endGamePanel.SetActive(true);
            UpdateScore(Mathf.RoundToInt(playerScore));
        }
    }

    public void WinGame()
    {
        state = States.Win;
        if(endGamePanel != null)
        {
            endGameTitle.text = "YOU WIN";
            endGamePanel.SetActive(true);
            UpdateScore(Mathf.RoundToInt(playerScore));
        }
    }

    private void UpdateScore(int score)
    {
        if(score > PlayerPrefs.GetInt("MagnetRush"+ SceneManager.GetActiveScene().name))
        {
            PlayerPrefs.SetInt("MagnetRush" + SceneManager.GetActiveScene().name, score);
        }
        gameOverScoreText.text = PlayerPrefs.GetInt("MagnetRush" + SceneManager.GetActiveScene().name).ToString();
    }
    
}
