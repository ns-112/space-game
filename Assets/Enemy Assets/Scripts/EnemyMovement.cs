using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //BulletHB is for the bullet and EnemyHB is for the player
    //to make it feel more fair, these hitboxes are seperate & differently sized
    private Vector2 spawnPosition;
    [SerializeField]
    private float enemySpeed = 10;
    private float genericMultiplier = 1;
    private Transform MainParent;
    private GameManager.EnemyType enemyType = GameManager.EnemyType.Default;
    
    void Start()
    {
        spawnPosition = new Vector2(9.5f, UnityEngine.Random.Range(-5f, 5f));
        transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        transform.parent = GameObject.Find("[Main] EnemyContainer").transform;
        ChooseEnemyType();
    }

    void ChooseEnemyType()
    {
        float chance = UnityEngine.Random.Range(0, 100);
        if (chance > 50)
        {
            enemyType = GameManager.EnemyType.Default;
        } else if (chance > 25)
        {
            enemyType = GameManager.EnemyType.Wave;
            if ((int)UnityEngine.Random.Range(0, 1) == 1)
                genericMultiplier = -1;
            
        }
    }

    
    void Update()
    {
        switch (enemyType)
        {   
            //Normal Enemy Movement
            case GameManager.EnemyType.Default:
            transform.position = new Vector3(transform.position.x - (Time.deltaTime * enemySpeed), 0, spawnPosition.y);
            break;
            case GameManager.EnemyType.Wave:
            transform.position = new Vector3(transform.position.x - (Time.deltaTime * enemySpeed), 0, spawnPosition.y + Mathf.Sin(Time.time * 2) * 2 * genericMultiplier);
            break;
        }
        
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }

}
