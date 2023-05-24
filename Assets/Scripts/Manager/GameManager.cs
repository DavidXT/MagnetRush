using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum States {Playing, GameOver, Pause}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public States state;

    private int playerMagnet = 0;
    private float playerScore = 0;
    [SerializeField] private float scoreSpeed;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI magnetText;
    [SerializeField] private GameObject gameOverPanel;
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
        UpdateMagnetText();
    }

    public void GameOver()
    {
        state = States.GameOver;
        //Show gameOverPanel + Stop score counting
        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
