using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{   
    
    [NonSerialized]
    public float speed;
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        
        transform.position = new Vector3(transform.position.x - (Time.deltaTime * speed), 0, transform.position.z);
        if (transform.position.x > 15)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
       if (collision.transform.CompareTag("BulletHB"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
    }
}
