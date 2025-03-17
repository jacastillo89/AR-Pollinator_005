using System.Collections;
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

    [Header("Audio Setup")]
    // An AudioSource that will play the clue audio
    public AudioSource clueAudioSource;

    // A single AudioClip for all clues
    public AudioClip clueAudio;

    // Internally store the clues in a dictionary keyed by string
    private Dictionary<string, GameObject> clueDictionary;

    private void Awake()
    {
        // Build the dictionary of clues
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
        HideAllClues();

        // Optional: Stop audio at start, if needed
        if (clueAudioSource != null)
        {
            clueAudioSource.Stop();
            clueAudioSource.panStereo = 0f; // Center
        }
    }

    /// <summary>
    /// Called by your UI button to show the clue for the current question.
    /// </summary>
    public void ShowClue()
    {
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

        // 1) If a clue matches that question, activate it
        if (clueDictionary.ContainsKey(currentQuestionName) && clueDictionary[currentQuestionName] != null)
        {
            HideAllClues();
            clueDictionary[currentQuestionName].SetActive(true);

            Debug.Log("[ClueManager] Showing clue for " + currentQuestionName);

            // 2) Play the single audio clip from LEFT to RIGHT
            if (clueAudioSource != null && clueAudio != null)
            {
                clueAudioSource.Stop();
                clueAudioSource.clip = clueAudio;
                StartCoroutine(PlayAudioLeftToRight());
            }
        }
        else
        {
            Debug.LogWarning("[ClueManager] No matching clue for question: " + currentQuestionName);
        }
    }

    /// <summary>
    /// Hide the currently displayed clue and play audio from RIGHT to LEFT.
    /// </summary>
    public void CloseClue()
    {
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

        // Hide all clues immediately
        HideAllClues();

        // If there's an audio clip, play it from RIGHT to LEFT
        if (clueAudioSource != null && clueAudio != null)
        {
            clueAudioSource.Stop();
            clueAudioSource.clip = clueAudio;
            StartCoroutine(PlayAudioRightToLeft());
        }

        Debug.Log("[ClueManager] Closing clue for " + currentQuestionName);
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

    /// <summary>
    /// Coroutine to play audio while panning from LEFT (-1) to RIGHT (+1).
    /// Adjust the duration if you want faster/slower panning.
    /// </summary>
    private IEnumerator PlayAudioLeftToRight()
    {
        float duration = 2f; // total time for panning
        float elapsed = 0f;

        // Start from full left
        clueAudioSource.panStereo = -1f;
        clueAudioSource.Play();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Lerp from -1 to +1
            clueAudioSource.panStereo = Mathf.Lerp(-1f, 1f, elapsed / duration);
            yield return null;
        }

        // Ensure we end at full right
        clueAudioSource.panStereo = 1f;
    }

    /// <summary>
    /// Coroutine to play audio while panning from RIGHT (+1) to LEFT (-1).
    /// Adjust the duration if you want faster/slower panning.
    /// </summary>
    private IEnumerator PlayAudioRightToLeft()
    {
        float duration = 2f; // total time for panning
        float elapsed = 0f;

        // Start from full right
        clueAudioSource.panStereo = 1f;
        clueAudioSource.Play();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Lerp from +1 to -1
            clueAudioSource.panStereo = Mathf.Lerp(1f, -1f, elapsed / duration);
            yield return null;
        }

        // Ensure we end at full left
        clueAudioSource.panStereo = -1f;
    }
}
