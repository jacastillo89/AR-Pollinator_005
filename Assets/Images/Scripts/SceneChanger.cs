using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Tooltip("Name of the scene you want to load.")]
    public string sceneName;

    // This method will be called when the button is clicked.
    public void ChangeScene()
    {
        // Ensure the string matches a valid scene name in your Build Settings
        SceneManager.LoadScene(sceneName);
    }
}