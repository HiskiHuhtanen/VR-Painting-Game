using UnityEngine;

public class SpeedLever : MonoBehaviour
{
    [Header("Components")]
    public HingeJoint hinge;
    public Transform ship;
    
    [Header("Speed Settings")]
    public float maxSpeed = 10f;
    
    [Header("Lever Angle Limits")]
    public float minAngle = -30f;
    public float maxAngle = 30f;
    
    [Header("Smoothing")]
    public bool useSmoothing = true;
    public float accelerationSmoothing = 5f;
    
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    void Start()
    {
        if (hinge == null)
            hinge = GetComponent<HingeJoint>();
            
        if (hinge == null)
            Debug.LogError("SpeedLever: No HingeJoint found! Please assign one in the inspector.");
            
        if (ship == null)
            Debug.LogError("SpeedLever: No ship Transform assigned! Please assign one in the inspector.");
    }

    void Update()
    {
        if (hinge == null || ship == null)
            return;
            
        float currentAngle = hinge.angle;
        
        // Map angle to throttle value (0 to 1)
        // Clamp the angle to our defined range and normalize it
        float throttle = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
        
        // Calculate target speed based on throttle
        targetSpeed = throttle * maxSpeed;
        
        // Apply smooth acceleration if enabled
        if (useSmoothing)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, accelerationSmoothing * Time.deltaTime);
        }
        else
        {
            currentSpeed = targetSpeed;
        }
        
        // Debug prints to show current values
        Debug.Log($"Lever Angle: {currentAngle:F2}° | Throttle: {throttle:F2} | Target Speed: {targetSpeed:F2} | Current Speed: {currentSpeed:F2}");
        
        // Move the ship forward along its local Z-axis
        Vector3 movement = ship.forward * currentSpeed * Time.deltaTime;
        ship.position += movement;
    }
    
    // Optional: Get current throttle percentage for UI or debugging
    public float GetThrottlePercentage()
    {
        if (hinge == null) return 0f;
        return Mathf.InverseLerp(minAngle, maxAngle, hinge.angle);
    }
    
    // Optional: Get current speed for UI or debugging
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
