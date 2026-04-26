using TMPro;
using UnityEngine;

public class FloatingDamageNumber : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshPro textMesh;

    [Header("Animation")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float fadeStartTime = 0.5f;

    private Color startColour;
    private float timer;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (textMesh == null)
        {
            textMesh = GetComponentInChildren<TextMeshPro>();
        }

        if (textMesh != null)
        {
            startColour = textMesh.color;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (mainCamera != null)
        {
            transform.forward = mainCamera.transform.forward;
        }

        if (textMesh != null && timer >= fadeStartTime)
        {
            float fadeDuration = Mathf.Max(0.01f, lifetime - fadeStartTime);
            float fadeProgress = (timer - fadeStartTime) / fadeDuration;

            Color currentColour = startColour;
            currentColour.a = Mathf.Lerp(startColour.a, 0f, fadeProgress);
            textMesh.color = currentColour;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int amount)
    {
        if (textMesh != null)
        {
            textMesh.text = amount.ToString();
        }
    }
}
