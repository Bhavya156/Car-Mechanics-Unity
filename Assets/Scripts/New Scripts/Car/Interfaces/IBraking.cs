using UnityEngine;

public interface IBraking
{
    void ApplyBrakes(bool isBraking, WheelCollider[] wheelColliders);
}

public class ApplyBrakes : IBraking
{
    void IBraking.ApplyBrakes(bool isBraking, WheelCollider[] wheelColliders)
    {
        float brakeForce = isBraking ? 1205 : 0;
        wheelColliders[2].brakeTorque = brakeForce;
        wheelColliders[3].brakeTorque = brakeForce;
    }
}