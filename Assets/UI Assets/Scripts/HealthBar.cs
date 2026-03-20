using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //440 -> 510
    private Slider slider;

    [SerializeField]
    private float velocity = 0f;
    [SerializeField]
    private float smoothTime = 0.3f;

    private float toValue, midValue;
    void Start()
    {
        midValue = 1;
        toValue = 1;
        slider = GetComponent<Slider>();
        slider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.HealthUpdated)
        {   
            toValue = GameManager.Instance.Health / 100;
            GameManager.Instance.HealthUpdated = false;
        }

        midValue = Mathf.SmoothDamp(midValue, toValue, ref velocity, smoothTime);
        slider.value = midValue;
    }

}
