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
    // Drag your "reward" objects here in the order you want them to appear
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

    // Called by ARQuizWithAnswers when a question is answered correctly
    public void OnQuestionAnsweredCorrectly()
    {
        correctAnswersCount++;
        Debug.Log("Correct Answers: " + correctAnswersCount);

        // 1) Deactivate the previously activated object (if any)
        if (correctAnswersCount - 2 >= 0 && correctAnswersCount - 2 < objectsToActivateInOrder.Count)
        {
            GameObject prevObject = objectsToActivateInOrder[correctAnswersCount - 2];
            if (prevObject != null)
            {
                prevObject.SetActive(false);
            }
        }

        // 2) Activate the current (new) object in the list, if it exists
        if (correctAnswersCount - 1 < objectsToActivateInOrder.Count)
        {
            GameObject currentObject = objectsToActivateInOrder[correctAnswersCount - 1];
            if (currentObject != null)
            {
                currentObject.SetActive(true);
            }
        }

        // 3) Check for "win" condition, but *do not* activate it immediately.
        //    We simply note that we've reached the condition. ARQuizWithAnswers
        //    will show the Win screen on the next button press instead.
        // if (correctAnswersCount >= questionsToWin)
        // {
        //     TriggerWinState(); // <-- Removed or commented out, so we don't show the Win UI right now
        // }
    }

    // ARQuizWithAnswers will call this to see if we reached 5 correct answers
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
        if (winUI != null)
        {
            winUI.SetActive(false);
        }

        // Optionally deactivate any objects that might still be active
        foreach (GameObject obj in objectsToActivateInOrder)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
