using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPopup;
    public GameObject losePopup;
    public GameObject newBestScorePopup;
    public Text currentScore, diemcaonhat;
    private int bestScore;
    private bool newBestScore_;
    private int score;

    void Awake()
    {
        gameOverPopup.SetActive(false);
        newBestScore_ = false;
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
    }

    void Update()
    {
        score = Convert.ToInt16(currentScore.text);
        bestScore = Convert.ToInt16(diemcaonhat.text);
    }
    private void status()
    {
        if (score >= bestScore) newBestScore_ = true;
    }

    private void OnGameOver(bool newBestScore)
    {
        status();
        if (newBestScore_ != true)
        {
            gameOverPopup.SetActive(true);
            losePopup.SetActive(true);
            newBestScorePopup.SetActive(false);
    }
        else
        {
            gameOverPopup.SetActive(true);
            losePopup.SetActive(false);
            newBestScorePopup.SetActive(true);
        }
    }
}
