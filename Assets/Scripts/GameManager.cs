using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject newVehicle;
    private Controller carController;
    private CarEffects carEffects;
    public VehicleList vehicleList;
    public GameObject needle;
    public float startPosition = 125f, endPosition = -125f;
    public float desiredPosition;
    public TextMeshProUGUI RPM_text;
    public TextMeshProUGUI gear;
    public Slider nitrousSlider;
    public int vehicleSpeed;
    public int RPM;
    private int vehiclePointer;

    private CarConfig vehicleConfig;

    private void Awake()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateVehicle();
    }

    private void FixedUpdate()
    {
        Debug.Log((carController.engineRPM / 10000));
        RPM = (int)(carController.engineRPM);
        vehicleSpeed = (int)(carController.KPH);
        //RPM_text.text = RPM.ToString();
        RPM_text.text = vehicleSpeed.ToString();
        //vehicleSpeed = (int)(carController.KPH.To);
        UpdateNeedle();
        NitrousUI();
    }

    private void InstantiateVehicle()
    {
        vehiclePointer = PlayerPrefs.GetInt("pointer");
        vehicleConfig = vehicleList.vehicles[vehiclePointer];
        Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);
        newVehicle = Instantiate(vehicleConfig.carPrefab, new Vector3(-9f, 0f, -25f), rotation);
        carController = newVehicle.GetComponent<Controller>();
        AddComponents();
        carEffects = newVehicle.GetComponent<CarEffects>();
        
    }

    private void AddComponents() {
        carController.engineCurve = vehicleConfig.engineCurve;
        carController.topSpeed = vehicleConfig.topSpeed;
        carController.gears = vehicleConfig.gearDifferentialRatios;
        carController.maxRPM = vehicleConfig.maxRPM;
        carController.minRPM = vehicleConfig.minRPM;
        carController.downForce = vehicleConfig.downForce;
        carController.smoothTime = 0.1f;
        carController.gameManager = this.GetComponent<GameManager>();

        // carController.inputManager = newVehicle.AddComponent<InputManager>();
    }
    private void UpdateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = carController.engineRPM / 10000;
        //float temp = vehicleSpeed / 180f;
        needle.transform.eulerAngles = new Vector3(0f, 0f, startPosition - temp * desiredPosition);
    }

    public void ChangeGear()
    {
        gear.text = (!carController.reverse) ? (carController.gearNum + 1).ToString() : "R";
    }

    public void NitrousUI()
    {
        nitrousSlider.value = carEffects.nitrousValue / 39;
    }
}
