using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverScreen;

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ShowGameOverScreen(int finalScore)
    {
        gameOverScreen.SetActive(true);
        finalScoreText.text = $"Final Score: {finalScore}";
    }
    public void OnRestartButton()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}