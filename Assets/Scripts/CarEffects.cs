using UnityEngine;

public class CarEffects : MonoBehaviour
{
    private Controller carController;
    private InputManager inputManager;
    private bool smokeFlag = false;
    private bool tireMarksFlag;
    public ParticleSystem[] smokeParticles;
    public TrailRenderer[] tireTrails;
    private AudioSource skidClip;
    private Rigidbody carRb;

    public ParticleSystem[] nitroSmoke;
    public float nitrousValue;
    public bool nitrousFlag;
    public float nitrousPower = 5000;

    private void Start()
    {
        carController = GetComponent<Controller>();
        inputManager = GetComponent<InputManager>();
        skidClip = GetComponent<AudioSource>();
        carRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ActivateSmoke();
        CheckDrift();
        ActivateNitrous();
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

    public void ActivateNitrous()
    {
        if (!inputManager.nitrous && nitrousValue <= 10)
        {
            nitrousValue += Time.deltaTime / 3;
        }
        else
        {
            nitrousValue -= (nitrousValue <= 0) ? 0 : Time.deltaTime * 2;
        }

        if (inputManager.nitrous)
        {
            if (nitrousValue > 0)
            {
                StartNitrousEmitter();
                if (carController.KPH < carController.topSpeed)
                {
                    carRb.AddForce(transform.forward * nitrousPower);
                }
            }
            else
            {
                StopNitrousEmitter();
            }
        }
        else
        {
            StopNitrousEmitter();
        }
    }


    public void StartNitrousEmitter()
    {
        carRb.AddForce(transform.forward * nitrousPower);
        if (nitrousFlag) return;
        for (int i = 0; i < nitroSmoke.Length; i++)
        {
            nitroSmoke[i].Play();
        }
        nitrousFlag = true;

    }

    public void StopNitrousEmitter()
    {
        if (!nitrousFlag) return;
        for (int i = 0; i < nitroSmoke.Length; i++)
        {
            nitroSmoke[i].Stop();
        }
        nitrousFlag = false;
    }
}
