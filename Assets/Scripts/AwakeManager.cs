using UnityEngine;
using UnityEngine.SceneManagement;

public class AwakeManager : MonoBehaviour
{
    public VehicleList vehicleList;
    public int vehiclePointer;
    public GameObject vehicleParent;

    private void Awake()
    {
        PlayerPrefs.SetInt("pointer", 0);

        InstantiateVehicles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (vehiclePointer < vehicleList.vehicles.Length - 1)
            {
                vehiclePointer++;
                PlayerPrefs.SetInt("pointer", vehiclePointer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (vehiclePointer > 0)
            {
                vehiclePointer--;
                PlayerPrefs.SetInt("pointer", vehiclePointer);
            }
        }

        if (vehiclePointer >= 0 && vehiclePointer < vehicleList.cameraPoints.Length)
        {
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                vehicleList.cameraPoints[vehiclePointer],
                Time.deltaTime * 5f
            );
        }
    }

    private void InstantiateVehicles()
    {
        foreach (var myVehicle in vehicleList.vehicles)
        {
            GameObject newVehicle = Instantiate(myVehicle.carPrefab, myVehicle.carPrefab.transform.position, myVehicle.carPrefab.transform.rotation);
            newVehicle.transform.parent = vehicleParent.transform;
        }
    }

    public void StartGameButton()
    {
        SceneManager.LoadScene("CarController_Scene");
    }
}
