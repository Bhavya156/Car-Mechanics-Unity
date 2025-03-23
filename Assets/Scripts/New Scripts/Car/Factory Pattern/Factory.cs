using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    public abstract IVehicle GetVehicle(Transform spawnPoint);

    public string GetLog(IVehicle vehicle) {
        string logMessage = "Factory: created product " + vehicle.VehicleName;
        return logMessage;
    }
}
