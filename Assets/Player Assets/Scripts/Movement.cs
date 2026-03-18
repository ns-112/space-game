using UnityEngine;
using UnityEngine.InputSystem;

//Todo: make player's hitbox a cube since a plane has no height, 
//and put a texture on the plane instead
public class Movement : MonoBehaviour
{
    
    public float rotationSpeed = 10.5f;
    [SerializeField]
    private float bulletSpeed = 3f;
    public float bulletSpeedMultiplier = 1f;

    public GameObject BulletPrefab;
    public GameObject BulletContainer;


    private GameObject other;
    private Camera camera;




    private InputActionAsset inputActions;
    private Rigidbody rb;

    private InputAction leftRight;
    private InputAction upDown;
    private InputAction fire;
    
    void Start()
    {   
        //Get the input actions from the component so i don't have to import it twice
        inputActions = GetComponent<PlayerInput>().actions;

        leftRight = inputActions["LeftRight"];
        upDown = inputActions["UpDown"];
        fire = inputActions["Fire"];

        rb = GetComponent<Rigidbody>();

        camera = Camera.main;
        other = transform.GetChild(1).gameObject;
        other.SetActive(false);
        
    }

 
    
    void Update()
    {
        float x = leftRight.ReadValue<float>();
        float y = upDown.ReadValue<float>();

        //Vector2 movement = new Vector2(x, y);
        if (fire.WasPressedThisFrame())
        {
            GameObject pref = Instantiate(BulletPrefab, BulletContainer.transform);
            pref.transform.position = transform.GetChild(0).GetChild(1).transform.position;
            pref.GetComponent<Bullet>().MainParent = gameObject;
            pref.GetComponent<Bullet>().speed = bulletSpeed;
            pref.GetComponent<ParticleSystem>().Clear();
            Physics.IgnoreCollision(transform.GetChild(0).GetChild(0).GetComponent<Collider>(), pref.GetComponent<Collider>());
            Physics.IgnoreCollision(transform.GetChild(1).GetChild(0).GetComponent<Collider>(), pref.GetComponent<Collider>());
            if (other.activeSelf)
            {
                GameObject pref2 = Instantiate(BulletPrefab, BulletContainer.transform);
                pref2.transform.position = transform.GetChild(1).GetChild(1).transform.position;
                pref2.GetComponent<Bullet>().MainParent = gameObject;
                pref2.GetComponent<Bullet>().speed = bulletSpeed;
                pref2.GetComponent<ParticleSystem>().Clear();
                Physics.IgnoreCollision(transform.GetChild(0).GetChild(0).GetComponent<Collider>(), pref2.GetComponent<Collider>());
                Physics.IgnoreCollision(transform.GetChild(1).GetChild(0).GetComponent<Collider>(), pref2.GetComponent<Collider>());

                
                
            }
            
        }


        //Scrapped smooth rotate to mouse: jittered when going offscreen

        /*
        Vector2 mouse = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mouse);
        Plane plane = new(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 mousePos = ray.GetPoint(distance);

            //convert from screen to world pos
            Vector3 targetDirection = mousePos - transform.position;
            targetDirection.y = 0; //ignore y

            if (targetDirection.sqrMagnitude > 0.001f) // avoid zero-length vectors
            {
                //target rotation
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                //Smooth lerp towards mouse pos
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        */
        
        rb.AddForce(new Vector3(x*2, 0, y*2));
        if (transform.position.x < -8.35f || transform.position.x > 8.35f)
        {
            rb.linearVelocity = new Vector3(-rb.linearVelocity.x, 0, rb.linearVelocity.z);

            if (transform.position.x > 0)
            {
                transform.position = new Vector3(8.35f, 0, transform.position.z);
            } else
            {
                transform.position = new Vector3(-8.35f, 0, transform.position.z);
            }
        }
        

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

    void FixedUpdate()
    {
        if (transform.position.z < -5f)
        {
            transform.position = new Vector3(transform.position.x, 0, 5f);
        } else if (transform.position.z > 5f)
        {
            transform.position = new Vector3(transform.position.x, 0, -5f);
        }
    }
}
