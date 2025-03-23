using UnityEngine;

public class VehicleFactory
{
    public static GameObject CreateVehicle(CarConfig carConfig, Transform parent)
    {
        GameObject newVehicle = Object.Instantiate(carConfig.carPrefab, parent.position, parent.rotation);
        return newVehicle;
    }
}

public class GarageManager : MonoBehaviour
{
    public VehicleList vehicleList;
    void Start()
    {
        InstantiateVehicles();
    }
    void InstantiateVehicles()
    {
        foreach (var myVehicle in vehicleList.vehicles)
        {
            VehicleFactory.CreateVehicle(myVehicle, myVehicle.carPrefab.transform);
        }
    }

    public void SelectCarButton() {
        //CarManager.Instance.SaveCarData();
    }
}