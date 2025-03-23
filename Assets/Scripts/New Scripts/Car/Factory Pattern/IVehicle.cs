using UnityEngine;

public interface IVehicle
{
    public string VehicleName {get; set;}

    public void Initialize(Rigidbody rb, WheelCollider[] colliders, Transform[] meshes, IEngine engine, ITransmission transmission, ISteering steering, IBraking braking);
}