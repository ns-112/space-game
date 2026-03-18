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
            GameManager.Instance.Points += 10;
            Destroy(transform.parent.gameObject);
            
        }
    }
}
