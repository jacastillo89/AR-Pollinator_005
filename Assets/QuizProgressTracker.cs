using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script tracks the total number of questions answered correctly.
// When the count reaches 'questionsToWin', it triggers a "win" state.
public class QuizProgressTracker : MonoBehaviour
{
    // Singleton instance (optional but convenient for simple manager scripts)
    public static QuizProgressTracker Instance { get; private set; }

    [Header("Win Condition")]
    [SerializeField]
    private int questionsToWin = 5;

    [Header("UI or GameObject to Show on Win")]
    public GameObject winUI;

    // Internally track how many questions have been answered correctly
    private int correctAnswersCount;

    private void Awake()
    {
        // Ensure there's only one instance of QuizProgressTracker
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Uncomment if you want this object to persist across scenes:
        // DontDestroyOnLoad(gameObject);
    }

    // Call this method whenever a question is answered correctly
    public void OnQuestionAnsweredCorrectly()
    {
        correctAnswersCount++;
        Debug.Log("Correct Answers: " + correctAnswersCount);

        // Check if we've reached our target number of correct answers
        if (correctAnswersCount >= questionsToWin)
        {
            TriggerWinState();
        }
    }

    // Called once we've reached the number of correct answers to win
    private void TriggerWinState()
    {
        Debug.Log("You won!");

        // Show some UI or do anything else you want in the "win" scenario
        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        // If you want to stop future questions, load another scene, or show a final message,
        // you can do so here. For example:
        // SceneManager.LoadScene("WinScene");
    }
}
