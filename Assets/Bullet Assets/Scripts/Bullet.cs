using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    [NonSerialized]
    public GameObject MainParent;
    [NonSerialized]
    public float speed;
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!MainParent)
        {   
            Debug.LogWarning("No parent added to child: " + name);
            Destroy(gameObject);
        }
        transform.position = new Vector3(transform.position.x + (Time.deltaTime * (speed * MainParent.GetComponent<PlayerController>().bulletSpeedMultiplier)), 0, transform.position.z);
        if (transform.position.x > 15)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //If we hit Boss shield destroy bullet ONLY
        if (collision.gameObject.GetComponent<BossShield>() != null)
        {
            Destroy(gameObject);
            return;
        }

        //If we hit the boss, damage it
        BossTemplate boss = collision.gameObject.GetComponent<BossTemplate>();
        if (boss != null)
        {
            boss.TakeHit();
            Destroy(gameObject);
            return;
        }

        // Ignore bullet hitboxes
        if (collision.transform.CompareTag("BulletHB"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }
    }
}
