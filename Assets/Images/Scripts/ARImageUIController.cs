using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageUIController : MonoBehaviour
{
    // These need to be public or [SerializeField] so they show in Inspector
    public ARTrackedImageManager trackedImageManager;
    public GameObject uiPanel;

    // Tracks whether the UI is currently open (thus ignoring further detections)
    private bool uiOpen = false;

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
        // If UI is already open, ignore any new tracking events
        if (uiOpen) return;

        // Check newly detected images
        foreach (var trackedImage in eventArgs.added)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                OpenUI();
            }
        }

        // Check updated images
        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                OpenUI();
            }
        }

        // We don't need to handle removed images for this scenario
        // because once UI is open, we ignore tracking updates
    }

    private void OpenUI()
    {
        uiOpen = true;
        uiPanel.SetActive(true);

        // Optional: Completely stop new detections:
        // trackedImageManager.enabled = false;
    }

    // This method is called by the Close Button on the UI
    public void CloseUI()
    {
        uiOpen = false;
        uiPanel.SetActive(false);

        // Re-enable detections if you disabled them earlier:
        // trackedImageManager.enabled = true;
    }
}
