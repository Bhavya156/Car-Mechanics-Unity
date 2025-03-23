using UnityEngine;

public interface ISteering
{
    void Steer(float input, WheelCollider[] wheelColliders);
}

public class CarSteering : ISteering
{
    private Rigidbody carRb;

    public CarSteering(Rigidbody rb)
    {
        carRb = rb;
    }

    public void Steer(float input, WheelCollider[] wheelColliders)
    {
        float speed = carRb.linearVelocity.magnitude * 3.6f; // Convert to km/h
        float radius = 6 + (speed / 20); // Example: Adjust turn radius dynamically

        float steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * input;
        wheelColliders[0].steerAngle = steerAngle;
        wheelColliders[1].steerAngle = steerAngle;
    }
}