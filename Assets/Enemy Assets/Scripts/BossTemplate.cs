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

    public BossStage stage = BossStage.IDLE;

    void Update()
    {
        //Timer between boss stages
        stageTimer -= Time.deltaTime;
        if (stageTimer <= 0 && stageTimer != -1)
        {
            switch (stage)
            {
                case BossStage.IDLE:
                    if (Random.Range(0, 2) == 1) // FIXED RANGE
                    {
                        stage = BossStage.ATTACK01;
                        stageTimer = 2;
                    }
                    else
                    {
                        stage = BossStage.ATTACK02;
                        stageTimer = 4;
                    }
                    break;

                case BossStage.ATTACK01:
                case BossStage.ATTACK02:
                    stage = BossStage.COOLDOWN;
                    stageTimer = 8;
                    break;

                case BossStage.COOLDOWN:
                    stage = BossStage.IDLE;
                    stageTimer = 8;
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

    //detect bullet hits
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            currentHits++;

            // destroy bullet on hit
            Destroy(collision.gameObject);

            Debug.Log("Boss hit: " + currentHits);

            if (currentHits >= maxHits)
            {
                Destroy(gameObject); // boss dies
            }
        }
    }
}