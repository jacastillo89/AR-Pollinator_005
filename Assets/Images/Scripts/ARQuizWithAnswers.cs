using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARQuizWithAnswers : MonoBehaviour
{
    [Header("AR Manager")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Questions (named as in the AR library)")]
    public GameObject BumbleBeeQuestion;
    public GameObject HoneybeesQuestion;
    public GameObject NativeGardenQuestion;
    public GameObject NestQuestion;
    public GameObject PollenQuestion;
    public GameObject PollinatorsQuestion;
    public GameObject PollinationQuestion;
    public GameObject VeggiesQuestion;
    public GameObject WaspQuestion;
    public GameObject WingsQuestion;

    [Header("Answers (same name as the question keys)")]
    public GameObject BumbleBeeAnswer;
    public GameObject HoneybeesAnswer;
    public GameObject NativeGardenAnswer;
    public GameObject NestAnswer;
    public GameObject PollenAnswer;
    public GameObject PollinatorsAnswer;
    public GameObject PollinationAnswer;
    public GameObject VeggiesAnswer;
    public GameObject WaspAnswer;
    public GameObject WingsAnswer;

    [Header("Feedback")]
    public GameObject incorrectFeedback;

    // INTERNAL DICTIONARIES
    private Dictionary<string, GameObject> questionDictionary;
    private Dictionary<string, GameObject> answerDictionary;

    // TRACK CURRENT STATE
    private string currentQuestionName;
    private bool questionActive;

    // NEW: Track which answer is currently displayed
    private string currentAnswerName;

    private void Awake()
    {
        // BUILD THE QUESTION DICTIONARY
        questionDictionary = new Dictionary<string, GameObject>()
        {
            { "BumbleBee",     BumbleBeeQuestion },
            { "Honeybees",     HoneybeesQuestion },
            { "NativeGarden",  NativeGardenQuestion },
            { "Nest",          NestQuestion },
            { "Pollen",        PollenQuestion },
            { "Pollinators",   PollinatorsQuestion },
            { "Pollination",   PollinationQuestion },
            { "Veggies",       VeggiesQuestion },
            { "Wasp",          WaspQuestion },
            { "Wings",         WingsQuestion }
        };

        // BUILD THE ANSWER DICTIONARY
        answerDictionary = new Dictionary<string, GameObject>()
        {
            { "BumbleBee",     BumbleBeeAnswer },
            { "Honeybees",     HoneybeesAnswer },
            { "NativeGarden",  NativeGardenAnswer },
            { "Nest",          NestAnswer },
            { "Pollen",        PollenAnswer },
            { "Pollinators",   PollinatorsAnswer },
            { "Pollination",   PollinationAnswer },
            { "Veggies",       VeggiesAnswer },
            { "Wasp",          WaspAnswer },
            { "Wings",         WingsAnswer }
        };
    }

    private void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void Start()
    {
        // PICK INITIAL QUESTION
        ShowRandomQuestion();
    }

    // -----------------------------------------
    //  CORE AR IMAGE TRACKING / CHECKING LOGIC
    // -----------------------------------------
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (!questionActive) return;

        // Check newly added images
        foreach (var trackedImage in eventArgs.added)
        {
            CheckTrackedImage(trackedImage);
        }

        // Check updated images
        foreach (var trackedImage in eventArgs.updated)
        {
            CheckTrackedImage(trackedImage);
        }
        // We ignore removed images for simplicity
    }

    private void CheckTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState != TrackingState.Tracking) return;

        string detectedName = trackedImage.referenceImage.name;
        if (detectedName.Equals(currentQuestionName))
        {
            // CORRECT
            Debug.Log("[ARQuiz] Correct answer for: " + currentQuestionName);

            if (incorrectFeedback != null)
                incorrectFeedback.SetActive(false);

            // Deactivate the question object so it's hidden when the answer is shown
            if (questionDictionary.ContainsKey(currentQuestionName) && questionDictionary[currentQuestionName] != null)
            {
                questionDictionary[currentQuestionName].SetActive(false);
            }

            // Activate the correct answer object
            if (answerDictionary.ContainsKey(currentQuestionName) && answerDictionary[currentQuestionName] != null)
            {
                answerDictionary[currentQuestionName].SetActive(true);

                // NEW: Store the name of the currently displayed answer
                currentAnswerName = currentQuestionName;
            }

            // Mark question as answered
            questionActive = false;

            // Remove the question so it won't show again
            if (questionDictionary.ContainsKey(currentQuestionName))
            {
                questionDictionary.Remove(currentQuestionName);
            }

            // Notify the progress tracker
            if (QuizProgressTracker.Instance != null)
            {
                QuizProgressTracker.Instance.OnQuestionAnsweredCorrectly();
            }
        }
        else
        {
            // INCORRECT
            Debug.Log("[ARQuiz] Incorrect answer for: " + currentQuestionName);

            if (incorrectFeedback != null)
                incorrectFeedback.SetActive(true);
        }
    }

    // -----------------------------------------
    //  MOVE TO THE NEXT QUESTION
    // -----------------------------------------
    public void NextQuestion()
    {
        // If we've already answered enough questions to "win," show the win object now.
        // (This ensures the user sees the 5th answer and THEN triggers the win UI on the button.)
        if (QuizProgressTracker.Instance != null && QuizProgressTracker.Instance.HasReachedWinCondition())
        {
            QuizProgressTracker.Instance.TriggerWinState();
        }
        else
        {
            ShowRandomQuestion();
        }
    }

    // -----------------------------------------
    //  HELPER: PICK AND SHOW A RANDOM QUESTION
    // -----------------------------------------
    private void ShowRandomQuestion()
    {
        // 1) DISABLE ALL QUESTIONS & ANSWERS
        foreach (var q in questionDictionary.Values)
        {
            if (q != null) q.SetActive(false);
        }
        foreach (var a in answerDictionary.Values)
        {
            if (a != null) a.SetActive(false);
        }

        // Hide incorrect feedback
        if (incorrectFeedback != null)
            incorrectFeedback.SetActive(false);

        // Optionally, clear out the currentAnswerName since no answer is displayed yet
        currentAnswerName = null;

        // If there are no more questions, we can stop
        if (questionDictionary.Count == 0)
        {
            Debug.Log("[ARQuiz] No more questions left!");
            return;
        }

        // 2) PICK A NEW QUESTION from those remaining
        List<string> availableQuestions = new List<string>(questionDictionary.Keys);
        int randomIndex = Random.Range(0, availableQuestions.Count);
        currentQuestionName = availableQuestions[randomIndex];
        Debug.Log("[ARQuiz] Next question: " + currentQuestionName);

        // 3) ACTIVATE THAT QUESTION
        if (questionDictionary[currentQuestionName] != null)
            questionDictionary[currentQuestionName].SetActive(true);

        // 4) RE-ENABLE CHECKING LOGIC
        questionActive = true;
    }

    // -----------------------------------------
    //  RESTART THE GAME
    // -----------------------------------------
    public void RestartGame()
    {
        // 1) Clear the dictionaries
        questionDictionary.Clear();
        answerDictionary.Clear();

        // 2) Re-populate them exactly as in Awake()
        questionDictionary.Add("BumbleBee", BumbleBeeQuestion);
        questionDictionary.Add("Honeybees", HoneybeesQuestion);
        questionDictionary.Add("NativeGarden", NativeGardenQuestion);
        questionDictionary.Add("Nest", NestQuestion);
        questionDictionary.Add("Pollen", PollenQuestion);
        questionDictionary.Add("Pollinators", PollinatorsQuestion);
        questionDictionary.Add("Pollination", PollinationQuestion);
        questionDictionary.Add("Veggies", VeggiesQuestion);
        questionDictionary.Add("Wasp", WaspQuestion);
        questionDictionary.Add("Wings", WingsQuestion);

        answerDictionary.Add("BumbleBee", BumbleBeeAnswer);
        answerDictionary.Add("Honeybees", HoneybeesAnswer);
        answerDictionary.Add("NativeGarden", NativeGardenAnswer);
        answerDictionary.Add("Nest", NestAnswer);
        answerDictionary.Add("Pollen", PollenAnswer);
        answerDictionary.Add("Pollinators", PollinatorsAnswer);
        answerDictionary.Add("Pollination", PollinationAnswer);
        answerDictionary.Add("Veggies", VeggiesAnswer);
        answerDictionary.Add("Wasp", WaspAnswer);
        answerDictionary.Add("Wings", WingsAnswer);

        // 3) Hide everything
        foreach (var qObj in questionDictionary.Values)
        {
            if (qObj != null) qObj.SetActive(false);
        }
        foreach (var aObj in answerDictionary.Values)
        {
            if (aObj != null) aObj.SetActive(false);
        }
        if (incorrectFeedback != null)
            incorrectFeedback.SetActive(false);

        questionActive = false;

        // Optionally reset the current answer name
        currentAnswerName = null;

        // 4) Reset the quiz progress
        if (QuizProgressTracker.Instance != null)
        {
            QuizProgressTracker.Instance.ResetProgress();
        }

        // 5) Start over with the first question
        ShowRandomQuestion();
    }

    // EXISTING GETTER FOR THE CURRENT QUESTION
    public string GetCurrentQuestionName()
    {
        return currentQuestionName;
    }

    // NEW GETTER FOR THE CURRENT ANSWER
    public string GetCurrentAnswerName()
    {
        return currentAnswerName;
    }
}
