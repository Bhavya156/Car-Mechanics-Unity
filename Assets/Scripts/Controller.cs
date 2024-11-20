using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    private InputManager inputManager;

    private enum DriveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [SerializeField] private DriveType driveType;

    private enum GearBox
    {
        automatic,
        manual,
    }
    [SerializeField] private GearBox gearbox;

    public GameManager gameManager;

    [Header("Wheels")]
    private WheelCollider[] wheelColliders = new WheelCollider[4];
    private Transform[] wheelMeshes = new Transform[4];
    private GameObject colliders, meshes;

    [Header("Acceleration")]
    public int motorTorque;
    public float brakeTorque;
    public float nitrousPower;
    public AnimationCurve engineCurve;
    public float wheelRpm;
    public float totalPower;
    public float engineRPM;
    public float[] gears;
    public int gearNum = 0;
    public float maxRPM, minRPM;
    public bool reverse;

    [Header("Steering")]
    public float steerAngle;
    public float radius;

    [Header("Drift")]
    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    public float handBrakeFriction;
    public float handBrakeFrictionMultiplier = 2f;
    public float smoothTime;

    [Header("Speedometer")]
    public float KPH;
    public float downForce;
    public float[] slip = new float[4];

    [Header("Effects")]
    public ParticleSystem[] nitroSmoke;
    public float nitrousValue;
    public bool nitrousFlag;

    private Rigidbody carRb;


    private void Awake()
    {
        StartCoroutine(TimedLoop());
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetObjects();
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "AwakeScene") return;
        RotateWheelTransform();
        CalculateEnginePower();
        Steering();
        AddDownForce();
        DriftCar();
        ActivateNitrous();
        //GetFriction();
    }

    private void Movement()
    {
        brakeVehicle();

        float power = totalPower / (driveType == DriveType.allWheelDrive ? 4 : 2);

        if (driveType == DriveType.frontWheelDrive)
        {
            for (int i = 0; i < 2; i++)  // Front wheels
            {
                wheelColliders[i].motorTorque = power;
            }
        }
        else if (driveType == DriveType.rearWheelDrive)
        {
            for (int i = 2; i < 4; i++)  // Rear wheels
            {
                wheelColliders[i].motorTorque = power;
            }
        }
        else // All-wheel drive
        {
            for (int i = 0; i < 4; i++)  // All wheels
            {
                wheelColliders[i].motorTorque = power;
            }
        }

        KPH = carRb.linearVelocity.magnitude * 3.6f;  // To calculate speed in km/h

        ////Nitrous
        //if (inputManager.nitrous)
        //{
        //    carRb.AddForce(transform.forward * 5000);
        //}
    }

    private void CalculateEnginePower()
    {
        WheelRPM();

        totalPower = engineCurve.Evaluate(engineRPM) * (gears[gearNum]) * inputManager.verticalInput;
        float velocity = 0.0f;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelRpm) * 3.6f * (gears[gearNum])), ref velocity, smoothTime);
        Movement();
        Shifter();
    }

    private void WheelRPM()
    {
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheelColliders[i].rpm;
            R++;
        }
        wheelRpm = (R != 0) ? sum / R : 0;
        //Debug.Log("Wheel rpm is :: " + wheelRpm);

        if (wheelRpm < 0 && !reverse)
        {
            reverse = true;
            gameManager.ChangeGear();
        }
        else if (wheelRpm > 0 && reverse)
        {
            reverse = false;
            gameManager.ChangeGear();
        }
    }

    private void Shifter()
    {
        if (!isGrounded()) return;

        if (gearbox == GearBox.automatic)
        {
            if (engineRPM > maxRPM && gearNum < gears.Length - 1 && !reverse)
            {
                Debug.Log("Gear num is :: " + gearNum);
                gearNum++;
                gameManager.ChangeGear();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gearNum++;
                gameManager.ChangeGear();
            }
        }
        if (engineRPM < minRPM && gearNum > 0)
        {
            gearNum--;
            gameManager.ChangeGear();
        }
    }

    private bool isGrounded()
    {
        if (wheelColliders[0].isGrounded && wheelColliders[0].isGrounded && wheelColliders[0].isGrounded && wheelColliders[0].isGrounded) { return true; }
        else
        {
            return false;
        }

    }

    private void Steering()
    {
        if (inputManager.horizontalInput > 0f)
        {
            wheelColliders[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontalInput;
            wheelColliders[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontalInput;
        }
        else if (inputManager.horizontalInput < 0f)
        {
            wheelColliders[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontalInput;
            wheelColliders[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontalInput;
        }
        else
        {
            wheelColliders[0].steerAngle = 0f;
            wheelColliders[1].steerAngle = 0f;
        }
    }
    private void brakeVehicle()
    {
        brakeTorque = (inputManager.verticalInput == -1 && wheelRpm > 1) ? 1205 : 0;
        wheelColliders[2].brakeTorque = wheelColliders[3].brakeTorque = (inputManager.handbrake) ? 100000 : 0;
    }
    private void RotateWheelTransform()
    {
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetWorldPose(out Vector3 pos, out Quaternion quat);
            wheelMeshes[i].transform.position = pos;
            wheelMeshes[i].transform.rotation = quat;
        }
    }

    private void AddDownForce()
    {
        carRb.AddForce(-transform.up * downForce * carRb.linearVelocity.magnitude);
    }

    private void GetFriction()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelHit wheelHit;
            wheelColliders[i].GetGroundHit(out wheelHit);

            slip[i] = wheelHit.sidewaysSlip;
        }
    }

    private void GetObjects()
    {
        inputManager = GetComponent<InputManager>();
        carRb = GetComponent<Rigidbody>();
        colliders = GameObject.Find("Colliders");
        meshes = GameObject.Find("Meshes");

        wheelColliders[0] = colliders.transform.Find("FrontLeftWheelCollider").gameObject.GetComponent<WheelCollider>();
        wheelColliders[1] = colliders.transform.Find("FrontRightWheelCollider").gameObject.GetComponent<WheelCollider>();
        wheelColliders[2] = colliders.transform.Find("RearLeftWheelCollider").gameObject.GetComponent<WheelCollider>();
        wheelColliders[3] = colliders.transform.Find("RearRightWheelCollider").gameObject.GetComponent<WheelCollider>();

        wheelMeshes[0] = meshes.transform.Find("FrontLeftWheel").transform;
        wheelMeshes[1] = meshes.transform.Find("FrontRightWheel").transform;
        wheelMeshes[2] = meshes.transform.Find("RearLeftWheel").transform;
        wheelMeshes[3] = meshes.transform.Find("RearRightWheel").transform;
    }

    //Drfiting

    private void DriftCar()
    {
        float velocity = 0f;
        WheelHit wheelHit;
        for (int i = 2; i < wheelColliders.Length; i++)
        {
            if (wheelColliders[i].GetGroundHit(out wheelHit))
            {
                radius = 4 + (-Mathf.Abs(wheelHit.sidewaysSlip) * 2) + carRb.linearVelocity.magnitude / 10;
                slip[i] = Mathf.Abs(wheelHit.forwardSlip) + Mathf.Abs(wheelHit.sidewaysSlip);

                forwardFriction = wheelColliders[i].forwardFriction;
                sidewaysFriction = wheelColliders[i].sidewaysFriction;
                if (inputManager.handbrake)
                {
                    forwardFriction.stiffness = Mathf.SmoothDamp(forwardFriction.stiffness, 0.5f, ref velocity, Time.deltaTime * 2); //old is 0.5f
                    sidewaysFriction.stiffness = Mathf.SmoothDamp(sidewaysFriction.stiffness, 0.8f, ref velocity, Time.deltaTime * 2);//old is 1

                }
                else
                {
                    forwardFriction.stiffness = Mathf.SmoothDamp(forwardFriction.stiffness, 1f, ref velocity, Time.deltaTime * 10);
                    sidewaysFriction.stiffness = Mathf.SmoothDamp(sidewaysFriction.stiffness, 1.3f, ref velocity, Time.deltaTime * 10);

                }

                wheelColliders[i].forwardFriction = forwardFriction;
                wheelColliders[i].sidewaysFriction = sidewaysFriction;

                if ((wheelHit.sidewaysSlip >= 0.3f) || (wheelHit.sidewaysSlip <= -0.3f) || (wheelHit.forwardSlip >= 0.3f) || (wheelHit.forwardSlip <= -0.3f))
                {
                    playSmokeParticles = true;
                    Debug.Log("Playing smoke particles");
                }
                else
                    playSmokeParticles = false;

                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }
    }
    public bool IsGrounded;
    [HideInInspector] public bool playSmokeParticles;

    private IEnumerator TimedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            radius = 6 + KPH / 20;
        }
    }

    public void ActivateNitrous()
    {
        if (!inputManager.nitrous && nitrousValue <= 10)
        {
            nitrousValue += Time.deltaTime / 3;
        }
        else
        {
            nitrousValue -= (nitrousValue <= 0) ? 0 : Time.deltaTime * 2;
        }

        if (inputManager.nitrous)
        {
            if (nitrousValue > 0)
            {
                StartNitrousEmitter();
            }
            else
            {
                StopNitrousEmitter();
            }
        }
        else
        {
            StopNitrousEmitter();
        }
    }

    public void StartNitrousEmitter()
    {
        carRb.AddForce(transform.forward * 5000);
        if (nitrousFlag) return;
        for (int i = 0; i < nitroSmoke.Length; i++)
        {
            nitroSmoke[i].Play();
        }
        nitrousFlag = true;
        
    }

    public void StopNitrousEmitter()
    {
        if (!nitrousFlag) return;
        for (int i = 0; i < nitroSmoke.Length; i++)
        {
            nitroSmoke[i].Stop();
        }
        nitrousFlag = false;
    }
}
