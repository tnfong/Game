using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreBar : MonoBehaviour
{
    public Image fillImage;
    public Text bestScoreText;


    private void OnEnable()
    {
        GameEvents.UpdateBestScoreBar += UpdateBestScoreBar;
    }

    private void OnDisable()
    {
        GameEvents.UpdateBestScoreBar -= UpdateBestScoreBar;
    }

    public void UpdateBestScoreBar(int currentScore, int bestScore)
    {
        float currentPrecenttage = (float)currentScore / (float)bestScore;
        fillImage.fillAmount = currentPrecenttage;
        bestScoreText.text = bestScore.ToString();
    }
}
