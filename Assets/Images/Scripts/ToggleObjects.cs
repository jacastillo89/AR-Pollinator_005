using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject activeObject;  // The currently active GameObject
    public GameObject inactiveObject; // The currently inactive GameObject

    public void ToggleGameObjects()
    {
        // Swap active states
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
