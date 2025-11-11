using UnityEngine;

public class ShipSpeed : MonoBehaviour
{
    public Rigidbody ship;
    public Transform lever;
    public float minAngle = -30f;
    public float maxAngle = 30f;
    public float maxSpeed = 20f;
    public float acceleration = 5f;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    // Update is called once per frame
    void Update()
    {
        float angle = lever.localEulerAngles.x;
        if (angle > 180f) angle -= 360f;
        float speed = Mathf.InverseLerp(minAngle, maxAngle, angle);
        targetSpeed = speed * maxSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime); //accel
        ship.MovePosition(ship.position + ship.transform.forward * currentSpeed * Time.deltaTime);
    }

    public float getCurrentSpeed() => currentSpeed;
}
