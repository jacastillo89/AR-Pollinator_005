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

    private Dictionary<string, GameObject> questionDictionary;
    private Dictionary<string, GameObject> answerDictionary;

    private string currentQuestionName;
    private bool questionActive;

    private void Awake()
    {
        // Build the question dictionary
        questionDictionary = new Dictionary<string, GameObject>()
        {
            { "BumbleBee", BumbleBeeQuestion },
            { "Honeybees", HoneybeesQuestion },
            { "NativeGarden", NativeGardenQuestion },
            { "Nest", NestQuestion },
            { "Pollen", PollenQuestion },
            { "Pollinators", PollinatorsQuestion },
            { "Pollination", PollinationQuestion },
            { "Veggies", VeggiesQuestion },
            { "Wasp", WaspQuestion },
            { "Wings", WingsQuestion }
        };

        // Build the answer dictionary
        answerDictionary = new Dictionary<string, GameObject>()
        {
            { "BumbleBee", BumbleBeeAnswer },
            { "Honeybees", HoneybeesAnswer },
            { "NativeGarden", NativeGardenAnswer },
            { "Nest", NestAnswer },
            { "Pollen", PollenAnswer },
            { "Pollinators", PollinatorsAnswer },
            { "Pollination", PollinationAnswer },
            { "Veggies", VeggiesAnswer },
            { "Wasp", WaspAnswer },
            { "Wings", WingsAnswer }
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
        // Disable all question objects
        foreach (var q in questionDictionary.Values)
        {
            if (q != null)
                q.SetActive(false);
        }
        // Disable all answer objects
        foreach (var a in answerDictionary.Values)
        {
            if (a != null)
                a.SetActive(false);
        }
        // Disable incorrect feedback
        if (incorrectFeedback != null)
            incorrectFeedback.SetActive(false);

        // Pick a random key from the question dictionary
        List<string> keys = new List<string>(questionDictionary.Keys);
        int randomIndex = Random.Range(0, keys.Count);
        currentQuestionName = keys[randomIndex];

        // Activate the chosen question
        if (questionDictionary[currentQuestionName] != null)
        {
            questionDictionary[currentQuestionName].SetActive(true);
        }

        questionActive = true;
        Debug.Log("[ARQuizWithAnswers] Chosen question: " + currentQuestionName);
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (!questionActive) return;

        // Handle newly added or updated images
        foreach (var trackedImage in eventArgs.added)
            CheckTrackedImage(trackedImage);

        foreach (var trackedImage in eventArgs.updated)
            CheckTrackedImage(trackedImage);

        // We don't handle removed images for this scenario
    }

    private void CheckTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState != TrackingState.Tracking)
            return;

        string detectedName = trackedImage.referenceImage.name;
        Debug.Log("[ARQuizWithAnswers] Detected: " + detectedName
                  + " | Required: " + currentQuestionName);

        // If the detected image name matches the current question's name
        if (detectedName.Equals(currentQuestionName))
        {
            // CORRECT ANSWER
            Debug.Log("[ARQuizWithAnswers] Correct answer!");

            // Hide incorrect feedback if it was active
            if (incorrectFeedback != null)
                incorrectFeedback.SetActive(false);

            // Activate the corresponding "Answer" GameObject
            if (answerDictionary.ContainsKey(currentQuestionName) &&
                answerDictionary[currentQuestionName] != null)
            {
                answerDictionary[currentQuestionName].SetActive(true);
            }

            // Mark question as answered
            questionActive = false;

            // You could proceed to next question after a delay, etc.
        }
        else
        {
            // INCORRECT
            Debug.Log("[ARQuizWithAnswers] Incorrect answer.");
            if (incorrectFeedback != null)
            {
                incorrectFeedback.SetActive(true);
            }
        }
    }
}
