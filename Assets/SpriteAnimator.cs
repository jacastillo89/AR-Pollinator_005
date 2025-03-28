using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{
    public Image targetImage;
    public string spriteFolder = "Sprites"; // Folder inside Resources
    public float frameRate = 24f;

    private Sprite[] animationFrames;
    private int currentFrame = 0;
    private Coroutine animationCoroutine;

    void Start()
    {
        // Load all sprites from Resources/Sprites (sorted alphabetically)
        animationFrames = Resources.LoadAll<Sprite>(spriteFolder);
        StartAnimation();
    }

    public void StartAnimation()
    {
        if (animationFrames == null || animationFrames.Length == 0 || targetImage == null)
        {
            Debug.LogWarning("Animation frames or target image not set.");
            return;
        }

        if (animationCoroutine == null)
        {
            animationCoroutine = StartCoroutine(PlayAnimation());
        }
    }

    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    private IEnumerator PlayAnimation()
    {
        float delay = 1f / frameRate;

        while (true)
        {
            targetImage.sprite = animationFrames[currentFrame];
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            yield return new WaitForSeconds(delay);
        }
    }
}
