using System;
using System.IO;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public static CarManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveCarData(string carName, CarConfig carConfig) {
        CarConfiguration newCarConfiguration = new CarConfiguration();
        newCarConfiguration.m_carName = carName;
        newCarConfiguration.m_carConfig = carConfig;

        string json = JsonUtility.ToJson(newCarConfiguration);
        File.WriteAllText(Application.persistentDataPath + "/saveCarConfiguration.json", json);
    }
}


[Serializable]
public class CarConfiguration {
    public string m_carName;
    public CarConfig m_carConfig;
}