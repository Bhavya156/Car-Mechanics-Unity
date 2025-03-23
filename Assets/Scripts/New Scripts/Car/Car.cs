using UnityEngine;

public class Car : MonoBehaviour, IVehicle
{
    [SerializeField]
    private string m_CarName = "Vehicle 1";
    [SerializeField] private Rigidbody carRb;
    [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] wheelMeshes = new Transform[4];
    [SerializeField] private InputManager inputManager;

    private IEngine m_engine;
    private ITransmission m_transmission;
    private ISteering m_steering;
    private IBraking m_braking;
    public string VehicleName {get => m_CarName; set => m_CarName = value;}

    private void FixedUpdate()
    {
        float acceleration = Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");
        bool isBraking = Input.GetKey(KeyCode.Space);

        m_engine.CalculateEnginePower(acceleration);
        m_transmission.Shifter();
        m_steering.Steer(steeringInput, wheelColliders);
        m_braking.ApplyBrakes(isBraking, wheelColliders);
        RotateWheelTransform();
    }

    private void RotateWheelTransform()
    {
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetWorldPose(out Vector3 pos, out Quaternion quat);
            wheelMeshes[i].position = pos;
            wheelMeshes[i].rotation = quat;
        }
    }

    public void Initialize(Rigidbody rb, WheelCollider[] colliders, Transform[] meshes, IEngine engine, ITransmission transmission, ISteering steering, IBraking braking)
    {
        this.carRb = rb;
        this.wheelColliders = colliders;
        this.wheelMeshes = meshes;
        this.m_engine = engine;
        this.m_transmission = transmission;
        this.m_steering = steering;
        this.m_braking = braking;

        gameObject.name = m_CarName;
    }
}
