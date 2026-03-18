using UnityEngine;

public class PlayerLoop : MonoBehaviour
{
    private GameObject other;
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
        other = transform.GetChild(1).gameObject;
        other.SetActive(false);
    }

    
    void Update()
    {   
        if (transform.position.z > 0)
        {
            if (transform.position.z > 4)
            {
                other.SetActive(true);
            } else
            {
                other.SetActive(false);
            }
            other.transform.position = new Vector3(transform.position.x, 0, transform.position.z - 10f);
            other.transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, 0, transform.eulerAngles.z);
        } else
        {
            if (transform.position.z < -4)
            {
                other.SetActive(true);
            } else
            {
                other.SetActive(false);
            }
            other.transform.position = new Vector3(transform.position.x, 0, transform.position.z + 10f);
            other.transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, 0, transform.eulerAngles.z);
        }
        
    }
}
