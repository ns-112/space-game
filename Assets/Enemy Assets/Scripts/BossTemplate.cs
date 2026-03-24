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

    //health system
    [SerializeField]
    private int maxHits = 10;
    private int currentHits = 0;

    //Idle props
    [SerializeField]
    private float idleSpeed;
    private float angle;
    private float centerPoint = 6;
    public int MaxHits => maxHits;
    public int CurrentHits => currentHits;
    public BossStage stage = BossStage.IDLE;

    //reference to shield
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
            Destroy(gameObject);
        }
    }



    void Update()
    {
        //Timer between boss stages
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

                        shield?.SetActiveShield(false); //OFF
                    }
                    else
                    {
                        stage = BossStage.ATTACK02;
                        stageTimer = 4;

                        shield?.SetActiveShield(false); //OFF
                    }
                    break;

                case BossStage.ATTACK01:
                case BossStage.ATTACK02:
                    stage = BossStage.COOLDOWN;
                    stageTimer = 8;

                    shield?.SetActiveShield(true); //ON
                    break;

                case BossStage.COOLDOWN:
                    stage = BossStage.IDLE;
                    stageTimer = 8;

                    shield?.SetActiveShield(false); //OFF
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

    //use TakeHit
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            TakeHit();
            Destroy(collision.gameObject);
        }
    }
}