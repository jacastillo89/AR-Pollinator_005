using System.Collections;
using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject activeObject;    // The currently active GameObject
    public GameObject inactiveObject;  // The currently inactive GameObject

    public AudioSource audioSource;    // Reference to an AudioSource component
    public AudioClip toggleSound;      // Audio clip to play when toggling

    // Call this method from your UI button
    public void TriggerToggle()
    {
        Debug.Log("TriggerToggle() called.");

        if (audioSource != null && toggleSound != null)
        {
            StartCoroutine(PlayAudioThenToggle());
        }
        else
        {
            ToggleGameObjects();
        }
    }

    private IEnumerator PlayAudioThenToggle()
    {
        Debug.Log("PlayAudioThenToggle() started.");

        audioSource.PlayOneShot(toggleSound);
        Debug.Log("Playing Audio...");

        // Wait for the audio to finish
        yield return new WaitForSeconds(toggleSound.length);

        Debug.Log("Audio finished, toggling objects now...");
        ToggleGameObjects();
    }

    private void ToggleGameObjects()
    {
        Debug.Log("ToggleGameObjects() called.");

        if (activeObject != null && inactiveObject != null)
        {
            activeObject.SetActive(false);
            inactiveObject.SetActive(true);

            // Swap references
            GameObject temp = activeObject;
            activeObject = inactiveObject;
            inactiveObject = temp;
        }
    }
}
