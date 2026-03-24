using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public BossTemplate boss;
    private Slider slider;

    [SerializeField] private float velocity = 0f;
    [SerializeField] private float smoothTime = 0.3f;

    private float toValue, midValue;

    void Start()
    {
        slider = GetComponentInChildren<Slider>();

        midValue = 1f;
        toValue = 1f;

        slider.value = 1f;
    }

    void Update()
    {
        // Hide bar if boss doesn't exist yet
        if (!boss)
        {
            slider.gameObject.SetActive(false);
            return;
        }

        slider.gameObject.SetActive(true);

        toValue = (float)(boss.MaxHits - boss.CurrentHits) / boss.MaxHits;
        midValue = Mathf.SmoothDamp(midValue, toValue, ref velocity, smoothTime);

        slider.value = midValue;
    }
}