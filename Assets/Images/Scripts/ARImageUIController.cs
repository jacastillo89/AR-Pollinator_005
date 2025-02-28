using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageUIController : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;

    [Header("Assign each canvas that matches the image names in the AR Library")]
    public GameObject BumbleBee;
    public GameObject Honeybees;
    public GameObject NativeGarden;
    public GameObject Nest;
    public GameObject Pollen;
    public GameObject Pollinators;
    public GameObject Pollination;
    public GameObject Veggies;
    public GameObject Wasp;
    public GameObject Wings;

    private Dictionary<string, GameObject> panelDictionary;

    private void Awake()
    {
        // Populate the dictionary with your imageName -> GameObject pairs
        panelDictionary = new Dictionary<string, GameObject>()
        {
            { "BumbleBee", BumbleBee },
            { "Honeybees", Honeybees },
            { "NativeGarden", NativeGarden },
            { "Nest", Nest },
            { "Pollen", Pollen },
            { "Pollinators", Pollinators },
            { "Pollination", Pollination },
            { "Veggies", Veggies },
            { "Wasp", Wasp },
            { "Wings", Wings }
        };
    }

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
        // Handle newly added images
        foreach (var trackedImage in eventArgs.added)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ActivatePanel(trackedImage.referenceImage.name);
            }
        }

        // Handle updated images
        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ActivatePanel(trackedImage.referenceImage.name);
            }
        }

        // We do not deactivate anything when tracking is lost or removed,
        // so we ignore eventArgs.removed entirely and do nothing if tracking is lost
    }

    private void ActivatePanel(string imageName)
    {
        if (panelDictionary.TryGetValue(imageName, out GameObject panel))
        {
            panel.SetActive(true);
        }
    }
}
