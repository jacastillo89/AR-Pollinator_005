using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFrameAnimator : MonoBehaviour
{
    public Sprite[] frames; // Assign 50 images in the Inspector
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        currentFrame = 0;
        StartCoroutine(PlayAnimation());
    }

    private System.Collections.IEnumerator PlayAnimation()
    {
        while (currentFrame < frames.Length)
        {
            spriteRenderer.sprite = frames[currentFrame];
            currentFrame++;
            yield return null; // Wait for one frame
        }

        Destroy(gameObject); // Remove the prefab after animation finishes
    }
}
