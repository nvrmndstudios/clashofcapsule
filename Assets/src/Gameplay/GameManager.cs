using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private GameplayController gameplayController;
    [SerializeField] private UIController uiController;

    public enum GameState
    {
        Splash,
        Menu,
        Gameplay,
        Result
    }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.Splash);
    }

    public void ChangeState(GameState newState)
    {
        Debug.Log($"Changing State: {CurrentState} → {newState}");
        ExitState(CurrentState);
        CurrentState = newState;
        EnterState(newState);
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.Splash:
                EnterSplash();
                break;
            case GameState.Menu:
                EnterMenu();
                break;
            case GameState.Gameplay:
                EnterGameplay();
                break;
            case GameState.Result:
                EnterResult();
                break;
        }
    }

    private void ExitState(GameState state)
    {
        switch (state)
        {
            case GameState.Splash:
                ExitSplash();
                break;
            case GameState.Menu:
                ExitMenu();
                break;
            case GameState.Gameplay:
                ExitGameplay();
                break;
            case GameState.Result:
                ExitResult();
                break;
        }
    }

    // ==============================
    // Splash State
    // ==============================

    private void EnterSplash()
    {
        Debug.Log("Entered Splash");
        uiController.ShowOnlySplash();
        Invoke(nameof(GoToMenu), 2f); // Go to menu after 2 seconds
    }

    private void GoToMenu()
    {
        ChangeState(GameState.Menu);
    }

    private void ExitSplash()
    {
        Debug.Log("Exited Splash");
    }

    // ==============================
    // Menu State
    // ==============================

    private void EnterMenu()
    {
        Debug.Log("Entered Menu");
        uiController.ShowOnlyMenu();

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        uiController.UpdateHighScore(highScore);
    }

    private void ExitMenu()
    {
        Debug.Log("Exited Menu");
    }

    // ==============================
    // Gameplay State
    // ==============================

    private void EnterGameplay()
    {
        Debug.Log("Entered Gameplay");
        gameplayController.StartGame();
        uiController.ShowOnlyGameplay();
        
        uiController.UpdateScore(GameData.CurrentScore);
        uiController.UpdateLife(GameData.CurrentLives);

        // TODO: Start spawning gameplay elements
    }

    private void ExitGameplay()
    {
        Debug.Log("Exited Gameplay");
        // TODO: Stop all gameplay activity (spawning, timers, etc.)
    }

    // ==============================
    // Result State
    // ==============================

    private void EnterResult()
    {
        Debug.Log("Entered Result");
        uiController.ShowOnlyResult();

        int score = GameData.CurrentScore;
        uiController.SetFinalScore(score);

        // Save high score if higher
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public void UpdateLifeAndScore()
    {
        uiController.UpdateScore(GameData.CurrentScore);
        uiController.UpdateLife(GameData.CurrentLives);
    }

    private void ExitResult()
    {
        Debug.Log("Exited Result");
    }
}