using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{
    public Image targetImage;                // Reference to the UI Image
    public string spriteFolder = "Sprites";  // Folder inside Resources
    public float frameRate = 24f;            // Frames per second
    public float fadeDuration = 0.5f;        // Fade-out duration in seconds

    private Sprite[] animationFrames;
    private int currentFrame = 0;
    private Coroutine animationCoroutine;

    void Start()
    {
        // Load sprites from Resources folder
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

        // Reset alpha in case object was reactivated
        Color color = targetImage.color;
        targetImage.color = new Color(color.r, color.g, color.b, 1f);

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

        // Play the animation once
        for (int i = 0; i < animationFrames.Length; i++)
        {
            targetImage.sprite = animationFrames[i];
            yield return new WaitForSeconds(delay);
        }

        // Fade out
        Color originalColor = targetImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            targetImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure fully transparent at the end
        targetImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Deactivate the whole GameObject
        gameObject.SetActive(false);

        animationCoroutine = null;
    }
}
