using System.Collections;
using UnityEngine;

public class FloatingTextEffect : MonoBehaviour
{
    [SerializeField] private float floatDistance = 100f;
    [SerializeField] private float duration = 0.8f;

    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    private Vector3 endPos;

    public void Initialize(Vector3 worldPosition, Canvas canvas)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        transform.SetParent(canvas.transform, false);
        rect = GetComponent<RectTransform>();
        rect.position = screenPos;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startPos = rect.anchoredPosition;
        endPos = startPos + new Vector3(0, floatDistance, 0);

        StartCoroutine(AnimateFloatUp());
    }

    private IEnumerator AnimateFloatUp()
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float curve = Mathf.Sin(t * Mathf.PI * 0.5f); // ease-out

            rect.anchoredPosition = Vector3.Lerp(startPos, endPos, curve);
            canvasGroup.alpha = 1 - t;

            yield return null;
        }

        Destroy(gameObject);
    }
}