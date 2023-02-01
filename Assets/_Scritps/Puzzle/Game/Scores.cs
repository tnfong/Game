using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class BestScoreData
{
    public int score = 0;
}

public class Scores : MonoBehaviour
{
    public SquareTextureData squareTextureData;
    public Text scoreTextCurent, scoreTextEndGame, getBestScore;
    private int currentScore_;
    private bool newBestScore = false;
    private BestScoreData bestScore_ = new BestScoreData();
    //private int currentScores_;
    private string bestScoreKey = "bsdat";


    private void Awake()
    {
        if (BinaryDataStream.Exist(bestScoreKey))
        {
            StartCoroutine(ReadDataFile());
        }
    }

    private IEnumerator ReadDataFile()
    {
        bestScore_ = BinaryDataStream.Read<BestScoreData>(bestScoreKey);
        yield return new WaitForEndOfFrame();
    }

    void Start()
    {
        currentScore_ = 0;
        newBestScore = false;
        squareTextureData.SetStartColor();
        UpdateScoresText();
    }

    private void OnEnable()
    {
        GameEvents.AddScores += AddScores;
        GameEvents.GameOver += SaveBestScores;
    }

    private void OnDisable()
    {
        GameEvents.AddScores -= AddScores;
        GameEvents.GameOver -= SaveBestScores;
    }

    private void SaveBestScores(bool newBestScore)
    {
        BinaryDataStream.Save<BestScoreData>(bestScore_, bestScoreKey);
    }

    private void AddScores(int scores)
    {
        currentScore_ += scores;
        UpdateScoresText();
        if (currentScore_ > bestScore_.score)
        {
            newBestScore = true;
            bestScore_.score = currentScore_;
            SaveBestScores(newBestScore);
        }
        UpdateSquareColor();
        GameEvents.UpdateBestScoreBar(currentScore_, bestScore_.score);
        getBestScore.text = bestScore_.score.ToString();
        UpdateScoresText();
    }

    private void UpdateSquareColor()
    {
        if(GameEvents.UpdateSquareColor != null && currentScore_ >= squareTextureData.tresholdVal)
        {
            squareTextureData.UpdateColors(currentScore_);
            GameEvents.UpdateSquareColor(squareTextureData.currentColor);
        }
    }

    private void UpdateScoresText()
    {
        scoreTextCurent.text = currentScore_.ToString();
        scoreTextEndGame.text = currentScore_.ToString();
    }
}
