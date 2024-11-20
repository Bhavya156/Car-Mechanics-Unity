using System;
using UnityEngine;
using static UnityEditor.SceneView;

public class CameraEffects : MonoBehaviour
{
    public Controller controller;
    public float desiredFOV;
    public float defaultFOV;
    [Range(0, 5)] public float smoothTime;

    // Update is called once per frame
    private void FixedUpdate()
    {
        NitrousFOV();
    }

    private void NitrousFOV()
    {
        if (controller.nitrousFlag)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, desiredFOV, smoothTime * Time.deltaTime);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV, smoothTime * Time.deltaTime);
        }
    }
}
