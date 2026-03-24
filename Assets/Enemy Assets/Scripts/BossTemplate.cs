using UnityEngine;

public enum BossStage
{
    IDLE,
    COOLDOWN,
    ATTACK01,
    ATTACK02,
};

public class BossTemplate : MonoBehaviour
{
    public float stageTimer = 8;

    // Health system
    [SerializeField]
    private int maxHits = 75;
    private int currentHits = 0;

    [Header("Death Effect")]
    public GameObject deathEffectPrefab;

    // Idle props
    [SerializeField]
    private float idleSpeed;
    private float angle;
    private float centerPoint = 6;
    public int MaxHits => maxHits;
    public int CurrentHits => currentHits;
    public BossStage stage = BossStage.IDLE;

    // Reference to shield
    private BossShield shield;

    void Start()
    {
        shield = GetComponentInChildren<BossShield>();
    }

    public void TakeHit()
    {
        currentHits++;

        Debug.Log("Boss hit: " + currentHits);

        if (currentHits >= maxHits)
        {
            //Spawn explosion effect (same as enemies)
            GameManager.Instance.SpawnEnemyDeathEffect(transform.position);

            // disable shield before death
            shield?.SetActiveShield(false);

            Destroy(gameObject);
        }
    }

    private void Die()
    {
        // Spawn death effect if assigned
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        // Timer between boss stages
        stageTimer -= Time.deltaTime;
        if (stageTimer <= 0 && stageTimer != -1)
        {
            switch (stage)
            {
                case BossStage.IDLE:
                    if (Random.Range(0, 2) == 1)
                    {
                        stage = BossStage.ATTACK01;
                        stageTimer = 2;
                        shield?.SetActiveShield(false); // OFF
                    }
                    else
                    {
                        stage = BossStage.ATTACK02;
                        stageTimer = 4;
                        shield?.SetActiveShield(false); // OFF
                    }
                    break;

                case BossStage.ATTACK01:
                case BossStage.ATTACK02:
                    stage = BossStage.COOLDOWN;
                    stageTimer = 8;
                    shield?.SetActiveShield(true); // ON
                    break;

                case BossStage.COOLDOWN:
                    stage = BossStage.IDLE;
                    stageTimer = 8;
                    shield?.SetActiveShield(false); // OFF
                    break;
            }
        }

        switch (stage)
        {
            case BossStage.IDLE:
                angle += idleSpeed * Time.deltaTime;
                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);
                transform.position = new Vector3(x + centerPoint, 0, y);
                break;

            case BossStage.ATTACK01:
                break;

            case BossStage.ATTACK02:
                break;

            case BossStage.COOLDOWN:
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MainPlayer"))
        {
            if (GameManager.Instance.ShieldActive)
            {
                GameManager.Instance.Shield -= 40;
                if (GameManager.Instance.Shield < 0)
                {
                    GameManager.Instance.Health += GameManager.Instance.Shield / 2;
                }
            } else
            {
                GameManager.Instance.SpawnPlayerHitEffect(collision.transform.position);
                GameManager.Instance.Health -= 20;
            }
            
        }
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            if (!shield)
            {
                TakeHit();
            }
            
            Destroy(collision.gameObject);

            Debug.Log("Boss hit: " + currentHits);

            if (currentHits >= maxHits)
            {
                GameManager.Instance.enemySpawnMod -= 0.01f;
                GameManager.Instance.enemySpawnMod -= 0.01f;
                GameManager.Instance.Points += 90;
                GameManager.Instance.isBossActive = false;
                GameManager.Instance.bossTimer = 120;
                GameManager.Instance.SpawnEnemyDeathEffect(transform.position);
                Destroy(gameObject); // boss dies
            }
        }
    }
}