using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject crossArrow;
    [SerializeField] private GameObject circleArrow;
    [SerializeField] private GameObject crossYouText;
    [SerializeField] private GameObject circleYouText;
    [SerializeField] private TextMeshProUGUI crossScoreText;
    [SerializeField] private TextMeshProUGUI circleScoreText;

    private void Awake()
    {
        crossScoreText.text = "";
        circleScoreText.text = "";
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnCurrentPlayablePlayerTypeChanged += GameManager_OnCurrentPlayablePlayerTypeChanged;
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
    }

    private void GameManager_OnScoreChanged(object sender, EventArgs e)
    {
        GameManager.Instance.GetScores(out int crossScore, out int circleScore);
        
        crossScoreText.text = crossScore.ToString();
        circleScoreText.text = circleScore.ToString();
    }

    private void GameManager_OnCurrentPlayablePlayerTypeChanged(object sender, EventArgs e)
    {
        UpdateCurrentArrow();
    }

    private void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetLocalPlayerType() == GameManager.PlayerType.Cross)
        {
            crossYouText.SetActive(true);
        }
        else
        {
            circleYouText.SetActive(true);
        }
        
        crossScoreText.text = "0";
        circleScoreText.text = "0";
        
        UpdateCurrentArrow();
    }

    private void UpdateCurrentArrow()
    {
        if (GameManager.Instance.GetCurrentPlayablePlayerType() == GameManager.PlayerType.Cross)
        {
            crossArrow.SetActive(true);
            circleArrow.SetActive(false);
        }
        else
        {
            crossArrow.SetActive(false);
            circleArrow.SetActive(true);
        }
    }
}
