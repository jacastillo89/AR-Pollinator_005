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

    // INTERNAL DICTIONARIES: Key = reference image name
    private Dictionary<string, GameObject> questionDictionary;
    private Dictionary<string, GameObject> answerDictionary;

    // TRACK CURRENT STATE
    private string currentQuestionName;
    private bool questionActive; // Are we currently waiting for the correct image?

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
            CheckTrackedImage(trackedImage);

        // Check updated images
        foreach (var trackedImage in eventArgs.updated)
            CheckTrackedImage(trackedImage);

        // We ignore removed images for simplicity
    }

    private void CheckTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState != TrackingState.Tracking) return;

        string detectedName = trackedImage.referenceImage.name;
        if (detectedName.Equals(currentQuestionName))
        {
            // CORRECT!
            Debug.Log("[ARQuiz] Correct answer for: " + currentQuestionName);

            if (incorrectFeedback != null)
                incorrectFeedback.SetActive(false);

            // Activate the correct answer object
            if (answerDictionary.ContainsKey(currentQuestionName)
                && answerDictionary[currentQuestionName] != null)
            {
                answerDictionary[currentQuestionName].SetActive(true);
            }

            // Mark question as answered
            questionActive = false;
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
        // This method can be linked to a Button 
        // on the "answer" UI to move on.

        ShowRandomQuestion();
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
        if (incorrectFeedback != null) incorrectFeedback.SetActive(false);

        // 2) PICK A NEW QUESTION
        List<string> keys = new List<string>(questionDictionary.Keys);
        int randomIndex = Random.Range(0, keys.Count);
        currentQuestionName = keys[randomIndex];
        Debug.Log("[ARQuiz] Next question: " + currentQuestionName);

        // 3) ACTIVATE THAT QUESTION
        if (questionDictionary[currentQuestionName] != null)
            questionDictionary[currentQuestionName].SetActive(true);

        // 4) RE-ENABLE CHECKING LOGIC
        questionActive = true;
    }
}
