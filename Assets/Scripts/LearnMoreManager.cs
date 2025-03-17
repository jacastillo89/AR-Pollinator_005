using System.Collections.Generic;
using UnityEngine;

public class LearnMoreManager : MonoBehaviour
{
    [Header("Reference to the AR Quiz")]
    public ARQuizWithAnswers arQuiz;

    [Header("Extra Info Objects (keyed by the same name as answers)")]
    public GameObject BumbleBeeExtraInfo;
    public GameObject HoneybeesExtraInfo;
    public GameObject NativeGardenExtraInfo;
    public GameObject NestExtraInfo;
    public GameObject PollenExtraInfo;
    public GameObject PollinatorsExtraInfo;
    public GameObject PollinationExtraInfo;
    public GameObject VeggiesExtraInfo;
    public GameObject WaspExtraInfo;
    public GameObject WingsExtraInfo;

    // Internally store them in a dictionary keyed by string
    private Dictionary<string, GameObject> extraInfoDictionary;

    private void Awake()
    {
        extraInfoDictionary = new Dictionary<string, GameObject>()
        {
            { "BumbleBee",     BumbleBeeExtraInfo },
            { "Honeybees",     HoneybeesExtraInfo },
            { "NativeGarden",  NativeGardenExtraInfo },
            { "Nest",          NestExtraInfo },
            { "Pollen",        PollenExtraInfo },
            { "Pollinators",   PollinatorsExtraInfo },
            { "Pollination",   PollinationExtraInfo },
            { "Veggies",       VeggiesExtraInfo },
            { "Wasp",          WaspExtraInfo },
            { "Wings",         WingsExtraInfo }
        };

        // Optionally disable all extra info objects initially
        foreach (var infoObj in extraInfoDictionary.Values)
        {
            if (infoObj != null)
                infoObj.SetActive(false);
        }
    }

    /// <summary>
    /// Called by the "Learn More" button to display extra info for the currently active answer.
    /// </summary>
    public void ShowExtraInfo()
    {
        // 1) Ensure we can access ARQuizWithAnswers
        if (arQuiz == null)
        {
            Debug.LogWarning("[LearnMoreManager] No reference to ARQuizWithAnswers!");
            return;
        }

        // 2) Grab the name of the currently active answer
        string answerName = arQuiz.GetCurrentAnswerName();
        if (string.IsNullOrEmpty(answerName))
        {
            Debug.LogWarning("[LearnMoreManager] No active answer to show extra info for.");
            return;
        }

        // 3) Activate the matching extra info object
        if (extraInfoDictionary.ContainsKey(answerName) && extraInfoDictionary[answerName] != null)
        {
            // Optional: Hide all other extra info if you only want one visible at a time
            HideAllExtraInfo();

            extraInfoDictionary[answerName].SetActive(true);
            Debug.Log("[LearnMoreManager] Showing extra info for: " + answerName);
        }
        else
        {
            Debug.LogWarning("[LearnMoreManager] No extra info found for: " + answerName);
        }
    }

    /// <summary>
    /// Example utility if you want to hide all extra info objects.
    /// You can also call this when a new question is shown or a new answer is revealed.
    /// </summary>
    public void HideAllExtraInfo()
    {
        foreach (var infoObj in extraInfoDictionary.Values)
        {
            if (infoObj != null)
                infoObj.SetActive(false);
        }
    }
}
