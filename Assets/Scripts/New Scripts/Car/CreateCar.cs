using System.Collections.Generic;
using UnityEngine;

public class CreateCar : MonoBehaviour
{
    [SerializeField]
    private Factory m_Factory;
    [SerializeField]
    private Transform m_CarSpawnPoint;
    [SerializeField]
    private List<GameObject> m_CreatedCar = new();

    void Start()
    {
        GetCarOnStart();
    }
    
    void GetCarOnStart()
    {
        Factory selectedFactory = m_Factory;

        IVehicle vehicle = selectedFactory.GetVehicle(m_CarSpawnPoint);

        if (vehicle is Component component)
        {
            m_CreatedCar.Add(component.gameObject);
        }
    }
}
