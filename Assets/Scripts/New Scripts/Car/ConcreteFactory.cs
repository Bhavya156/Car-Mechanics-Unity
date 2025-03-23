using UnityEngine;

public class ConcreteFactory : Factory
{
    [SerializeField]
    private Car m_VehiclePrefab;
    [SerializeField]
    private CarConfig carConfig;
    [SerializeField]
    private InputManager inputManager;

    public override IVehicle GetVehicle(Transform spawnPoint)
    {
        GameObject instance = Instantiate(carConfig.carPrefab, spawnPoint.position, spawnPoint.rotation);
        Car newVehicle = instance.GetComponent<Car>();

        Rigidbody rb = instance.GetComponent<Rigidbody>();

        GameObject collidersObj = instance.transform.Find("Wheels/Colliders").gameObject;
        GameObject meshesObj = instance.transform.Find("Wheels/Meshes").gameObject;

        WheelCollider[] wheelColliders = new WheelCollider[4]
        {
            collidersObj.transform.Find("FrontLeftWheelCollider").GetComponent<WheelCollider>(),
            collidersObj.transform.Find("FrontRightWheelCollider").GetComponent<WheelCollider>(),
            collidersObj.transform.Find("RearLeftWheelCollider").GetComponent<WheelCollider>(),
            collidersObj.transform.Find("RearRightWheelCollider").GetComponent<WheelCollider>()
        };

        Transform[] wheelMeshes = new Transform[4]
        {
            meshesObj.transform.Find("FrontLeftWheel"),
            meshesObj.transform.Find("FrontRightWheel"),
            meshesObj.transform.Find("RearLeftWheel"),
            meshesObj.transform.Find("RearRightWheel")
        };

        IEngine engine = new Engine(rb, wheelColliders, carConfig.engineCurve, carConfig.gearDifferentialRatios, carConfig.maxRPM, carConfig.minRPM, carConfig.topSpeed);
        ITransmission transmission = new CarTransmission(carConfig.gearDifferentialRatios, carConfig.maxRPM, carConfig.minRPM);
        ISteering steering = new CarSteering(rb);
        IBraking braking = new ApplyBrakes();

        newVehicle.Initialize(rb, wheelColliders, wheelMeshes, engine, transmission, steering, braking);

        return newVehicle;
    }
}
