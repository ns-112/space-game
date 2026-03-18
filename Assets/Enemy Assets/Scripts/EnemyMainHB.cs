using UnityEngine;

public class EnemyMainHB : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MainPlayer"))
        {
            if (GameManager.Instance.ShieldActive)
            {
                GameManager.Instance.Shield -= 20;
                if (GameManager.Instance.Shield < 0)
                {
                    GameManager.Instance.Health += GameManager.Instance.Shield / 2;
                }
            } else
            {
                GameManager.Instance.Health -= 10;
            }
            
            Destroy(transform.parent.gameObject);
            
        }
        
    }
}
