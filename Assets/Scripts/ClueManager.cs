using System.Collections.Generic;
using UnityEngine;

public class ClueManager : MonoBehaviour
{
    [Header("Reference to the AR Quiz")]
    public ARQuizWithAnswers arQuiz;

    [Header("Clue Objects (named as in the quiz)")]
    public GameObject BumbleBeeClue;
    public GameObject HoneybeesClue;
    public GameObject NativeGardenClue;
    public GameObject NestClue;
    public GameObject PollenClue;
    public GameObject PollinatorsClue;
    public GameObject PollinationClue;
    public GameObject VeggiesClue;
    public GameObject WaspClue;
    public GameObject WingsClue;

    // Internally store the clues in a dictionary keyed by string
    private Dictionary<string, GameObject> clueDictionary;

    private void Awake()
    {
        // Build the dictionary
        clueDictionary = new Dictionary<string, GameObject>()
        {
            { "BumbleBee",     BumbleBeeClue },
            { "Honeybees",     HoneybeesClue },
            { "NativeGarden",  NativeGardenClue },
            { "Nest",          NestClue },
            { "Pollen",        PollenClue },
            { "Pollinators",   PollinatorsClue },
            { "Pollination",   PollinationClue },
            { "Veggies",       VeggiesClue },
            { "Wasp",          WaspClue },
            { "Wings",         WingsClue }
        };

        // Optionally hide all clues at start
        foreach (var clueObj in clueDictionary.Values)
        {
            if (clueObj != null)
                clueObj.SetActive(false);
        }
    }

    /// <summary>
    /// Called by your UI button to show the clue for the current question.
    /// </summary>
    public void ShowClue()
    {
        // 1) Obtain the current question name from the ARQuizWithAnswers script
        if (arQuiz == null)
        {
            Debug.LogWarning("[ClueManager] No reference to ARQuizWithAnswers!");
            return;
        }

        string currentQuestionName = arQuiz.GetCurrentQuestionName();
        if (string.IsNullOrEmpty(currentQuestionName))
        {
            Debug.LogWarning("[ClueManager] No question is currently active.");
            return;
        }

        // 2) If a clue matches that question, activate it
        if (clueDictionary.ContainsKey(currentQuestionName) && clueDictionary[currentQuestionName] != null)
        {
            // Optional: Hide other clues first, if you only want one visible at a time
            HideAllClues();

            // Now show the relevant clue
            clueDictionary[currentQuestionName].SetActive(true);
            Debug.Log("[ClueManager] Showing clue for " + currentQuestionName);
        }
        else
        {
            Debug.LogWarning("[ClueManager] No matching clue for question: " + currentQuestionName);
        }
    }

    /// <summary>
    /// Example helper function if you want to hide all clues 
    /// before showing a new one, or when the question changes.
    /// </summary>
    public void HideAllClues()
    {
        foreach (var clueObj in clueDictionary.Values)
        {
            if (clueObj != null)
                clueObj.SetActive(false);
        }
    }
}
