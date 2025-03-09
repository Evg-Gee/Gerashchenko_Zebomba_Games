using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PendulumSwing pendulum;
    [SerializeField] private CircleFactory circleFactory; 
    [SerializeField] private ZoneController[] zones;
    [SerializeField] private ComboChecker comboChecker;
    [SerializeField] private GameStateManager gameState;
    [SerializeField] private UIManager uiManager;
    private ScoreManager scoreManager;
    
    void Awake()
    {
        scoreManager = new ScoreManager();
         gameState.Initialize(scoreManager);
    }
    void Start()
    {
        InitializePendulum();
    }
    
    private void OnEnable()
    {
        InputHandler.OnTap += HandleTap;
        comboChecker.OnComboDetected += HandleCombo;
        //comboChecker.OnComboDetected += scoreManager.AddScore;
        comboChecker.OnMinusScore += HandleMinusScore;
        gameState.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        InputHandler.OnTap -= HandleTap;
        comboChecker.OnComboDetected -= HandleCombo;
        //comboChecker.OnComboDetected -= scoreManager.AddScore;
        comboChecker.OnMinusScore -= HandleMinusScore;
        gameState.OnGameOver -= HandleGameOver;
    }
    private void HandleTap()
    {
        if (pendulum.HasCircle())
        {
            pendulum.ReleaseCircle();
        }
        else
        {
            GameObject circle = circleFactory.CreateCircle();
            if (circle != null)
            {            
                pendulum.AttachCircle(circle);
            }
        }
    }
    private void HandleCombo(Color color)
    {
        scoreManager.AddScore(color);
        uiManager.UpdateScore(scoreManager.GetCurrentScore());
    }
    private void HandleMinusScore(Color color)
    {
        scoreManager.SubtractScore(color);
        uiManager.UpdateScore(scoreManager.GetCurrentScore());
    }

    private void HandleGameOver()
    {
        uiManager.ShowGameOverScreen(scoreManager.GetCurrentScore());
        Time.timeScale = 0;
    }
    private void InitializePendulum()
    {
        GameObject initialCircle = circleFactory.CreateCircle();
        pendulum.AttachCircle(initialCircle);
    }
    private void InitializeZones()
    {       
        foreach (var zone in zones)
        {
            int circleCount = Random.Range(1, 3); // 1 или 2 круга
            for (int i = 0; i < circleCount; i++)
            {
                GameObject circle = circleFactory.CreateCircle();
                if (!zone.AddCircle(circle))
                {
                    Destroy(circle);
                }
            }
        }
    }
}