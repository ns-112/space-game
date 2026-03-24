using UnityEngine;

public class BossShield : MonoBehaviour
{
    public bool isEnabled = false;
    public float baseScale = 6;
    public float rotationSpeed = 25;

    private SpriteRenderer sr;
    private BossTemplate boss;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        boss = GetComponentInParent<BossTemplate>();
    }

    public void SetActiveShield(bool active)
    {
        isEnabled = active;

        if (TryGetComponent(out SpriteRenderer sr))
            sr.enabled = active;

        if (TryGetComponent(out Collider col))
            col.enabled = active;
    }

    void LateUpdate()
    {
        if (!boss) return;

        //Enable shield ONLY during cooldown (not moving)
        isEnabled = (boss.stage == BossStage.COOLDOWN);

        sr.enabled = isEnabled;

        if (!isEnabled) return;

        //Optional scaling
        transform.localScale = new Vector3(baseScale, baseScale, 1);

        // Rotate shield
        transform.localEulerAngles += new Vector3(0, 0, Time.deltaTime * rotationSpeed);
    }
}