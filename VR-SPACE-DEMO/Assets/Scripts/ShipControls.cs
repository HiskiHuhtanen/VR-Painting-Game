using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class ShipControls : MonoBehaviour
{
    [Header("Ship Components")]
    public Rigidbody ship;
    public XRJoystick joystick;
    public XRLever rollLever;

    [Header("Control Settings")]
    public float rotationSpeed = 50f;
    public float pitchSpeed = 30f;

    private Vector2 joystickInput;


    void Start()
    {
        if (joystick != null)
        {
            joystick.onValueChangeX.AddListener(OnJoystickXChange);
            joystick.onValueChangeY.AddListener(OnJoystickYChange);
        }
    }

    void FixedUpdate()
    {
        ApplyRotation();
        
    }

    void OnJoystickXChange(float value)
    {
        joystickInput.x = value;
    }

    void OnJoystickYChange(float value)
    {
        joystickInput.y = value;
    }

    void ApplyRotation()
    {
        // Direct rotation - simple and effective
        float yaw = joystickInput.x * rotationSpeed * Time.fixedDeltaTime;
        float pitch = joystickInput.y * pitchSpeed * Time.fixedDeltaTime;

        // Apply rotation directly
        ship.transform.Rotate(pitch, yaw, 0, Space.Self);
        
    }
}
