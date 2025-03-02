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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

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

        // 3) Check for "win" condition
        if (correctAnswersCount >= questionsToWin)
        {
            TriggerWinState();
        }
    }

    private void TriggerWinState()
    {
        Debug.Log("You won!");
        if (winUI != null)
        {
            winUI.SetActive(true);
        }
    }
}
