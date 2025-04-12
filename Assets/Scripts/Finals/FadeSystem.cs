using UnityEngine;

public class FadeSystem : MonoBehaviour
{
    public CanvasGroup canvasGroup;   
    public float fadeDuration = 1.5f; 

    void Start()
    {
        if (canvasGroup != null)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            Debug.LogWarning("CanvasGroup not assigned!");
        }
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
