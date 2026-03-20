using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    //list of effects (particles) is auto deleted by GameManager
    [SerializeField]
    private List<GameObject> effects;


    private Vector3Int gameTime = new Vector3Int(0, 0, 0);
    private float gameTimeIncrementor = 0;
    private int points;
    private float health = 1;
    private float shield = 1;
    private float smoothShield = 1;
    private bool shieldActive = false;
    private bool shieldFullCooldown = false;

    private bool pointsUpdated = false;
    private bool healthUpdated = false;
    private bool shieldUpdated = false;

    public bool gameRunning = true;


    private float enemySpawnTimer = 5f;
    [SerializeField]
    private Vector2 timerRange = new Vector2(2, 15);
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemyExplosionPrefab;
    [SerializeField]
    private GameObject playerHitPrefab;
    [SerializeField]
    private GameObject playerDeathPrefab;

    public enum EnemyType
    {
        Default,
        Wave
    };

    public void SpawnEnemyDeathEffect(Vector3 position)
    {
        GameObject obj = Instantiate(enemyExplosionPrefab, GameObject.Find("[Effect] EffectsContainer").transform);
        obj.transform.position = position;
        obj.GetComponent<ParticleSystem>().Play();
        effects.Add(obj);
        
    }

    public void SpawnPlayerDeathEffect(Vector3 position)
    {
        GameObject obj = Instantiate(playerDeathPrefab, GameObject.Find("[Effect] EffectsContainer").transform);
        obj.transform.position = position;
        obj.GetComponent<ParticleSystem>().Play();
        effects.Add(obj);
        
    }

    public void SpawnPlayerHitEffect(Vector3 position)
    {
        GameObject obj = Instantiate(playerHitPrefab, GameObject.Find("[Effect] EffectsContainer").transform);
        obj.transform.position = position;
        obj.GetComponent<ParticleSystem>().Play();
        effects.Add(obj);
        
    }

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

   //Returns a Vector of 3 ints (hours, minutes, seconds)
    public Vector3Int getGameTime()
    {
        return gameTime;
    }

    public float SmoothShield
    {
        get { return smoothShield; }
        set { smoothShield = value; }
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

    void UpdateGameTime()
    {
        gameTimeIncrementor += Time.deltaTime;
        gameTime.z = (int)gameTimeIncrementor;
        //seconds
        if (gameTime.z == 60)
        {
            gameTime.y += 1;
            gameTime.z = 0;
        }
        //minutes
        if (gameTime.y == 60)
        {
            gameTime.x += 1;
            gameTime.y = 0;
        }
        //hours
        if (gameTime.x >= 60)
        {
            Debug.Log("???");
        }
    }

    void Update()
    {   if (gameRunning)
        {
            UpdateGameTime();
            if (enemySpawnTimer <= 0)
            {
                Instantiate(enemyPrefab);
                enemySpawnTimer = UnityEngine.Random.Range(timerRange.x, timerRange.y);
            } else
            {
                enemySpawnTimer -= Time.deltaTime;
            }

        }
        
        for (int i = effects.Count - 1; i >= 0; i--)
        {   
            //i like the c++ based checks more

            //Check effect type & update accordingly

            //Particle Bursts
            if (effects[i].TryGetComponent(out ParticleSystem sys))
            {
                if (sys.isStopped)
                {
                    Destroy(effects[i]);
                    effects.RemoveAt(i);
                }
            }
            
            
        }
    }
}
