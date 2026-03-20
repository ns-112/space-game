using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isEnabled = false;
    public float baseScale = 6;
    public float rotationSpeed = 25;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            sr.enabled = true;
        } else
        {
            sr.enabled = false;
        }

        if (GameManager.Instance.ShieldActive)
        {
            isEnabled = true;
        } else
        {
            isEnabled = false;
        }

        transform.localScale = new Vector3(GameManager.Instance.SmoothShield * baseScale, GameManager.Instance.SmoothShield * baseScale, 1);
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + (Time.deltaTime * rotationSpeed));
    }
}
