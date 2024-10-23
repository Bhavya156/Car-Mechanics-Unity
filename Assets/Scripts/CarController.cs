using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class WheelMeshes
{
    public MeshRenderer frontLeftWheelMesh;
    public MeshRenderer frontRightWheelMesh;
    public MeshRenderer rearLeftWheelMesh;
    public MeshRenderer rearRightWheelMesh;
}

[Serializable]
public class WheelColliders
{
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
}

public class CarController : MonoBehaviour
{
    private Rigidbody _playerRb;
    public WheelMeshes wheelMeshes;
    public WheelColliders wheelColliders;
    public float acceleration;
    public float turn;
    public float brake;
    public float enginePower;
    public float brakePower;
    public float slipAngle;
    private float velocity;
    public AnimationCurve animationCurve;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = _playerRb.linearVelocity.magnitude;
        PlayerInput();
        ApplyTorque();
        Steering();
        Braking();
        ApplyWheelMotion();
    }

    private void PlayerInput()
    {
        acceleration = Input.GetAxis("Vertical");
        turn = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, _playerRb.linearVelocity - transform.forward);
        if (slipAngle < 120f)
        {
            if (acceleration < 0)
            {
                brake = Mathf.Abs(acceleration);
                acceleration = 0;
            }
        }
        else
        {
            brake = 0;
        }
    }

    private void ApplyTorque()
    {
        wheelColliders.rearLeftWheelCollider.motorTorque = enginePower * acceleration;
        wheelColliders.rearRightWheelCollider.motorTorque = enginePower * acceleration;
    }

    private void Steering()
    {
        float turnValue = turn * animationCurve.Evaluate(velocity);
        wheelColliders.frontLeftWheelCollider.steerAngle = turnValue;
        wheelColliders.frontRightWheelCollider.steerAngle = turnValue;
    }

    private void Braking()
    {
        wheelColliders.frontLeftWheelCollider.brakeTorque = brake * brakePower * 0.7f;
        wheelColliders.frontRightWheelCollider.brakeTorque = brake * brakePower * 0.7f;

        wheelColliders.rearLeftWheelCollider.brakeTorque = brake * brakePower * 0.3f;
        wheelColliders.rearRightWheelCollider.brakeTorque = brake * brakePower * 0.3f;
    }

    private void ApplyWheelMotion()
    {
        UpdateWheel(wheelColliders.frontLeftWheelCollider, wheelMeshes.frontLeftWheelMesh);
        UpdateWheel(wheelColliders.frontRightWheelCollider, wheelMeshes.frontRightWheelMesh);
        UpdateWheel(wheelColliders.rearLeftWheelCollider, wheelMeshes.rearLeftWheelMesh);
        UpdateWheel(wheelColliders.rearRightWheelCollider, wheelMeshes.rearRightWheelMesh);
    }

    private void UpdateWheel(WheelCollider wheelCollider, MeshRenderer meshRenderer)
    {
        Quaternion quaternion;
        Vector3 position;

        wheelCollider.GetWorldPose(out position, out quaternion);
        meshRenderer.transform.position = position;
        meshRenderer.transform.rotation = quaternion;
    }
}
