using UnityEngine;

public class ShipRotation : MonoBehaviour
{
    [Header("Ship Components")]
    public Transform ship;      // The ship to rotate
    public Transform lever;     // The lever that controls rotation
    public float minAngle = -30f; // Lever angle min
    public float maxAngle = 30f;  // Lever angle max
    public float rotationSpeed = 50f;

    void Update()
    {
        // Get lever angle
        float angle = lever.localEulerAngles.x;
        if (angle > 180f) angle -= 360f;

        // Map lever angle to -1..1
        float normalized = Mathf.InverseLerp(minAngle, maxAngle, angle) * 2f - 1f;

        // Apply rotation around ship's local Y-axis (yaw)
        ship.Rotate(0f, normalized * rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
