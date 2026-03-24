using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public BossTemplate boss;    // assign in Inspector
    private Slider slider;

    [SerializeField] private float velocity = 0f;
    [SerializeField] private float smoothTime = 0.3f;

    private float toValue, midValue;

    [SerializeField] private Vector3 screenOffset = new Vector3(0, -50, 0); // pixels below boss

    void Start()
    {
        slider = GetComponent<Slider>();
        if (!slider) Debug.LogError("Slider component missing on BossHealthBar!");

        midValue = 1f;
        toValue = 1f;
        slider.value = 1f;
    }

    void Update()
    {
        if (!boss || !slider) return;

        // Update target health (0-1)
        toValue = (float)(boss.MaxHits - boss.CurrentHits) / boss.MaxHits;

        // Smooth interpolation like player health bar
        midValue = Mathf.SmoothDamp(midValue, toValue, ref velocity, smoothTime);
        slider.value = midValue;

        // Make slider follow boss position on screen
        Vector3 screenPos = Camera.main.WorldToScreenPoint(boss.transform.position);

        if (screenPos.z > 0) // in front of camera
        {
            slider.gameObject.SetActive(true);
            transform.position = screenPos + screenOffset;
        }
        else
        {
            // hide slider if boss is behind camera
            slider.gameObject.SetActive(false);
        }
    }
}