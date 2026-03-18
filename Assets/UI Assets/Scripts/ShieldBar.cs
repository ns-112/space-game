using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    private Slider slider;

    [SerializeField]
    private float velocity = 0f;
    [SerializeField]
    private float smoothTime = 0.1f;

    private float toValue, midValue;
    void Start()
    {
        midValue = 1;
        toValue = 1;
        slider = GetComponent<Slider>();
        slider.value = 1;
    }

    
    void Update()
    {
        if (GameManager.Instance.ShieldUpdated)
        {   
            toValue = GameManager.Instance.Shield / 100;
            GameManager.Instance.ShieldUpdated = false;
        }

        midValue = Mathf.SmoothDamp(midValue, toValue, ref velocity, smoothTime);
        slider.value = midValue;
    }
}
