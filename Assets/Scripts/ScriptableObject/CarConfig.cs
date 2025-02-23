using UnityEngine;

[CreateAssetMenu(fileName = "CarConfig", menuName = "Scriptable Objects/CarConfig")]
public class CarConfig : ScriptableObject
{
    public string carName;
    public GameObject carPrefab;
    public AnimationCurve engineCurve;
    public float topSpeed;
    public float[] gearDifferentialRatios;
    public float maxRPM, minRPM;
    public float downForce;
    public float nitrousPower;
}
