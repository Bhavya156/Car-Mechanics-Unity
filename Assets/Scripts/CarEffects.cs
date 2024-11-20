using UnityEngine;

public class CarEffects : MonoBehaviour
{
    private Controller carController;
    private InputManager inputManager;
    private bool smokeFlag = false;
    private bool tireMarksFlag;
    public ParticleSystem[] smokeParticles;
    public TrailRenderer[] tireTrails;
    public AudioSource skidClip;

    private void Start()
    {
        carController = GetComponent<Controller>();
        inputManager = GetComponent<InputManager>();
    }

    private void FixedUpdate()
    {
        ActivateSmoke();
        CheckDrift();
    }

    private void ActivateSmoke()
    {
        if (carController.playSmokeParticles)
        {
            StartSmoke();
        }
        else
        {
            StopSmoke();
        }

        if (smokeFlag)
        {
            for (int i = 0; i < smokeParticles.Length; i++)
            {
                var emission = smokeParticles[i].emission;
                emission.rateOverTime = ((int)carController.KPH * 10 <= 2000) ? (int)carController.KPH * 10 : 2000;
            }
        }
    }

    private void StartSmoke()
    {
        if (smokeFlag) return;
        for (int i = 0; i < smokeParticles.Length; i++)
        {
            var emission = smokeParticles[i].emission;
            emission.rateOverTime = ((int)carController.KPH * 2 <= 2000) ? (int)carController.KPH * 2 : 2000;
            smokeParticles[i].Play();
        }
        smokeFlag = true;
    }

    private void StopSmoke()
    {
        if (!smokeFlag) return;
        for (int i = 0; i < smokeParticles.Length; i++)
        {
            smokeParticles[i].Stop();
        }
        smokeFlag = false;
    }

    private void CheckDrift()
    {
        if (carController.playSmokeParticles) StartTireTrailEmitter();
        else StopTireTrailEmitter();
    }

    private void StartTireTrailEmitter()
    {
        if (tireMarksFlag) return;
        foreach (TrailRenderer tireMark in tireTrails)
        {
            tireMark.emitting = true;
        }
        skidClip.Play();
        tireMarksFlag = true;
    }

    private void StopTireTrailEmitter()
    {
        if (!tireMarksFlag) return;
        foreach (TrailRenderer tireMark in tireTrails)
        {
            tireMark.emitting = false;
        }
        skidClip.Stop();
        tireMarksFlag = false;
    }
}
