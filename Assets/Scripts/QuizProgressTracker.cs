using UnityEngine;
using System.Collections.Generic;

public class QuizProgressTracker : MonoBehaviour
{
    public static QuizProgressTracker Instance { get; private set; }

    [Header("Win Condition")]
    [SerializeField]
    private int questionsToWin = 5;

    [Header("UI or GameObject to Show on Win")]
    public GameObject winUI;

    [Header("Objects to Activate in Order")]
    // Drag your "progress" objects here in the order you want them to appear
    public List<GameObject> objectsToActivateInOrder;

    private int correctAnswersCount;

    private void Awake()
    {
        // Standard singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Ensure everything starts fresh
        ResetProgress();
    }

    // Called by ARQuizWithAnswers (or your quiz script) when a question is answered correctly
    public void OnQuestionAnsweredCorrectly()
    {
        // 1) Deactivate the currently active object (if any)
        if (correctAnswersCount >= 0 && correctAnswersCount < objectsToActivateInOrder.Count)
        {
            objectsToActivateInOrder[correctAnswersCount].SetActive(false);
        }

        // 2) Increment the counter
        correctAnswersCount++;
        Debug.Log("Correct Answers: " + correctAnswersCount);

        // 3) Activate the "next" object (if it exists in the list)
        if (correctAnswersCount >= 0 && correctAnswersCount < objectsToActivateInOrder.Count)
        {
            objectsToActivateInOrder[correctAnswersCount].SetActive(true);
        }

        // 4) (Optional) Check for "win" condition here, or handle externally
        // if (correctAnswersCount >= questionsToWin)
        // {
        //     TriggerWinState();
        // }
    }

    // Use this to check if the quiz has reached the win condition
    public bool HasReachedWinCondition()
    {
        return correctAnswersCount >= questionsToWin;
    }

    // Actually show the Win UI
    public void TriggerWinState()
    {
        Debug.Log("You won! Triggering the win UI...");
        if (winUI != null)
        {
            winUI.SetActive(true);
        }
    }

    // Reset the internal progress so that the quiz can be fully replayed
    public void ResetProgress()
    {
        correctAnswersCount = 0;

        // Hide the Win UI
        if (winUI != null)
        {
            winUI.SetActive(false);
        }

        // Deactivate all objects
        foreach (GameObject obj in objectsToActivateInOrder)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // Activate the very first object, if it exists
        if (objectsToActivateInOrder.Count > 0)
        {
            objectsToActivateInOrder[0].SetActive(true);
        }
    }
}
