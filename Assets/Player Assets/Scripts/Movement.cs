using UnityEngine;
using UnityEngine.InputSystem;

//Todo: make player's hitbox a cube since a plane has no height, 
//and put a texture on the plane instead
public class Movement : MonoBehaviour
{
    
    
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
        rb.AddForce(new Vector3(x, 0, y));
        Debug.Log(movement);
    }
}
