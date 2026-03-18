using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //BulletHB is for the bullet and EnemyHB is for the player
    //to make it feel more fair, these hitboxes are seperate & differently sized
    private Vector2 spawnPosition;
    [SerializeField]
    private float enemySpeed = 10;
    private Transform MainParent;
    
    void Start()
    {
        spawnPosition = new Vector2(9.5f, UnityEngine.Random.Range(-5f, 5f));
        transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.y);
        transform.parent = GameObject.Find("[Main] EnemyContainer").transform;
    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x - (Time.deltaTime * enemySpeed), 0, spawnPosition.y);
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }

}
