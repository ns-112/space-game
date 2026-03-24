using UnityEngine;

public class EnemyBullletHB : MonoBehaviour
{
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("MainPlayer").transform.GetChild(0).GetChild(0).GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("MainPlayer").transform.GetChild(1).GetChild(0).GetComponent<Collider>());
    }
    void OnCollisionEnter(Collision collision)
    {
        
        
        
        if (collision.transform.CompareTag("BulletHB"))
        {
            Destroy(collision.gameObject);
            GameManager.Instance.enemySpeedMod += 0.02f;
            GameManager.Instance.enemySpawnMod += 0.13f;
            GameManager.Instance.Points += 10;
            GameManager.Instance.SpawnEnemyDeathEffect(transform.parent.position);
            Destroy(transform.parent.gameObject);
            
        }
    }
}
