using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageObjectController : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject targetObject; // Specify the GameObject to activate in the inspector

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ActivateObject();
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ActivateObject();
            }
        }
    }

    private void ActivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }

    // Method to deactivate the GameObject, if needed (e.g., when a button is pressed)
    public void DeactivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
