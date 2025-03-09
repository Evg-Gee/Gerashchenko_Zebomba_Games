using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverScreen;

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ShowGameOverScreen(int finalScore)
    {
        gameOverScreen.SetActive(true);
        gameOverScreen.GetComponentInChildren<TextMeshProUGUI>().text = $"Final Score: {finalScore}";
    }
    public void OnRestartButton()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}