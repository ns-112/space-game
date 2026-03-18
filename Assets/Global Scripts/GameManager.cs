using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int points;
    private float health = 1;
    private float shield = 1;
    private bool shieldActive = false;
    private bool shieldFullCooldown = false;

    private bool pointsUpdated = false;
    private bool healthUpdated = false;
    private bool shieldUpdated = false;


    private float spawnTimer = 5f;
    [SerializeField]
    private Vector2 timerRange = new Vector2(2, 15);
    [SerializeField]
    private GameObject enemyPrefab;


    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        { 
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    //Getters and setters

    public int Points
    {
        get { return points; }
        set { points = value; pointsUpdated = true; }
    }

    public float Health
    {
        get { return health * 100f; } 
        set
        {
            health = Mathf.Clamp01(value / 100f); 
            healthUpdated = true;
        }
    }

    public float Shield
    {
        get { return shield * 100f; } 
        set
        {
            shield = Mathf.Clamp01(value / 100f); 
            shieldUpdated = true;
        }
    }

    

    public bool PointsUpdated
    {
        get { return pointsUpdated; }
        set { pointsUpdated = value; }
    }

    public bool HealthUpdated
    {
        get { return healthUpdated; }
        set { healthUpdated = value; }
    }
    
    public bool ShieldUpdated
    {
        get { return shieldUpdated; }
        set { shieldUpdated = value; }
    }

    public bool ShieldActive
    {
        get { return shieldActive; }
        set { shieldActive = value; }
    }

    public bool ShieldFullCooldown
    {
        get { return shieldFullCooldown; }
        set { shieldFullCooldown = value; }
    }

    void Update()
    {
        if (spawnTimer <= 0)
        {
            GameObject obj = Instantiate(enemyPrefab);
            spawnTimer = UnityEngine.Random.Range(timerRange.x, timerRange.y);
        } else
        {
            spawnTimer -= Time.deltaTime;
        }
    }
}
