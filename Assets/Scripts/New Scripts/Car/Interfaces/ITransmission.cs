using UnityEngine;

public interface ITransmission
{
    void Shifter();
}

public class CarTransmission : ITransmission
{
    private int gearNum;
    private float engineRPM;
    private float maxRPM, minRPM;
    private float[] gears;
    private GameManager gameManager;

    public CarTransmission(float[] gearRatios, float maxRpm, float minRpm)
    {
        gears = gearRatios;
        maxRPM = maxRpm;
        minRPM = minRpm;
        // gameManager = manager;
    }

    public void Shifter()
    {
        if (engineRPM > maxRPM && gearNum < gears.Length - 1)
        {
            gearNum++;
            // gameManager.ChangeGear();
        }
        else if (engineRPM < minRPM && gearNum > 0)
        {
            gearNum--;
            // gameManager.ChangeGear();
        }
    }
}