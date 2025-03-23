using UnityEngine;

public interface IEngine
{
    void CalculateEnginePower(float acceleration);
}

public class Engine : IEngine
{
    private float totalPower;
    private float engineRPM;
    private float wheelRpm;
    private AnimationCurve engineCurve;
    private float[] gears;
    private int gearNum = 0;
    private float maxRPM, minRPM;
    private float topSpeed;

    private Rigidbody carRb;
    private WheelCollider[] wheelColliders;

    public Engine(Rigidbody rb, WheelCollider[] colliders, AnimationCurve curve, float[] gearRatios, float maxRpm, float minRpm, float speed)
    {
        carRb = rb;
        wheelColliders = colliders;
        engineCurve = curve;
        gears = gearRatios;
        maxRPM = maxRpm;
        minRPM = minRpm;
        topSpeed = speed;
    }

    public void CalculateEnginePower(float acceleration)
    {
        float velocity = 0.0f;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelRpm) * 3.6f * gears[gearNum]), ref velocity, 0.1f);
        totalPower = engineCurve.Evaluate(engineRPM) * gears[gearNum] * acceleration;

        ApplyTorque();
    }

    private void ApplyTorque()
    {
        float power = totalPower / 2;
        for (int i = 2; i < 4; i++) // Rear wheels
        {
            wheelColliders[i].motorTorque = power;
        }
    }
}