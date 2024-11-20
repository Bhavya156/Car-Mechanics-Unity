using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Controller carController;

    public GameObject needle;
    public float startPosition = 125f, endPosition = -125f;
    public float desiredPosition;
    public TextMeshProUGUI RPM_text;
    public TextMeshProUGUI gear;
    public Slider nitrousSlider;
    public int vehicleSpeed;
    public int RPM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.Log((carController.engineRPM / 10000));
        RPM = (int)(carController.engineRPM );
        vehicleSpeed = (int)(carController.KPH);
        //RPM_text.text = RPM.ToString();
        RPM_text.text = vehicleSpeed.ToString();
        //vehicleSpeed = (int)(carController.KPH.To);
        UpdateNeedle();
        NitrousUI();
    }

    private void UpdateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = carController.engineRPM / 10000;
        //float temp = vehicleSpeed / 180f;
        needle.transform.eulerAngles = new Vector3(0f, 0f, (startPosition - temp * desiredPosition));
    }

    public void ChangeGear()
    {
        gear.text = (!carController.reverse) ? (carController.gearNum + 1).ToString() : "R";
    }

    public void  NitrousUI()
    {
        nitrousSlider.value = carController.nitrousValue / 39;
    }
}
