using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Tooltip("Name of the scene you want to load.")]
    public string sceneName;

    [Tooltip("Attach an Audio Source here.")]
    public AudioSource audioSource;

    [Tooltip("The audio clip to play when the button is pressed.")]
    public AudioClip buttonSound;

    // This method will be called when the button is clicked.
    public void ChangeScene()
    {
        StartCoroutine(PlaySoundAndLoad());
    }

    private IEnumerator PlaySoundAndLoad()
    {
        if (audioSource != null && buttonSound != null)
        {
            // Play the sound once
            audioSource.PlayOneShot(buttonSound);

            // Wait for the clip’s duration before switching scenes
            yield return new WaitForSeconds(buttonSound.length);
        }

        // Now load the new scene
        SceneManager.LoadScene(sceneName);
    }
}