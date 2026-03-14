using UnityEngine;
using UnityEngine.InputSystem;

//Todo: make player's hitbox a cube since a plane has no height, 
//and put a texture on the plane instead
public class Movement : MonoBehaviour
{
    
    public float rotationSpeed = 10.5f;

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
        
    }

    
    void Update()
    {
        float x = leftRight.ReadValue<float>();
        float y = upDown.ReadValue<float>();

        Vector2 movement = new Vector2(x, y);
        if (fire.WasPressedThisFrame())
                {
                    Debug.Log("Fire pressed");
                }


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
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,   
                    targetRotation,          
                    rotationSpeed * Time.deltaTime 
                );
            }
        }

        
        rb.AddForce(new Vector3(x*2, 0, y*2));
        Debug.Log(movement);
    }
}
