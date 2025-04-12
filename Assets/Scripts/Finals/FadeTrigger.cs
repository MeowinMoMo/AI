using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
    public CanvasGroup canvasGroup;     
    public float fadeDuration = 1f;     
    public string playerTag = "Player"; 

    private bool hasFadedIn = false;

    void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasFadedIn && other.CompareTag(playerTag))
        {
            hasFadedIn = true;
            StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
