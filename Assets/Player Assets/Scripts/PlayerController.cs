using UnityEngine;
using UnityEngine.InputSystem;

//Todo: make player's hitbox a cube since a plane has no height, 
//and put a texture on the plane instead
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 10.5f;
    [SerializeField]
    private float playerSpeed = 15f;
    [SerializeField]
    private float bulletSpeed = 3f;
    public float bulletSpeedMultiplier = 1f;
    [SerializeField]
    private float shieldFullCDMod = 0.5f;
    [SerializeField]
    private float shieldMod = 0.5f;
    private float resetTimer, waitTimer = 1;

    public GameObject BulletPrefab;
    public GameObject BulletContainer;


    private GameObject other;
    private Camera camera;

    [SerializeField]
    private float bulletCD = .5f;
    private float bulletCDinitial;


    private InputActionAsset inputActions;
    private Rigidbody rb;

    private InputAction leftRight;
    private InputAction upDown;
    private InputAction fire;
    private InputAction shield;
    
    void Start()
    {   
        bulletCDinitial = bulletCD;
        //Get the input actions from the component so i don't have to import it twice
        inputActions = GetComponent<PlayerInput>().actions;

        leftRight = inputActions["LeftRight"];
        upDown = inputActions["UpDown"];
        fire = inputActions["Fire"];
        shield = inputActions["Shield"];

        rb = GetComponent<Rigidbody>();

        camera = Camera.main;
        other = transform.GetChild(1).gameObject;
        other.SetActive(false);
        
    }

    //Called in Update
    void HandleShield()
    {
        bool shieldPressed = shield.IsPressed();
        var gameManager = GameManager.Instance;

        gameManager.ShieldActive = false; //default

        //If pressing shield and not in full cooldown
        if (shieldPressed && !gameManager.ShieldFullCooldown)
        {
            resetTimer = 1f; // reset usage delay

            if (gameManager.Shield > 0f)
            {
                //Use shield
                gameManager.ShieldActive = true;
                gameManager.Shield -= Time.deltaTime * shieldMod;
                gameManager.Shield = Mathf.Max(gameManager.Shield, 0f);

                //If shield just hit 0, start full cooldown
                if (gameManager.Shield <= 0f)
                {
                    gameManager.ShieldFullCooldown = true;
                    gameManager.ShieldActive = false;
                    waitTimer = 0.5f; //pause before recharging
                }
            }
        }
        else
        {
            //Recharge shield
            if (waitTimer > 0f)
            {
                waitTimer -= Time.deltaTime;
                return; //wait before starting recharge
            }

            float rechargeRate = gameManager.ShieldFullCooldown ? shieldFullCDMod : shieldMod;

            gameManager.Shield += Time.deltaTime * rechargeRate;

            if (gameManager.Shield >= 100f)
            {
                gameManager.Shield = 100f;
                gameManager.ShieldFullCooldown = false;
            }
        }
    }

    //Recharge immediately (full cooldown mode)
    //Unused
    void RechargeShield(GameManager gameManager, float rate)
    {
        gameManager.Shield += Time.deltaTime * rate;
        if (gameManager.Shield >= 100f)
        {
            gameManager.Shield = 100f;
            gameManager.ShieldFullCooldown = false;
        }
    }

    //Recharge after reset timer
    //Unused currently
    void RechargeShieldWithDelay(GameManager gameManager, float rate)
    {
        if (resetTimer > 0f)
        {
            resetTimer -= Time.deltaTime;
            return;
        }

        resetTimer = 0f;
        gameManager.Shield += Time.deltaTime * rate;

        if (gameManager.Shield >= 100f)
        {
            gameManager.Shield = 100f;
            gameManager.ShieldFullCooldown = false;
        }
    }
    
    void Update()
    {   
        if (GameManager.Instance.gameRunning)
        {
            //Handle inputs
            float x = leftRight.ReadValue<float>();
            float y = upDown.ReadValue<float>();
            HandleShield();

            //Create Bullet(s)
            if (fire.WasPressedThisFrame() && bulletCD <= 0f)
            {
                bulletCD = bulletCDinitial;
                GameObject pref = Instantiate(BulletPrefab, BulletContainer.transform);
                pref.transform.position = transform.GetChild(0).GetChild(1).transform.position;
                pref.GetComponent<Bullet>().MainParent = gameObject;
                pref.GetComponent<Bullet>().speed = bulletSpeed;
                pref.GetComponent<ParticleSystem>().Clear();
                Physics.IgnoreCollision(transform.GetChild(0).GetChild(0).GetComponent<Collider>(), pref.GetComponent<Collider>());
                Physics.IgnoreCollision(transform.GetChild(1).GetChild(0).GetComponent<Collider>(), pref.GetComponent<Collider>());

                //Create a duplicate bullet at the offscreen player if its active
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
            
            //Add velocity
            rb.AddForce(new Vector3(x*2, 0, y*2) * Time.deltaTime * (playerSpeed * 10));

            //X Bounds
            if (transform.position.x < -8.35f || transform.position.x > 8.35f)
            {
                rb.linearVelocity = new Vector3(-rb.linearVelocity.x, 0, rb.linearVelocity.z);

                if (transform.position.x > 0)
                {
                    transform.position = new Vector3(8.35f, 1, transform.position.z);
                } else
                {
                    transform.position = new Vector3(-8.35f, 1, transform.position.z);
                }
            }
            
            //Y Bounds
            if (transform.position.z > 0)
            {
                if (transform.position.z > 4)
                {
                    other.SetActive(true);
                } else
                {
                    other.SetActive(false);
                }
                other.transform.position = new Vector3(transform.position.x, 1, transform.position.z - 10f);
                //other.transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, 0, transform.eulerAngles.z);
            } else
            {
                if (transform.position.z < -4)
                {
                    other.SetActive(true);
                } else
                {
                    other.SetActive(false);
                }
                other.transform.position = new Vector3(transform.position.x, 1, transform.position.z + 10f);
                //other.transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, 0, transform.eulerAngles.z);
            }
            if (GameManager.Instance.Health <= 5)
            {
                GameManager.Instance.SpawnPlayerDeathEffect(transform.position);
                GameManager.Instance.gameRunning = false;
                Destroy(gameObject);
            }
        }
        

        

    }

    void FixedUpdate()
    {   
        //Loop on Y
        if (transform.position.z < -5f)
        {
            transform.position = new Vector3(transform.position.x, 1, 5f);
        } else if (transform.position.z > 5f)
        {
            transform.position = new Vector3(transform.position.x, 1, -5f);
        }

        if (bulletCD > 0)
        {
            bulletCD -= Time.deltaTime;
        } else
        {
            bulletCD = 0;
        }
    }

    
}
