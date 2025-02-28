using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnButtonPress : MonoBehaviour
{
    // Reference to the GameObject to be deactivated
    public GameObject objectToDeactivate;

    // This method is called when the button is pressed
    public void OnButtonPressed()
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No GameObject assigned to deactivate.");
        }
    }
}
